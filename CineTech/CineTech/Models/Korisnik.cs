using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Korisnik : IdentityUser
    {
        public Korisnik() { }
        [Required]
        [Display(Name = "Ime i prezime:")]
        public String imePrezime { get; set; }

    }
}
