using ChatBotVK.Models.Buttons;
using ChatBotVK.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatBotVK.Models
{
    public class KeyboardModel
    {
        [JsonPropertyName("inline")]
        public bool Inline { get; set; }

        [JsonPropertyName("buttons")]
        public List<Button[]> Buttons { get; set; }

        [JsonPropertyName("one_time")]
        public bool OneTime { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class Button
    {
        [JsonPropertyName("action")]
        public BaseAction Action { get; set; }
    }
}
