using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class ZauzetaSjedista
    {
        [Key]
        public int id { get; set; }
        public int red { get; set; }
        public int redniBrojSjedista { get; set; }
        [ForeignKey("Projekcija")]
        public int ProjekcijaId { get; set; }
        public Projekcija  Projekcija { get; set; } 
        public ZauzetaSjedista() { }
    }
}
