using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Film
    {
        [Key]
        public int id { get; set; }
        [Required]
        [Display(Name = "Naziv Filma:")]
        public String naziv { get; set; }
        [Required]
        [Display(Name = "Naslovna slika:")]
        public String naslovnaSlika { get; set; }
        [Required]
        [MaxLength(500)]
        [Display(Name = "Opis radnje:")]
        public String opis { get; set; }
        [Required]
        [Display(Name = "Redatelj:")]
        public String redatelj { get; set; }
        [Required]
        [Display(Name = "Glumci:")]
        public String glumci { get; set; }
        [Required]
        [Display(Name = "Relese Date:")]
        [DataType(DataType.Date)]

        public DateTime releseDate { get; set; }
        [Required]
        [Display(Name = "Trailer:")]
        public String trailer { get; set; }
        [Required]
        [EnumDataType(typeof(StatusPrikazivanja))]
        [Display(Name = "Status prikazivanja:")]
        public StatusPrikazivanja StatusPrikazivanja { get; set; }
        public Film() { }
}
}
