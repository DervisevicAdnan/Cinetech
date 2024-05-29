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
        public int brojRedova { get; set; }
        [Required]
        [Display(Name = "Broj kolona:")]
        public int brojKolona { get; set; }
        public KinoSala() { }
}
}
