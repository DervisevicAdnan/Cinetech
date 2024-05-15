using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public class Roles
    {
        [Key]
        public int id { get; set; }
        public String naziv { get; set; }
    }
}
