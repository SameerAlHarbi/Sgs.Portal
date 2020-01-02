using Sgs.Portal.Shared.Services;
using System.Threading.Tasks;

namespace Sgs.Portal.Shared.Helpers
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(int employeeId, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
