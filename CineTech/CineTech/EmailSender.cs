using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
using System.Net;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CineTech
{
    public class EmailSender:InEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("companycinetech@gmail.com");
                message.To.Add(new MailAddress(email));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = confirmLink;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("companycinetech@gmail.com", "elru xlle xgpi amzn"),
                    EnableSsl = true,
                };
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                // Log detailed SMTP exceptions
                _logger.LogError($"SMTP Error: {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Log other exceptions
                _logger.LogError($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
