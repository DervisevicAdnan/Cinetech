using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Kupovina:Transakcija
    {
        public double cijena { get; set; }
        public Kupovina() { }
    }
}
