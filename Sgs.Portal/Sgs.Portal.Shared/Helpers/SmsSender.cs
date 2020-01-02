using Sgs.Portal.Shared.Services;
using System.Threading.Tasks;

namespace Sgs.Portal.Shared.Helpers
{
    public class SmsSender : ISmsSender
    {
        public Task SendSmsAsync(string phoneNumber, string message)
        {
            return Task.CompletedTask;
        }
    }
}
