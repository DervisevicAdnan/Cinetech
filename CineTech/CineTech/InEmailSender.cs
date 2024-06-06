namespace CineTech
{
    public interface InEmailSender
    {
            Task<bool> SendEmailAsync(string email, string subject, string confirmLink);
    }
}
