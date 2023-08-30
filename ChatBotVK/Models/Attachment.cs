namespace ChatBotVK.Models
{
    public class Attachment
    {
        public string Alias { get; set; } = "photo";
        public long PhotoId { get; set; }
        public long OwnerId { get; set; }

        public override string ToString()
        {
            return $"{Alias}{OwnerId}_{PhotoId}";
        }
    }
}
