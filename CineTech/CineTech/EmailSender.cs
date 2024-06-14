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

                string emailTemplateText = "<!DOCTYPE html>\r\n<html lang=\"hr\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f9f9f9;\r\n        }}\r\n\r\n        .container {{\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            border: 1px solid #dddddd;\r\n            padding: 20px;\r\n        }}\r\n\r\n        .header {{\r\n            text-align: center;\r\n            border-bottom: 1px solid #dddddd;\r\n            padding-bottom: 10px;\r\n        }}\r\n\r\n        .header img {{\r\n            max-width: 150px;\r\n            height: auto;\r\n        }}\r\n\r\n        .details {{\r\n            margin: 20px 0;\r\n        }}\r\n\r\n        .details h2 {{\r\n            margin: 0;\r\n            color: #333333;\r\n        }}\r\n\r\n        .details p {{\r\n            margin: 5px 0;\r\n            color: #666666;\r\n        }}\r\n\r\n        .seats {{\r\n            width: 100%;\r\n            border-collapse: collapse;\r\n            margin: 20px 0;\r\n        }}\r\n\r\n        .seats th, .seats td {{\r\n            border: 1px solid #dddddd;\r\n            text-align: left;\r\n            padding: 8px;\r\n        }}\r\n\r\n        .seats th {{\r\n            background-color: #f2f2f2;\r\n        }}\r\n\r\n        .footer {{\r\n            text-align: center;\r\n            margin-top: 20px;\r\n            color: #999999;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <img src=\"https://i.ibb.co/Trjr29Z/logoSivi.png\" alt=\"Cinetech Logo\">\r\n        </div>\r\n        <div class=\"details\">\r\n            <h2>Račun</h2>\r\n            <p><strong>Ime filma:</strong> {0}</p>\r\n            <p><strong>Vrijeme projekcije:</strong> {1}</p>\r\n            <p><strong>Sala:</strong> {2}</p>\r\n        </div>\r\n        <table class=\"seats\">\r\n            <thead>\r\n                <tr>\r\n                    <th>Redni broj</th>\r\n                    <th>Red</th>\r\n                    <th>Broj sjedišta</th>\r\n                </tr>\r\n            </thead>\r\n            <tbody>\r\n                {3}\r\n            </tbody>\r\n        </table>\r\n        <div class=\"footer\">\r\n            <p>Hvala na kupovini! Uživajte u filmu.</p>\r\n            <p>&copy; 2024 Vaš Cinetech. Sva prava pridržana.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

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

                string emailTemplateText = "<!DOCTYPE html>\r\n<html lang=\"hr\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f9f9f9;\r\n        }}\r\n\r\n        .container {{\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            border: 1px solid #dddddd;\r\n            padding: 20px;\r\n        }}\r\n\r\n        .header {{\r\n            text-align: center;\r\n            border-bottom: 1px solid #dddddd;\r\n            padding-bottom: 10px;\r\n        }}\r\n\r\n        .header img {{\r\n            max-width: 150px;\r\n            height: auto;\r\n        }}\r\n\r\n        .details {{\r\n            margin: 20px 0;\r\n        }}\r\n\r\n        .details h2 {{\r\n            margin: 0;\r\n            color: #333333;\r\n        }}\r\n\r\n        .details p {{\r\n            margin: 5px 0;\r\n            color: #666666;\r\n        }}\r\n\r\n        .seats {{\r\n            width: 100%;\r\n            border-collapse: collapse;\r\n            margin: 20px 0;\r\n        }}\r\n\r\n        .seats th, .seats td {{\r\n            border: 1px solid #dddddd;\r\n            text-align: left;\r\n            padding: 8px;\r\n        }}\r\n\r\n        .seats th {{\r\n            background-color: #f2f2f2;\r\n        }}\r\n\r\n        .footer {{\r\n            text-align: center;\r\n            margin-top: 20px;\r\n            color: #999999;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <img src=\"https://i.ibb.co/Trjr29Z/logoSivi.png\" alt=\"Cinetech Logo\">\r\n        </div>\r\n        <div class=\"details\">\r\n            <h2>Rezervacija</h2>\r\n            <p><strong>Ime filma:</strong> {0}</p>\r\n            <p><strong>Vrijeme projekcije:</strong> {1}</p>\r\n            <p><strong>Sala:</strong> {2}</p>\r\n        </div>\r\n        <table class=\"seats\">\r\n            <thead>\r\n                <tr>\r\n                    <th>Redni broj</th>\r\n                    <th>Red</th>\r\n                    <th>Broj sjedišta</th>\r\n                </tr>\r\n            </thead>\r\n            <tbody>\r\n                {3}\r\n            </tbody>\r\n        </table>\r\n        <div class=\"footer\">\r\n            <p>Hvala na rezervaciji! Uživajte u filmu.</p>\r\n            <p>&copy; 2024 Vaš Cinetech. Sva prava pridržana.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

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

                string emailTemplateText = "<!DOCTYPE html>\r\n<html lang=\"hr\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f9f9f9;\r\n        }}\r\n\r\n        .container {{\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            border: 1px solid #dddddd;\r\n            padding: 20px;\r\n        }}\r\n\r\n        .header {{\r\n            text-align: center;\r\n            border-bottom: 1px solid #dddddd;\r\n            padding-bottom: 10px;\r\n        }}\r\n\r\n        .header img {{\r\n            max-width: 150px;\r\n            height: auto;\r\n        }}\r\n\r\n        .details {{\r\n            margin: 20px 0;\r\n        }}\r\n\r\n        .details h2 {{\r\n            margin: 0;\r\n            color: #333333;\r\n        }}\r\n\r\n        .details p {{\r\n            margin: 5px 0;\r\n            color: #666666;\r\n        }}\r\n\r\n        .seats {{\r\n            width: 100%;\r\n            border-collapse: collapse;\r\n            margin: 20px 0;\r\n        }}\r\n\r\n        .seats th, .seats td {{\r\n            border: 1px solid #dddddd;\r\n            text-align: left;\r\n            padding: 8px;\r\n        }}\r\n\r\n        .seats th {{\r\n            background-color: #f2f2f2;\r\n        }}\r\n\r\n        .footer {{\r\n            text-align: center;\r\n            margin-top: 20px;\r\n            color: #999999;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <img src=\"https://i.ibb.co/Trjr29Z/logoSivi.png\" alt=\"Cinetech Logo\">\r\n        </div>\r\n        <div class=\"details\">\r\n            <center>\r\n                <strong><h2>Podsjetnik</h2></strong>\r\n                <p>Podsjećamo Vas da film {0} dolazi u kino dana {1}!</p>\r\n            </center>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>&copy; 2024 Vaš Cinetech. Sva prava pridržana.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";

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
