using System.ComponentModel.DataAnnotations;
using System.Net;

public class ValidateDate : ValidationAttribute
{
    protected override ValidationResult IsValid
    (object date, ValidationContext validationContext)
    {
        return ((DateTime)date >= DateTime.Now)
        ? ValidationResult.Success
        : new ValidationResult("Validan je upis današnjeg dana pa u budućnost!"); 
    }
}