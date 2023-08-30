using System.Text;

namespace ChatBotVK.Models
{
    public class MessageModel
    {
        public long UserId { get; set; }
        public int RandomId { get; set; }
        public string Message { get; set; }
        public KeyboardModel? Keyboard { get; set; }
        public TemplateModel? Template { get; set; }
        public string? Attachments { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"user_id={UserId}&");
            stringBuilder.Append($"random_id={RandomId}&");
            stringBuilder.Append($"message={Message}&");
            if ( Keyboard != null )
            {
                stringBuilder.Append($"keyboard={Keyboard.ToString()}");
            }
            if ( Template != null )
            {
                stringBuilder.Append($"template={Template.ToString()}");
            }
            return stringBuilder.ToString();
        }
    }
}
