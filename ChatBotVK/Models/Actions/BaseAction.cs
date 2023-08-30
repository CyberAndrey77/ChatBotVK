using ChatBotVK.Models.Enums;
using System.Text.Json.Serialization;

namespace ChatBotVK.Models.Buttons
{
    public class BaseAction
    {
        private ButtonType _buttonType;
        private string _type;

        [JsonIgnore]
        public ButtonType ButtonType
        {
            get => _buttonType;
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
