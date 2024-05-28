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
        public String imePrezime { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 8, ErrorMessage = "Vaš password nema minimalnih 8 karatkera")]
        public String password { get; set; }
    }
}
