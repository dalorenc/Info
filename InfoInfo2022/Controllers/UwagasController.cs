using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using info_2022.Data;
using info_2022.Models;

namespace info_2022.Controllers
{
    public class UwagasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UwagasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Uwagas
        public async Task<IActionResult> Index()
        {
              return View(await _context.Uwaga.ToListAsync());
        }

        // GET: Uwagas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Uwaga == null)
            {
                return NotFound();
            }

            var uwaga = await _context.Uwaga
                .FirstOrDefaultAsync(m => m.IdUwaga == id);
            if (uwaga == null)
            {
                return NotFound();
            }

            return View(uwaga);
        }

        // GET: Uwagas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uwagas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUwaga,Imie,Adres,TekstUwaga,Rozpatrzone")] Uwaga uwaga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uwaga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uwaga);
        }

        // GET: Uwagas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Uwaga == null)
            {
                return NotFound();
            }

            var uwaga = await _context.Uwaga.FindAsync(id);
            if (uwaga == null)
            {
                return NotFound();
            }
            return View(uwaga);
        }

        // POST: Uwagas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUwaga,Imie,Adres,TekstUwaga,Rozpatrzone")] Uwaga uwaga)
        {
            if (id != uwaga.IdUwaga)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uwaga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UwagaExists(uwaga.IdUwaga))
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
            return View(uwaga);
        }

        // GET: Uwagas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Uwaga == null)
            {
                return NotFound();
            }

            var uwaga = await _context.Uwaga
                .FirstOrDefaultAsync(m => m.IdUwaga == id);
            if (uwaga == null)
            {
                return NotFound();
            }

            return View(uwaga);
        }

        // POST: Uwagas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Uwaga == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Uwaga'  is null.");
            }
            var uwaga = await _context.Uwaga.FindAsync(id);
            if (uwaga != null)
            {
                _context.Uwaga.Remove(uwaga);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UwagaExists(int id)
        {
          return _context.Uwaga.Any(e => e.IdUwaga == id);
        }
    }
}
