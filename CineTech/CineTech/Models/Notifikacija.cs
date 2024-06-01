using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class Notifikacija
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Korisnik")]
        [Display(Name = "Korisnik")]
        public String KorisnikId {  get; set; }
        //public Korisnik Korisnik { get; set; }
        [Required]
        [EnumDataType(typeof(PeriodNotifikacije))]
        [Display(Name = "Period notifikacije:")]
        public PeriodNotifikacije PeriodNotifikacije { get; set; }
        [Required]
        [EnumDataType(typeof(StatusNotifikacije))]
        [Display(Name = "Status notifikacije:")]
        public StatusNotifikacije StatusNotifikacije { get; set; }
        public Notifikacija() { }
    }
}
