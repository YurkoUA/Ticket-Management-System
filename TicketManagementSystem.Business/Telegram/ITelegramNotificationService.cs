using System.Threading.Tasks;

namespace TicketManagementSystem.Business.Telegram
{
    public interface ITelegramNotificationService
    {
        Task<bool> TrySendMessage(string text);
    }
}
