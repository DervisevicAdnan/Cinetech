using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineTech.Models
{
    public class ZanroviFilma
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Film")]
        [Display(Name = "FilmId:")]
        public int idFilma { get; set; }
        public Film Film { get; set; }
        [EnumDataType(typeof(Zanr))]
        [Display(Name = "Zanr:")]
        public Zanr zanrFilma { get; set; }
        public ZanroviFilma() { }
    }
}
