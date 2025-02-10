using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Infrastructure.Services.Email
{
    public class EmailService(IOptions<EmailConfig> config) : IEmailService
    {
        private readonly EmailConfig _emailConfig = config.Value;

        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            var email = CreateEmail(emailRequest);

            using var smtpClient = new SmtpClient();

            try
            {
                await smtpClient.ConnectAsync(
                  _emailConfig.Server,
                  _emailConfig.Port,
                  SecureSocketOptions.StartTls);

                await smtpClient.AuthenticateAsync(
                  _emailConfig.Username,
                  _emailConfig.Password);

                await smtpClient.SendAsync(email);
            }
            finally
            {
                await smtpClient.DisconnectAsync(quit: true);
            }
        }

        private MimeMessage CreateEmail(EmailRequest emailRequest)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.FromEmail));

            emailMessage.To.AddRange(emailRequest.ToEmails.Select(MailboxAddress.Parse));

            emailMessage.Subject = emailRequest.Subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = emailRequest.Body
            };

            if (emailRequest.Attachments != null)
            {
                foreach (var (fileName, file, mediaType, subMediaType) in emailRequest.Attachments)
                {
                    bodyBuilder.Attachments.Add(fileName, file, new ContentType(mediaType, subMediaType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

    }
}
