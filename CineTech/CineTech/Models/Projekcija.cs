using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Projekcija
    {
        [Key]
        public int id { get; set; }
        public DateTime datum { get; set; }
        public TimeOnly vrijeme { get; set; }
        [Required]
        public double cijenaOsnovneKarte { get; set; }
        [ForeignKey("KinoSala")]
        public int kinoSalaId { get; set; }
        public KinoSala kinoSala { get; set; }
        [ForeignKey("Film")]
        public int filmId { get; set; }
        public Film Film { get; set; }

        public Projekcija() { }

    }
}
