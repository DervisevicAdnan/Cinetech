using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class NotifikacijeFilma
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Film")]
        public int FilmId { get; set; }
        [ForeignKey("Notifikacija")]
        public int NotifikacijaId { get; set; }
        //public Film Film { get; set; }
        //public Notifikacija Notifikacija { get; set; }
        public NotifikacijeFilma() { }
    }
}
