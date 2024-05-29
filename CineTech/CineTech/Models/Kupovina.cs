using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Kupovina:Transakcija
    {
        [Required]
        [Display(Name = "Cijena:")]
        public double cijena { get; set; }
        public Kupovina() { }
    }
}
