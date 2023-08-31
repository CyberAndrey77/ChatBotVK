using Database.Models;
using Database.Repositories;

namespace ChatBotVK.Commands
{
    public class Command
    {
        public Dictionary<string, EnumCommand> MainCommands { get; set; }
        public Dictionary<string, EnumCommand> EquipmentCommands { get; set; }
        public Dictionary<string, EnumCommand> SizeTableCommands { get; set; }

        public Dictionary<string, EnumCommand> BookingEquipments { get; set; }
        public bool IsMainCommans { get; set; }
        public bool IsEquipmentCommands { get; set; }
        public bool IsBookingEquipments { get; set; }
        public bool IsSizeTableCommans { get; set; }
        public Command()
        {
            MainCommands = new Dictionary<string, EnumCommand>()
            {
                {"Бот", EnumCommand.Bot},
                {"Начать", EnumCommand.Start},
                {"Список снаряжения", EnumCommand.EquipmentList},
                {"О боте", EnumCommand.About },
                {"Забронировать снаряжение", EnumCommand.BookEquipment },
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

            SizeTableCommands = new Dictionary<string, EnumCommand>()
            {
                {"Женский", EnumCommand.Female },
                {"Мужской", EnumCommand.Male },
                {"Я вертолет", EnumCommand.Helecopter }
            };

            BookingEquipments = new Dictionary<string, EnumCommand>()
            {
                {"Снаряжение", EnumCommand.Equipment},
                {"Оружие", EnumCommand.Weapon},
                {"Бивачное снаряжение", EnumCommand.EquipmentCamp},
                {"Одежда", EnumCommand.Clothes},
                {"Забронировать", EnumCommand.Book },
                {"Разрешить", EnumCommand.BookPositive},
                {"Не разрешать", EnumCommand.BookNegative}
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
        EquipmentCamp,
        About,
        SizeTablse,
        Male,
        Female,
        Helecopter,
        BookEquipment,
        Book,
        BookPositive,
        BookNegative
    }
}
