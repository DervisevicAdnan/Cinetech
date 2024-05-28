using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class KinoSala
    {
        [Key]
        public int id { get; set; }
        [Required]
        public String naziv { get; set; }
        [Required]
        public int brojRedova { get; set; }
        [Required]
        public int brojKolona { get; set; }
        public KinoSala() { }
}
}
