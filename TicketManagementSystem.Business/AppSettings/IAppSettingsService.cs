namespace TicketManagementSystem.Business.AppSettings
{
    public interface IAppSettingsService
    {
        string BaseUrl { get; }

        bool IsAutomaticReportsEnabled { get; }
        bool IsAutomaticSummariesEnabled { get; }

        bool RemoveOldLogins { get; }
        int LatestLoginsToShow { get; }

        int TokenLifeTime { get; }

        bool IsTelegramNotificationsEnabled { get; }
        string BotToken { get; }
        string TelegramChatId { get; }

        int ItemsOnPage { get; }

        string GetValue(string key);
    }
}
