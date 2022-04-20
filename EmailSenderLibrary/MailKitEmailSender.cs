using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Net.Sockets;

namespace EmailSenderLibrary
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly SmtpCredentials _smtpCredentials;

        public MailKitEmailSender(IOptions<SmtpCredentials> options)
        {
            _smtpCredentials = options.Value;
        }

        public async Task SendMessage(string toEmail, string? subject = null, string? body = null, CancellationToken token = default)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("", _smtpCredentials.UserName));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpCredentials.Host, _smtpCredentials.Port, false, token);
                    await client.AuthenticateAsync(_smtpCredentials.UserName, _smtpCredentials.Password, token);
                    await client.SendAsync(emailMessage, token);
                    await client.DisconnectAsync(true, token);
                }
            }
            catch (SocketException)
            {
                throw new NetworkException();
            }
            
        }
    }
}
