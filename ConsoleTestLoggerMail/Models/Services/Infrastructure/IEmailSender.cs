using System.Threading.Tasks;

namespace TestLogger.Library.Models.Services.Infrastructure
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string sender, string email, string subject, string htmlMessage, string attachments = "");

        void SendEmail(string sender, string email, string subject, string htmlMessage, string attachments = "");
    }
}