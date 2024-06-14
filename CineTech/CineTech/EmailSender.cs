using Humanizer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net.Mail;
using System.Net;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using CineTech.Models;

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

        public async Task<bool> SendHTMLEmailAsync(HTMLMailData htmlMailData)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("companycinetech@gmail.com");
                message.To.Add(new MailAddress(htmlMailData.EmailToId));
                message.Subject = "Račun: " + htmlMailData.NazivFilma;
                message.IsBodyHtml = true;

                string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Racun.html";
                string emailTemplateText = File.ReadAllText(filePath);

                string sjedista = "";
                int b = 1;
                foreach (ZauzetaSjedista z in htmlMailData.ZauzetaSjedista)
                {
                    sjedista += "                <tr>\r\n" +
                                "                   <td>" + b + "</td>\r\n" +
                                "                   <td>Red " + z.red.ToString() + "</td>\r\n" +
                                "                   <td>Sjedište " + z.redniBrojSjedista.ToString() + "</td>\r\n" +
                                "                </tr>";
                    b++;
                }

                emailTemplateText = string.Format(emailTemplateText, htmlMailData.NazivFilma, htmlMailData.TerminProjekcije, htmlMailData.NazivSale, sjedista);

                message.Body = emailTemplateText;

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
        public async Task<bool> SendRezervacijaMail(HTMLMailData htmlMailData)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("companycinetech@gmail.com");
                message.To.Add(new MailAddress(htmlMailData.EmailToId));
                message.Subject = "Rezervacija: " + htmlMailData.NazivFilma;
                message.IsBodyHtml = true;

                string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Rezervacija.html";
                string emailTemplateText = File.ReadAllText(filePath);

                string sjedista = "";
                int b = 1;
                foreach (ZauzetaSjedista z in htmlMailData.ZauzetaSjedista)
                {
                    sjedista += "                <tr>\r\n" +
                                "                   <td>" + b + "</td>\r\n" +
                                "                   <td>Red " + z.red.ToString() + "</td>\r\n" +
                                "                   <td>Sjedište " + z.redniBrojSjedista.ToString() + "</td>\r\n" +
                                "                </tr>";
                    b++;
                }

                emailTemplateText = string.Format(emailTemplateText, htmlMailData.NazivFilma, htmlMailData.TerminProjekcije, htmlMailData.NazivSale, sjedista);

                message.Body = emailTemplateText;

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

        public async Task<bool> SendNotifikacijaMail(NotifikacijaMailData htmlMailData)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress("companycinetech@gmail.com");
                message.To.Add(new MailAddress(htmlMailData.EmailToId));
                message.Subject = "Premijera: " + htmlMailData.NazivFilma;
                message.IsBodyHtml = true;

                string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Notifikacija.html";
                string emailTemplateText = File.ReadAllText(filePath);
                emailTemplateText = string.Format(emailTemplateText, htmlMailData.NazivFilma, htmlMailData.DatumPredstavljanja);

                message.Body = emailTemplateText;

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
