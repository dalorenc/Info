using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Info.Models
{
    public class Category
    {
        [Key] //zbędne oznaczenie, gdyż w nazwie pola występuje ciąg Id
        [Display(Name = "Identyfikator kategorii:")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kategorii.")]
        [Display(Name = "Nazwa kategorii: ")]
        [MaxLength(50, ErrorMessage = "Nazwa kategorii nie może być dłuższy niż 50 znaków.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Proszę podać opis kategorii.")]
        [Display(Name = "Opis kategorii: ")]
        [MaxLength(255, ErrorMessage = "Opis kategorii nie może być dłuższy niż 255 znaków.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę ikony.")]
        [Display(Name = "Ikona kategorii: ")]
        [MaxLength(30, ErrorMessage = "Opis ikony nie może być dłuższy niż 30 znaków.")]
        public string? Icon { get; set; }

        [Required]
        [Display(Name = "Czy aktywna?")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Czy aktywna?")]
        [DefaultValue(true)]
        public bool Display { get; set; }
    }
}