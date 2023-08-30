using ChatBotVK.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ChatBotVK.Services
{
    public class SenderMessageService
    {
        private readonly string _groupId;
        private readonly string _token;
        private readonly HttpClient _httpClient;
        private readonly string _vkApiPath = "https://api.vk.com/method/";
        private readonly string _messageSend = "messages.send?";

        public SenderMessageService(IConfiguration configuration, HttpClient httpClient)
        {
            _groupId = configuration["GroupId"];
            _token = configuration["AccessToken"];
            _httpClient = httpClient;
        }

        public async Task SendMessage(MessageModel message)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-encoded"));
            var uri = new Uri($"{_vkApiPath}{_messageSend}");

            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new FormUrlEncodedContent(CreateQuery(message))
            };

            var answer = await _httpClient.SendAsync(request);
            string content = await answer.Content.ReadAsStringAsync();
        }

        private Dictionary<string, string> CreateQuery(MessageModel message)
        {
            var query = new Dictionary<string, string>
            {
                { "user_id", $"{message.UserId}"},
                { "random_id", $"{message.RandomId}"},
                { "message", $"{message.Message}"}
            };
            if (message.Keyboard != null)
            {
                query.Add("keyboard", $"{message.Keyboard}");
            }
            if (message.Template != null)
            {
                query.Add("template", $"{message.Template}");
            }
            if (message.Attachments != null)
            {
                query.Add("attachment", $"{message.Attachments}");
            }
            query.Add("group_id", $"{_groupId}");
            query.Add("v", "5.131");
            return query;
        }
    }
}
