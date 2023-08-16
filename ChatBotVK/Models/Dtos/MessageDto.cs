using System.Text.Json.Serialization;

namespace ChatBotVK.Models.Dtos
{
    public class MessageDto
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }

        [JsonPropertyName("client_info")]
        public ClientInfo ClientInfo { get; set; }
    }
    public class ClientInfo
    {
        [JsonPropertyName("button_actions")]
        public List<string> ButtonActions { get; set; }

        [JsonPropertyName("keyboard")]
        public bool Keyboard { get; set; }

        [JsonPropertyName("inline_keyboard")]
        public bool InlineKeyboard { get; set; }

        [JsonPropertyName("carousel")]
        public bool Carousel { get; set; }

        [JsonPropertyName("lang_id")]
        public int LangId { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("date")]
        public int Date { get; set; }

        [JsonPropertyName("from_id")]
        public int FromId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("out")]
        public int Out { get; set; }

        [JsonPropertyName("attachments")]
        public List<object> Attachments { get; set; }

        [JsonPropertyName("conversation_message_id")]
        public int ConversationMessageId { get; set; }

        [JsonPropertyName("fwd_messages")]
        public List<object> FwdMessages { get; set; }

        [JsonPropertyName("important")]
        public bool Important { get; set; }

        [JsonPropertyName("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonPropertyName("peer_id")]
        public int PeerId { get; set; }

        [JsonPropertyName("random_id")]
        public int RandomId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
