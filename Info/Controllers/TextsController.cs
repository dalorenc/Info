using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Info.Data;
using Info.Models;
using Info.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Info.Infrastructure;

namespace Info.Controllers
{
    public class TextsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _hostEnvironment;

        public TextsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostEnvironment = environment;
        }

        // GET: Texts
        public async Task<IActionResult> Index(string Fraza, string Autor, int? Kategoria, int PageNumber = 1)
        {
            var SelectedTexts = _context.Texts?
                .Include(t => t.Category)
                .Include(t => t.User)
                .Where(t => t.Active == true)
                .OrderByDescending(t => t.AddedDate);

            if (Kategoria != null)
            {
                SelectedTexts = (IOrderedQueryable<Text>)SelectedTexts.Where(r => r.Category.CategoryId == Kategoria);
            }
            if (!String.IsNullOrEmpty(Autor))
            {
                SelectedTexts = (IOrderedQueryable<Text>)SelectedTexts.Where(r => r.User.Id == Autor);
            }
            if (!String.IsNullOrEmpty(Fraza))
            {
                SelectedTexts = (IOrderedQueryable<Text>)SelectedTexts.Where(r => r.Content.Contains(Fraza));
            }

            TextsViewModel textsViewModel = new();
            textsViewModel.TextsView = new TextsView();

            textsViewModel.TextsView.TextCount = SelectedTexts.Count();
            textsViewModel.TextsView.PageNumber = PageNumber;
            textsViewModel.TextsView.Author = Autor;
            textsViewModel.TextsView.Phrase = Fraza;
            textsViewModel.TextsView.Category = Kategoria;

            textsViewModel.Texts = (IEnumerable<Text>?)await SelectedTexts
                .Skip((PageNumber - 1) * textsViewModel.TextsView.PageSize)
                .Take(textsViewModel.TextsView.PageSize)
                .ToListAsync();

            ViewData["Category"] = new SelectList(_context.Categories?
                .Where(c => c.Active == true),
                "CategoryId", "Name", Kategoria);

            ViewData["Author"] = new SelectList(_context.Texts
                .Include(u => u.User)
                .Select(u => u.User)
                .Distinct(),
                "Id", "FullName", Autor);

            return View(textsViewModel);
        }
        // GET: Texts
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> List()
        {
            var applicationDbContext = _context.Texts.Include(t => t.Category).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Texts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Texts == null)
            {
                return NotFound();
            }

            var text = await _context.Texts
                .Include(t => t.Category)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TextId == id);
            if (text == null)
            {
                return NotFound();
            }

            return View(text);
        }

        // GET: Texts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Texts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TextId,Title,Summary,Keywords,Content,Active,CategoryId")] Text text, IFormFile? picture)
        {
            if (ModelState.IsValid)
            {
                //odczytywanie identyfikatora użytkownika i daty
                text.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                text.AddedDate = DateTime.Now;

                if (picture != null && picture.Length > 0)
                {
                    ImageFileUpload imageFileResult = new(_hostEnvironment);
                    FileSendResult fileSendResult = imageFileResult.SendFile(picture, "img", 600);
                    if (fileSendResult.Success)
                    {
                        text.Graphic = fileSendResult.Name;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Wybrany plik nie jest obrazkiem!";
                        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", text.CategoryId);
                        return View(text);
                    }
                }




                _context.Add(text);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", text.CategoryId);
            return View(text);
        }

        // GET: Texts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Texts == null)
            {
                return NotFound();
            }

            var text = await _context.Texts.FindAsync(id);
            if (text == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Description", text.CategoryId);
            ViewData["Id"] = new SelectList(_context.AppUsers, "Id", "Id", text.Id);
            return View(text);
        }

        // POST: Texts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TextId,Title,Summary,Keywords,Content,Graphic,Active,AddedDate,CategoryId,Id")] Text text)
        {
            if (id != text.TextId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(text);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TextExists(text.TextId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Description", text.CategoryId);
            ViewData["Id"] = new SelectList(_context.AppUsers, "Id", "Id", text.Id);
            return View(text);
        }

        // GET: Texts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Texts == null)
            {
                return NotFound();
            }

            var text = await _context.Texts
                .Include(t => t.Category)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TextId == id);
            if (text == null)
            {
                return NotFound();
            }

            return View(text);
        }

        // POST: Texts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Texts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Texts'  is null.");
            }
            var text = await _context.Texts.FindAsync(id);
            if (text != null)
            {
                _context.Texts.Remove(text);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextExists(int id)
        {
          return _context.Texts.Any(e => e.TextId == id);
        }
    }
}
