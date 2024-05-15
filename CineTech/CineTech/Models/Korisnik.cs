using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Korisnik
    {
        [Key]
        public int id { get; set; }
        public String korisnickoIme { get; set; }
        public String imePreizime { get; set; }
        public String email { get; set; }
        public String telefon { get; set; }
        public String password { get; set; }
    }
}
