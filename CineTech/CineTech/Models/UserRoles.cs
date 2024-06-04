using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class UserRoles
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Korisnik")]
        public String korisnikId { get; set; }
        [ForeignKey("Roles")]
        public int roleId { get; set; }
       // public Korisnik korisnik {get; set;}
        //public Roles rola { get; set; } 
        public UserRoles() { }
    }
}
