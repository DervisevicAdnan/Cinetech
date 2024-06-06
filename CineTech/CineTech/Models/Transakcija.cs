using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public abstract class Transakcija
    {
        [Key]
        public int id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum transakcije:")]
        public DateTime datum { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Vrijeme transakcije:")]
        public DateTime vrijeme { get; set; }

        [ForeignKey("IdentityUser")]
        [Display(Name = "KorisnikID")]
        public String KorisnikId { get; set; }

       // public IdentityUser Korisnik { get; set; }
        [ForeignKey("ZauzetaSjedista")]
        [Display(Name = "Zauzeta sjedista:")]
        public int ZauzetaSjedistaId {  get; set; }

        //public ZauzetaSjedista ZauzetaSjedista { get; set; }
        public Transakcija() { }
    }
}
