using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatBotVK.Models
{
    public class TemplateModel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "carousel";

        [JsonPropertyName("elements")]
        public List<Element> Elements { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class Element
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("buttons")]
        public List<Button> Buttons { get; set; }

        [JsonPropertyName("photo_id")]
        public string PhotoId { get; set; }

        [JsonPropertyName("action")]
        public PhotoAction Action { get; set; }
    }

    public class PhotoAction
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "open_photo";
    }
}
