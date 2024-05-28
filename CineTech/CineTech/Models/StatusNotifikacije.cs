using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public enum StatusNotifikacije
    {
        [Display(Name="NaCekanju")]
        NaCekanju, 
        [Display(Name="Dostavljena")]
        Dostavljena
    }
}
