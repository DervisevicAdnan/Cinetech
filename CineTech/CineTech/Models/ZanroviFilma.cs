using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class ZanroviFilma
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Film")]
        public int idFilma { get; set; }
        public Film Film { get; set; }
        public Zanr zanrFilma { get; set; }
        public ZanroviFilma() { }
    }
}
