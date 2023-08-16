using ChatBotVK.Models;

namespace ChatBotVK.Events
{
    public class ClearModelEvent : EventArgs
    {
        public long UserId { get; set; }
        public Model Model { get; set; }

        public ClearModelEvent(long userId, Model model)
        {
            UserId = userId;
            Model = model;
        }
    }
}
