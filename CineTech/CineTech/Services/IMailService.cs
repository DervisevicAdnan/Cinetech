using CineTech.Models;

namespace CineTech.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
