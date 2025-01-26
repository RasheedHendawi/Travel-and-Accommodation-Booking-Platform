
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
