using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Film
    {
        [Key]
        public int id { get; set; }
        public String naziv { get; set; }
        public String naslovnaSlika { get; set; }
        public String opis { get; set; }
        public String redatelj { get; set; }
        public String glumci { get; set; }
        public DateTime releseDate { get; set; }
        public String trailer { get; set; }
        public StatusPrikazivanja StatusPrikazivanja { get; set; }
        public Film() { }
}
}
