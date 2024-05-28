using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Ocjena
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Range(minimum:1,maximum:5,ErrorMessage ="Ocjena mora biti između 1-5 !")]
        public int ocjena { get; set; }
        [StringLength(maximumLength:50,ErrorMessage = "Prekoračili ste dužinu od 50")]
        public String komentar { get; set; }
        public DateTime datum { get; set; }
        [ForeignKey("Korisnik")]
        public String korisnikId { get; set; }
        public Korisnik korisnik { get; set; }
        public Ocjena() { }
    }
}
