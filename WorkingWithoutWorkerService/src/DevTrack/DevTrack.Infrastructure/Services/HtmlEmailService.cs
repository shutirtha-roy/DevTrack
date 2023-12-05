using DevTrack.Infrastructure.BusinessObjects;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DevTrack.Infrastructure.Services
{
    public class HtmlEmailService : IEmailService
    {
        private bool emailSent;
        private readonly Smtp _emailSettings;
        private readonly ILogger<HtmlEmailService> _logger;

        public HtmlEmailService(IOptions<Smtp> emailSettings, ILogger<HtmlEmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger=logger;

        }

        public async Task<bool> SendSingleEmail(string receiverName, string receiverEmail,
            string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.SenderName,
                _emailSettings.SenderEmail));

            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
                    client.Timeout = 30000;
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                    await client.SendAsync(message);
                    emailSent = true;
                }
                catch (SmtpCommandException ex)
                {
                    _logger.LogError(ex,ex.Message);
                    emailSent = false;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }   
            }
            return emailSent;
        }
    }
}