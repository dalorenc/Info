using System.ComponentModel.DataAnnotations;

namespace info_2022.Models
{
    public class Uwaga
    {
        [Key]
        [Display(Name = "IdUwaga")]
        public int IdUwaga { get; set; }

        [Display(Name = "Imie")]
        public string? Imie { get; set; }

        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [Display(Name = "TekstUwaga")]
        public string? TekstUwaga { get; set; }

        [Display(Name = "Rozpatrzone")]
        public bool Rozpatrzone { get; set; }
    }
}
