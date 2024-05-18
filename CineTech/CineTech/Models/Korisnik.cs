using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace CineTech.Models
{
    public class Korisnik : IdentityUser
    {
        public Korisnik() { }   
        public String imePrezime { get; set; }
        public String password { get; set; }
    }
}
