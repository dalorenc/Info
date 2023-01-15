using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace info_2022.Models
{
    public class Category
    {
        [Key]
        [Display(Name = "Identyfikator kategorii")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kategorii")]
        [Display(Name = "Nazwa kategorii")]
        [MaxLength(50, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 50 znaków")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Proszę podać opis kategorii")]
        [Display(Name = "Opis kategorii")]
        [MaxLength(255, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 255 znaków")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę ikony")]
        [Display(Name = "Nazwa ikony")]
        [MaxLength(30, ErrorMessage = "Nazwa ikony nie może być dłuższa niż 30 znaków")]
        public string? Icon { get; set; }

        [Required]
        [Display(Name = "Czy kategoria aktywna")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Czy wyświetlać")]
        public bool Display { get; set; }

        public virtual List<Text>? Texts { get; set; }
    }
}
