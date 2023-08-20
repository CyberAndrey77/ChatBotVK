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
        public Action Action { get; set; }
    }

    public class Action
    {
        private ButtonType _buttonType;
        private string _type;

        [JsonIgnore]
        public ButtonType ButtonType { get => _buttonType; 
            set
            {
                _buttonType = value;
                _type = _buttonType.ToString().ToLower();
            }
        }

        [JsonPropertyName("type")]
        public string Type => _type ?? string.Empty;

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }
    }
}
