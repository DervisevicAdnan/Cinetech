using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class ZauzetaSjedista
    {
        [Key]
        public int id { get; set; }
        
        [Required]
        [Display(Name = "Red:")]
        public int red { get; set; }

        [Required]
        [Display(Name = "Redni broj sjedista:")]
        public int redniBrojSjedista { get; set; }

        [ForeignKey("Projekcija")]
        [Display(Name = "Projekcija:")]
        public int ProjekcijaId { get; set; }

        [ForeignKey("Transakcija")]
        [Display(Name = "Transakcija:")]
        public int TransakcijaId { get; set; }
        public ZauzetaSjedista() { }
    }
}
