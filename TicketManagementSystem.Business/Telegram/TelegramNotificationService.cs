using System.Threading.Tasks;

namespace TicketManagementSystem.Business.Telegram
{
    public class TelegramNotificationService : ITelegramNotificationService
    {
        private ITelegramService _telegramService;
        private TelegramNotificationsSettings _settings;

        public TelegramNotificationService(ITelegramService telegramService, TelegramNotificationsSettings settings)
        {
            _telegramService = telegramService;
            _settings = settings;

            _telegramService.Configure(_settings.AccessToken);
        }

        public async Task<bool> TrySendMessage(string text)
        {
            if (_settings.IsNotificationsEnabled)
                return await _telegramService.TrySendMessageAsync(_settings.ChatId, text);

            return false;
        }
    }
}
