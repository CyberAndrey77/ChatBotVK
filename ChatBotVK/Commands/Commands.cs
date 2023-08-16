using Database.Models;
using Database.Repositories;

namespace ChatBotVK.Commands
{
    public class Commands
    {
        public List<string> MainCommands { get; set; }
        public List<string> EquipmentCommands { get; set; }
        public bool IsMainCommans { get; set; }
        public bool IsEquipmentCommands { get; set; }
        public Commands()
        {
            MainCommands = new List<string>
            {
                "Бот",
                "Начать",
                "Список снаряжения",
                "Назад"
            };

            EquipmentCommands = new List<string>
            {
                "Полный список"
            };
        }

        public void FillEquipmentCommands(IRepository<Category> categoryRep)
        {
            var categories = categoryRep.GetAllAsync().Result;
            foreach (var category in categories)
            {
                EquipmentCommands.Add(category.Name);
            }
        }
    }
}
