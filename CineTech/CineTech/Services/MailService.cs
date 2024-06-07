using CineTech.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Security;

namespace CineTech.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
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
    }
}
