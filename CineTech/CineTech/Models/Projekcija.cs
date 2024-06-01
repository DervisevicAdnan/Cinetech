using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Projekcija
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Datum projekcije:")]
        public DateTime datum { get; set; }
        [Display(Name = "Vrijeme projekcije:")]
        public TimeOnly vrijeme { get; set; }
        [Required]
        [Display(Name = "Cijena osnovne karte:")]
        public double cijenaOsnovneKarte { get; set; }
        [ForeignKey("KinoSala")]
        [Display(Name = "KinoSalaID")]
        public int kinoSalaId { get; set; }
        //public KinoSala kinoSala { get; set; }
        [ForeignKey("Film")]
        [Display(Name = "FilmID")]
        public int filmId { get; set; }
       // public Film Film { get; set; }

        public Projekcija() { }

    }
}
