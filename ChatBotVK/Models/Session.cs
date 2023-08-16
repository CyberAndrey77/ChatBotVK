namespace ChatBotVK.Models
{
    public class Session
    {
        public long UserId { get; set; }
        public Model? LastModel { get; set; } = null;
        public DateTime LastRequest { get; set; }
    }
}
