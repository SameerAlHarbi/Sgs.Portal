using System.Threading.Tasks;

namespace Sgs.Portal.Shared.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string phoneNumber, string message);
    }
}
