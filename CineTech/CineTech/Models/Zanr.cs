using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public enum Zanr
    {
        [Display(Name="Drama")]
        Drama, 
        [Display(Name="Komedija")]
        Komedija, 
        [Display(Name="Akcija")]
        Akcija,
        [Display(Name="Dokumentarni")]
        Dokumentarni, 
        [Display(Name="Horor")]
        Horor, 
        [Display(Name="Historijski")]
        Historijski,
        [Display(Name="SciFi")]
        SciFi
    }
}
