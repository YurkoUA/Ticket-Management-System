using System.Configuration;

namespace TicketManagementSystem.Business.AppSettings
{
    public class AppSettingsService : IAppSettingsService
    {
        public string BaseUrl => GetValue("BaseUrl");

        public bool IsAutomaticReportsEnabled => bool.Parse(GetValueOrDefault("AutomaticReportsEnabled", false));

        public bool IsAutomaticSummariesEnabled => bool.Parse(GetValueOrDefault("AutomaticSummariesEnabled", false));

        public bool RemoveOldLogins => bool.Parse(GetValueOrDefault("RemoveOldLogins", false));

        public int LatestLoginsToShow => int.Parse(GetValueOrDefault("LatestLoginsToShow", 10));

        public int TokenLifeTime => int.Parse(GetValueOrDefault("TokenLifeTimeMinutes", 1440));

        public bool IsTelegramNotificationsEnabled => bool.Parse(GetValueOrDefault("IsTelegramNotificationsEnabled", false));

        public string BotToken => GetValue("BotToken");

        public string TelegramChatId => GetValue("TelegramChatId");

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private string GetValueOrDefault<TDefault>(string key, TDefault defaultValue)
        {
            try
            {
                return GetValue(key);
            }
            catch
            {
                return defaultValue.ToString();
            }
        }
    }
}
