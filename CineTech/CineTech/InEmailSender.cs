using CineTech.Models;

namespace CineTech
{
    public interface InEmailSender
    {
            Task<bool> SendEmailAsync(string email, string subject, string confirmLink);
            Task<bool> SendHTMLEmailAsync(HTMLMailData htmlMailData);
            Task<bool> SendRezervacijaMail(HTMLMailData htmlMailData);
            Task<bool> SendNotifikacijaMail(NotifikacijaMailData htmlMailData);
    }
}
