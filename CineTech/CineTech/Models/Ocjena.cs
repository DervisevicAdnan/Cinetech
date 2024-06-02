using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
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
        [Display(Name = "Ocjena:")]
        public int ocjenaFilma { get; set; }
        [StringLength(maximumLength:50,ErrorMessage = "Prekoračili ste dužinu od 50")]
        [Display(Name = "Komentar:")]
        public String komentar { get; set; }
        [DataType(DataType.Date)]
        public DateTime datum { get; set; }
        [ForeignKey("Korisnik")]
        public String korisnikId { get; set; }
        [ForeignKey("Film")]
        public int FilmId { get; set; } 
        //public IdentityUser korisnik { get; set; }
        public Ocjena() { }
    }
}
