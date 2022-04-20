using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace EmailSenderLibrary
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpCredentials _smtpCredentials;

        public SmtpEmailSender(IOptions<SmtpCredentials> options)
        {
            _smtpCredentials = options.Value;
        }

        public async Task SendMessage(string toEmail, string? subject = null, string? body = null, CancellationToken token = default)
        {
            try
            {
                using (SmtpClient client = new(_smtpCredentials.Host))
                {
                    client.Port = _smtpCredentials.Port;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_smtpCredentials.UserName, _smtpCredentials.Password);
                    await client.SendMailAsync(_smtpCredentials.UserName, toEmail, subject, body, token);
                }
            }
            catch (SmtpException)
            {
                throw new NetworkException();
            }
            
        }
    }
}
