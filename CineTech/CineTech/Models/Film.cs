using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Film
    {
        [Key]
        public int id { get; set; }
        [Required]
        public String naziv { get; set; }
        [Required]
        public String naslovnaSlika { get; set; }
        [Required]
        [MaxLength(500)]
        public String opis { get; set; }
        [Required]
        public String redatelj { get; set; }
        [Required]
        public String glumci { get; set; }
        [Required]
        public DateTime releseDate { get; set; }
        [Required]
        public String trailer { get; set; }
        [Required]
        [EnumDataType(typeof(StatusPrikazivanja))]
        public StatusPrikazivanja StatusPrikazivanja { get; set; }
        public Film() { }
}
}
