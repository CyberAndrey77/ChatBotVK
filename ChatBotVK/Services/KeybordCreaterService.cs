using Database.Models;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace ChatBotVK.Services
{
    public class KeybordCreaterService
    {
        public MessageKeyboardButtonAction CreateButton(string text, KeyboardButtonActionType type, string number)
        {
            return new MessageKeyboardButtonAction
            {
                Type = type,
                Label = text,
                Payload = "{\"button\": \"" + number + "\"}"
            };
        }

        public MessageKeyboardButtonAction[] CreateButtons(string[] nameButtons, KeyboardButtonActionType type)
        {
            var count = nameButtons.Length;
            var buttons = new MessageKeyboardButtonAction[count];
            for (var i = 0; i < count; i++)
            {
                buttons[i] = new MessageKeyboardButtonAction()
                {
                    Type = type,
                    Label = nameButtons[i],
                };
            }
            return buttons;
        }

        public MessageKeyboardButtonAction[] CreateButtons(List<string> nameButtons, KeyboardButtonActionType type)
        {
            return CreateButtons(nameButtons.ToArray(), type);
        }

        public MessageKeyboard CreateKeyboard(MessageKeyboardButtonAction[] buttons)
        {
            var keyboardBuilder = new KeyboardBuilder();
            foreach (var button in buttons)
            {
                keyboardBuilder.AddButton(button).AddLine();
            }
            return keyboardBuilder.Build();
        }

        public MessageKeyboard CreateKeyboard(List<MessageKeyboardButtonAction> buttons)
        {
            return CreateKeyboard(buttons.ToArray());
        }

        internal  MessageKeyboardButtonAction[] CreateButtons(Category[] nameButtons, KeyboardButtonActionType type)
        {
            var count = nameButtons.Length;
            var buttons = new MessageKeyboardButtonAction[count];
            for (var i = 0; i < count; i++)
            {
                buttons[i] = new MessageKeyboardButtonAction()
                {
                    Type = type,
                    Label = nameButtons[i].Name,
                };
            }
            return buttons;
        }
    }
}
