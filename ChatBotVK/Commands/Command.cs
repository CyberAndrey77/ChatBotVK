using Database.Models;
using Database.Repositories;

namespace ChatBotVK.Commands
{
    public class Command
    {
        public Dictionary<string, EnumCommand> MainCommands { get; set; }
        public Dictionary<string, EnumCommand> EquipmentCommands { get; set; }
        public bool IsMainCommans { get; set; }
        public bool IsEquipmentCommands { get; set; }
        public Command()
        {
            MainCommands = new Dictionary<string, EnumCommand>()
            {
                {"Бот", EnumCommand.Bot},
                {"Начать", EnumCommand.Start},
                {"Список снаряжения", EnumCommand.EquipmentList},
                {"Назад", EnumCommand.Back}
            };

            EquipmentCommands = new Dictionary<string, EnumCommand>()
            {
                {"Полный список", EnumCommand.FullEquipmentList},
                {"Снаряжение", EnumCommand.Equipment},
                {"Оружие", EnumCommand.Weapon},
                {"Бивачное снаряжение", EnumCommand.EquipmentCamp},
                {"Одежда", EnumCommand.Clothes}
            };
        }
    }

    public enum EnumCommand
    {
        Bot,
        Start,
        EquipmentList,
        Back,
        FullEquipmentList,
        Equipment,
        Clothes,
        Weapon,
        EquipmentCamp
    }
}
