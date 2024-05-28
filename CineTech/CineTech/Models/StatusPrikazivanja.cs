using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public enum StatusPrikazivanja
    {
       [Display(Name = "UNajavi")] 
        UNajavi, 
       [Display(Name = "Aktuelan")] 
        Aktuelan, 
       [Display(Name = "Arhiviran")] 
        Arhiviran
    }
}
