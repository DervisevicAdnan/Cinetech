using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public abstract class Transakcija
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Datum transakcije:")]
        public DateOnly datum { get; set; }
        [Display(Name = "Vrijeme transakcije:")]
        public TimeOnly vrijeme { get; set; }
        [ForeignKey("AspNetUsers")]
        [Display(Name = "KorisnikID")]
        public String KorisnikId { get; set; }
        public IdentityUser Korisnik { get; set; }
        [ForeignKey("ZauzetaSjedista")]
        [Display(Name = "Zauzeta sjedista")]
        public int ZauzetaSjedistaId {  get; set; }
        public ZauzetaSjedista ZauzetaSjedista { get; set; }
        public Transakcija() { }
    }
}
