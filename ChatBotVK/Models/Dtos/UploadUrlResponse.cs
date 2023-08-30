using System.Text.Json.Serialization;

namespace ChatBotVK.Models.Dtos
{
    public class UploadUrlResponse
    {
        [JsonPropertyName("response")]
        public Response Response { get; set; }
    }
    public class Response
    {
        [JsonPropertyName("album_id")]
        public long AlbumId { get; set; }

        [JsonPropertyName("upload_url")]
        public string? UploadUrl { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }
    }
}
