using ChatBotVK.Models.Buttons;
using System.Text.Json.Serialization;

namespace ChatBotVK.Models.Actions
{
    public class LinkAction : BaseAction
    {
        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}
