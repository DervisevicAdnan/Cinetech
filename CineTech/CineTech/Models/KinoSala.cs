using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class KinoSala
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Naziv sale")]
        public String naziv { get; set; }

        [Required]
        [Display(Name = "Broj redova:")]
        [Range(0, int.MaxValue, ErrorMessage = "Broj redova ne može biti manji od 0.")]
        public int brojRedova { get; set; }

        [Required]
        [Display(Name = "Broj kolona:")]
        [Range(0, int.MaxValue, ErrorMessage = "Broj redova ne može biti manji od 0.")]

        public int brojKolona { get; set; }

        public KinoSala() { }
}
}
