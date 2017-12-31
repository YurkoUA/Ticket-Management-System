using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketManagementSystem.Business.Telegram
{
    public class TelegramService : ITelegramService
    {
        private HttpClient _httpClient = new HttpClient();

        public async Task<bool> TrySendMessageAsync(string chatId, string text)
        {
            try
            {
                var message = new
                {
                    chat_id = chatId,
                    text = text
                };

                var resp = await _httpClient.PostAsync("sendMessage", 
                    new StringContent(await JsonConvert.SerializeObjectAsync(message), Encoding.UTF8, "application/json"));

                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public void Configure(string accessToken)
        {
            _httpClient.BaseAddress = new Uri($"https://api.telegram.org/bot{accessToken}/");
        }
    }
}
