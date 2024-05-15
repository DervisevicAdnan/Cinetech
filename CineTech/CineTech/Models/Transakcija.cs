using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public abstract class Transakcija
    {
        [Key]
        public int id { get; set; }
        public DateOnly datum { get; set; }
        public TimeOnly vrijeme { get; set; }
        [ForeignKey("Korisnik")]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        [ForeignKey("Transakcija")]
        public int TransakcijaId {  get; set; }
        public Transakcija transakcija { get; set; }
    }
}
