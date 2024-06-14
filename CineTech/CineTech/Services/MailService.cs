using CineTech.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Security;
using System.Text.Encodings.Web;

namespace CineTech.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly InEmailSender _emailSender1;
        public MailService(IOptions<MailSettings> mailSettingsOptions, InEmailSender emailSender1)
        {
            _mailSettings = mailSettingsOptions.Value;
            _emailSender1 = emailSender1;
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => {
                            if (sslPolicyErrors == SslPolicyErrors.None)
                                return true;

                            Console.WriteLine($"SSL Policy Errors: {sslPolicyErrors}");
                            foreach (var status in chain.ChainStatus)
                            {
                                Console.WriteLine($"Certificate error: {status.StatusInformation}");
                            }

                            // Allow any certificate during development (not recommended for production)
                            return true;
                        };
                        try
                        {
                            mailClient.Connect(_mailSettings.Server, 587, SecureSocketOptions.StartTlsWhenAvailable);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to connect using StartTls: {ex.Message}");
                            Console.WriteLine("Retrying with SslOnConnect...");
                            mailClient.Connect(_mailSettings.Server, 465, SecureSocketOptions.SslOnConnect);
                        }
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                Console.WriteLine($"Failed to send email: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> SendMailAsync(MailData mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    // you can add the CCs and BCCs here.
                    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public bool SendHTMLMail(HTMLMailData htmlMailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(htmlMailData.EmailToName, htmlMailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = "Račun: " + htmlMailData.NazivFilma;

                    string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Racun.html";
                    string emailTemplateText = File.ReadAllText(filePath);

                    //DateTime.Today.Date.ToShortDateString()

                    string sjedista = "";
                    int b = 1;
                    foreach(ZauzetaSjedista z in htmlMailData.ZauzetaSjedista)
                    {
                        sjedista += "                <tr>\r\n" +
                                    "                   <td>" + b + "</td>\r\n" +
                                    "                   <td>Red " + z.red.ToString() + "</td>\r\n" +
                                    "                   <td>Sjedište " + z.redniBrojSjedista.ToString() + "</td>\r\n" +
                                    "                </tr>";
                        b++;
                    }

                    emailTemplateText = string.Format(emailTemplateText, htmlMailData.NazivFilma, htmlMailData.TerminProjekcije, htmlMailData.NazivSale,sjedista);

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    //await _emailSender1.SendEmailAsync(Input.Email, "Potvrda registracije", $"Da potvrdite registraciju pritisnite -> <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ovaj dio teksta</a>.");


                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => {
                            if (sslPolicyErrors == SslPolicyErrors.None)
                                return true;

                            Console.WriteLine($"SSL Policy Errors: {sslPolicyErrors}");
                            foreach (var status in chain.ChainStatus)
                            {
                                Console.WriteLine($"Certificate error: {status.StatusInformation}");
                            }

                            // Allow any certificate during development (not recommended for production)
                            return true;
                        };
                        try
                        {
                            mailClient.Connect(_mailSettings.Server, 587, SecureSocketOptions.StartTlsWhenAvailable);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to connect using StartTls: {ex.Message}");
                            Console.WriteLine("Retrying with SslOnConnect...");
                            mailClient.Connect(_mailSettings.Server, 465, SecureSocketOptions.SslOnConnect);
                        }
                        //mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public bool SendRezervacijaMail(HTMLMailData htmlMailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(htmlMailData.EmailToName, htmlMailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = "Rezervacija: " + htmlMailData.NazivFilma;

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

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        /*mailClient.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => {
                            if (sslPolicyErrors == SslPolicyErrors.None)
                                return true;

                            Console.WriteLine($"SSL Policy Errors: {sslPolicyErrors}");
                            foreach (var status in chain.ChainStatus)
                            {
                                Console.WriteLine($"Certificate error: {status.StatusInformation}");
                            }

                            // Allow any certificate during development (not recommended for production)
                            return true;
                        };
                        try
                        {
                            mailClient.Connect(_mailSettings.Server, 587, SecureSocketOptions.StartTlsWhenAvailable);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to connect using StartTls: {ex.Message}");
                            Console.WriteLine("Retrying with SslOnConnect...");
                            mailClient.Connect(_mailSettings.Server, 465, SecureSocketOptions.SslOnConnect);
                        }*/
                        mailClient.Connect(_mailSettings.Server, 25, SecureSocketOptions.None);
                        //mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public bool SendNotifikacijaMail(NotifikacijaMailData htmlMailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);

                    MailboxAddress emailTo = new MailboxAddress(htmlMailData.EmailToName, htmlMailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = "Premijera: " + htmlMailData.NazivFilma;

                    string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Notifikacija.html";
                    string emailTemplateText = File.ReadAllText(filePath);

                    emailTemplateText = string.Format(emailTemplateText, htmlMailData.NazivFilma, htmlMailData.DatumPredstavljanja);

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = emailTemplateText;
                    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => {
                            if (sslPolicyErrors == SslPolicyErrors.None)
                                return true;

                            Console.WriteLine($"SSL Policy Errors: {sslPolicyErrors}");
                            foreach (var status in chain.ChainStatus)
                            {
                                Console.WriteLine($"Certificate error: {status.StatusInformation}");
                            }

                            // Allow any certificate during development (not recommended for production)
                            return true;
                        };
                        try
                        {
                            mailClient.Connect(_mailSettings.Server, 587, SecureSocketOptions.StartTlsWhenAvailable);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to connect using StartTls: {ex.Message}");
                            Console.WriteLine("Retrying with SslOnConnect...");
                            mailClient.Connect(_mailSettings.Server, 465, SecureSocketOptions.SslOnConnect);
                        }
                        //mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }
    }
}
