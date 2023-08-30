using System.Text.Json.Serialization;

namespace ChatBotVK.Models.Dtos
{
    public class UploadPhotoResponse
    {
        [JsonPropertyName("server")]
        public int Server { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}
