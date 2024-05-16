using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace CineTech.Models
{
    public class Korisnik : IdentityUser
    {
        public Korisnik() { }   
        public String korisnickoIme { get; set; }
        public String imePreizime { get; set; }
        public String email { get; set; }
        public String telefon { get; set; }
        public String password { get; set; }
    }
}
