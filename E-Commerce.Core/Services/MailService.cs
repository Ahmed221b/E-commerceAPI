using System.Net;
using System.Net.Mail;
using E_Commerce.Core.Configuration;
using E_Commerce.Core.Interfaces.Services;
using E_Commerce.Core.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace E_Commerce.Core.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;   
        }
        public async Task<ServiceResult<bool>> SendEmailAsync(List<string> emails, string subject, string message,bool isBodyHtml)
        {
            try
            {
                var password = Environment.GetEnvironmentVariable("MailServicePassword");
                // Create the email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_mailSettings.Email, _mailSettings.DisplayName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isBodyHtml,
                   
                };
                
                foreach(var email in emails)
                {
                    mailMessage.To.Add(email);
                }

                // Configure the SMTP client
                using (var smtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port))
                {
                    smtpClient.Credentials = new NetworkCredential(_mailSettings.Email, password);
                    smtpClient.EnableSsl = _mailSettings.EnableSsl;

                    // Send the email
                    await smtpClient.SendMailAsync(mailMessage);
                }

                // Return success
                return new ServiceResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>("Unexpected error occured: " + ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
