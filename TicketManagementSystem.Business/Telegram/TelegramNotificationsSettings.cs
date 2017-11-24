namespace TicketManagementSystem.Business.Telegram
{
    public class TelegramNotificationsSettings
    {
        public bool IsNotificationsEnabled { get; set; }
        public string AccessToken { get; set; }
        public string ChatId { get; set; }
    }
}
