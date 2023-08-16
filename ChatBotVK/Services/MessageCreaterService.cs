using ChatBotVK.Models;
using VkNet.Model;

namespace ChatBotVK.Services
{
    public class MessageCreaterService
    {
        public MessagesSendParams CreateMessage(string message, long? userID)
        {
            var rnd = new Random();
            return new MessagesSendParams
            {
                RandomId = rnd.Next(),
                UserId = userID,
                Message = message
            };
        }

        public MessagesSendParams CreateMessage(Model model, long? userID)
        {
            var newMessage = CreateMessage(model.Message, userID);
            if (model.Keyboard != null)
            {
                newMessage.Keyboard = model.Keyboard;
            }
            return newMessage;
        }

        //public List<MessagesSendParams> CreateMessages(Model model, long? userID)
        //{
        //    var messages = new List<MessagesSendParams>();
        //    for (int i = 0; i < model.Messages.Count; i++)
        //    {
        //        var newMessage = CreateMessage(model.Messages[i].Message, userID);
        //        if (model.Keyboard != null)
        //        {
        //            newMessage.Keyboard = model.Keyboard;
        //        }
        //        messages.Add(newMessage);
        //    }
        //    return messages;
        //}
    }
}
