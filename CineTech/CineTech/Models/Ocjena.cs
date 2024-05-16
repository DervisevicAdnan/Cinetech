using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Ocjena
    {
        [Key]
        public int id { get; set; }
        public int ocjena { get; set; }
        public String komentar { get; set; }
        public DateTime datum { get; set; }
        [ForeignKey("Korisnik")]
        public String korisnikId { get; set; }
        public Korisnik korisnik { get; set; }
        public Ocjena() { }
    }
}
