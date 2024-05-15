using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class OcjeneFilma
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Film")]
        public int FilmId { get; set; }
        [ForeignKey("Ocjena")]
        public int OcjenaId { get; set; }
        public Film Film { get; set; }
        public Ocjena Ocjena { get; set; }
    }
}
