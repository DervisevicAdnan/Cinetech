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

        public Task SetImePrezime(Korisnik user, string normalizedName)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }
    }
}
