using System.Text.Json.Serialization;

namespace ChatBotVK.Models.Dtos
{
    [JsonSourceGenerationOptions()]
    public class SavePhotoResponce
    {
        [JsonPropertyName("response")]
        public List<PhotoResponce> Responces { get; set; }
    }

    public class PhotoResponce
    {
        [JsonPropertyName("album_id")]
        public int AlbumId { get; set; }

        [JsonPropertyName("date")]
        public int Date { get; set; }

        [JsonPropertyName("id")]
        public long PhotoId { get; set; }

        [JsonPropertyName("owner_id")]
        public long OwnerId { get; set; }

        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; }
    }
}
