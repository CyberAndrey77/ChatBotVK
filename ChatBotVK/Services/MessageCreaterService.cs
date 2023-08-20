using ChatBotVK.Models;

namespace ChatBotVK.Services
{
    public class MessageCreaterService
    {
        public MessageModel CreateMessage(string message, long userID)
        {
            var rnd = new Random();
            return new MessageModel
            {
                RandomId = rnd.Next(),
                UserId = userID,
                Message = message
            };
        }

        public MessageModel CreateMessage(Model model, long userID)
        {
            var newMessage = CreateMessage(model.Message, userID);
            if (model.Keyboard != null)
            {
                newMessage.Keyboard = model.Keyboard;
            }
            return newMessage;
        }
    }
}
