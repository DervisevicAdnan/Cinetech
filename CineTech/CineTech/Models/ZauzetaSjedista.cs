using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class ZauzetaSjedista
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Red:")]
        public int red { get; set; }
        [Display(Name = "Redni broj sjedista:")]
        public int redniBrojSjedista { get; set; }
        [ForeignKey("Projekcija")]
        [Display(Name = "Projekcija")]
        public int ProjekcijaId { get; set; }
        public Projekcija  Projekcija { get; set; } 
        public ZauzetaSjedista() { }
    }
}
