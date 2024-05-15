using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class KinoSala
    {
        [Key]
        public int id { get; set; }
        public String naziv { get; set; }
        public int brojRedova { get; set; }
        public int brojKolona { get; set; }
}
}
