using System.Threading.Tasks;

namespace Sgs.Portal.Shared.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailAsync(int employeeId, string subject, string message);
    }
}
