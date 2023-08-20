using ChatBotVK.Models;
using ChatBotVK.Models.Enums;
using Action = ChatBotVK.Models.Action;
using Button = ChatBotVK.Models.Button;

namespace ChatBotVK.Services
{
    public class KeybordCreaterService
    {

        public KeyboardModel CreateKeyboard(string[] nameButtons, ButtonType buttonType, bool inLine = false, bool isOneTime = false)
        {
            var keyboard = new KeyboardModel
            {
                Inline = inLine,
                OneTime = isOneTime
            };

            var count = nameButtons.Length;
            var listButtons = new List<Button[]>();
            for (int i = 0; i < count; i++)
            {
                var buttons = new Button[1];
                var button = new Button
                {
                    Action = new Action()
                    {
                        ButtonType = buttonType,
                        Label = nameButtons[i],
                    }
                };
                buttons[0] = button;
                listButtons.Add(buttons);
            }
            

            keyboard.Buttons = listButtons;
            return keyboard;
        }
    }
}
