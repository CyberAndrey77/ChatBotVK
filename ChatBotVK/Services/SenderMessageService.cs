using ChatBotVK.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ChatBotVK.Services
{
    public class SenderMessageService
    {
        private readonly string _idGroup;
        private readonly string _token;
        private readonly HttpClient _httpClient;
        private readonly string _vkApiPath = "https://api.vk.com/method/";
        private readonly string _messageSend = "messages.send?";

        public SenderMessageService(IConfiguration configuration, HttpClient httpClient)
        {
            _idGroup = configuration["GroupId"];
            _token = configuration["AccessToken"];
            _httpClient = httpClient;
        }

        public async Task SendMessage(MessageModel message)
        {
            //HttpContent content = new StringContent($"{message}&group_id={_idGroup}");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var uri = new Uri($"{_vkApiPath}{_messageSend}{message}&group_id={_idGroup}&v=5.131");
            var answer = await _httpClient.PostAsync(uri, null);
            string content = await answer.Content.ReadAsStringAsync();
        }
    }
}
