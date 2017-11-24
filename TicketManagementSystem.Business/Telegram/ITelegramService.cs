using System.Threading.Tasks;

namespace TicketManagementSystem.Business.Telegram
{
    public interface ITelegramService
    {
        Task<bool> TrySendMessageAsync(string chatId, string text);
        void Configure(string accessToken);
    }
}
