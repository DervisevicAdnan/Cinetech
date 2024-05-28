using System.ComponentModel.DataAnnotations;

namespace CineTech.Models
{
    public enum PeriodNotifikacije
    {
        [Display(Name = "DanPrije")]
        DanPrije,
        [Display(Name ="SedmicuPrije")] 
        SedmicuPrije,
        [Display(Name ="MjesecPrije")] 
        MjesecPrije, 
        [Display(Name="PriOdredjivanjuDatuma")]
        PriOdredjivanjuDatuma
    }
}
