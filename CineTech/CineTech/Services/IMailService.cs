using CineTech.Models;

namespace CineTech.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
        Task<bool> SendMailAsync(MailData mailData);
        bool SendHTMLMail(HTMLMailData htmlMailData);
        bool SendRezervacijaMail(HTMLMailData htmlMailData);
    }
}
