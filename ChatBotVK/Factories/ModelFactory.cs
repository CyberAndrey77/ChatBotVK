using ChatBotVK.Commands;
using ChatBotVK.Models;
using ChatBotVK.Models.Actions;
using ChatBotVK.Models.Buttons;
using ChatBotVK.Services;
using Database.Models;
using Database.Repositories;
using System.Net.Mail;
using System.Text;

namespace ChatBotVK.Factories
{
    public class ModelFactory
    {
        private readonly KeybordCreaterService _keybordCreaterService;
        private readonly Command _commands;
        private readonly PhotoLoaderService _photoLoader;

        //public object KeybordCreater { get; private set; }
        public ModelFactory(KeybordCreaterService keybordCreaterService, Command commands, 
            PhotoLoaderService photoLoader)
        {
            _keybordCreaterService = keybordCreaterService;
            _commands = commands;
            _photoLoader = photoLoader;
        }
        public async Task<Model> CreateModel(string message, long userId, Model? parent,
            IRepository<Category> categoryRep, IRepository<Thing> thingRep, 
            IRepository<User> userRep, IRepository<Equipment> equipmentRep, EnumCommand command)
        {
            var model = new Model
            {
                NameModel = message,
                Parent = parent,
                NameByttons = new List<string>(),
                Childs = new List<Model>()
            };

            switch (command)
            {
                case EnumCommand.Bot:
                case EnumCommand.Start:
                    model.Message = "Чем могу помочь?";
                    break;
                case EnumCommand.EquipmentList:
                {
                    model.Message = "Какой список вам нужен?";
                    var nameButtons = await categoryRep.GetAllAsync();
                    model.NameByttons.AddRange(AddButtons(command));
                    break;
                }
                case EnumCommand.FullEquipmentList:
                {
                    var attachments = new StringBuilder();
                    var things = await thingRep.GetAllAsync();
                    model.Message = await CreateListEquipment(things, thingRep, attachments);
                    model.IsEndPoint = true;
                    model.Attachments = attachments.ToString();
                    break;
                }
                case EnumCommand.Equipment:
                case EnumCommand.Clothes:
                case EnumCommand.Weapon:
                case EnumCommand.EquipmentCamp:
                {
                    if (_commands.IsBookingEquipments)
                    {
                        model.Message = "Какое снаряжение вы хотите попросить?";
                        model.BookModels = await CreateBookList(equipmentRep, categoryRep, userRep, command, message);
                        break;
                    }
                    else
                    {
                        var attachments = new StringBuilder();
                        var category = await categoryRep.GetAsync(x => x.Name == message);
                        var things = await thingRep.GetAllAsync(x => x.CategotyId == category.Id);
                        model.Message = await CreateListEquipment(things, thingRep, attachments);
                        model.IsEndPoint = true;
                        model.Attachments = attachments.ToString();
                        break;
                    }
                }
                case EnumCommand.About:
                {
                    var user = await userRep.GetAsync(x => x.Id == 178084478);
                    model.Message = "Приложение разработанные для автоматизации" +
                                    " получения информации по основным вопросам. " +
                                    "Пока доступно только информация о снаряжении.\n" +
                                    $"По всем вопросам писать ему {user.Link}";
                    model.IsEndPoint = true;
                    break;
                }
                case EnumCommand.BookEquipment:
                    {
                        model.Message = "Выберите категорию";
                        model.NameByttons.AddRange(AddButtons(command));
                        break;
                    }
                case EnumCommand.Book:
                {
                    model.Message = "Успех";
                    model.IsEndPoint= true;
                    break;
                }
            }

            if (model.Parent == null || model.IsEndPoint)
            {
                model.NameByttons.AddRange(AddButtons(command));
            }
            else
            {
                model.NameByttons.AddRange(AddButtons(EnumCommand.Back));
            }

            if (model.NameByttons.Count > 0)
            {
                model.Keyboard = _keybordCreaterService.CreateKeyboard(model.NameByttons.ToArray(),
                    Models.Enums.ButtonType.text);
            }
            return model;
        }

        private List<string> AddButtons(EnumCommand enumCommand)
        {
            var buttons = new List<string>();
            switch (enumCommand)
            {
                case EnumCommand.Start:
                {
                    buttons.Add(_commands.MainCommands.First(x => x.Value == EnumCommand.EquipmentList).Key);
                    buttons.Add(_commands.MainCommands.First(x => x.Value == EnumCommand.BookEquipment).Key);
                    buttons.Add(_commands.MainCommands.First(x => x.Value == EnumCommand.About).Key);
                    break;
                }
                case EnumCommand.Back:
                {
                    buttons.Add(_commands.MainCommands.First(x => x.Value == EnumCommand.Back).Key);
                    break;
                }
                case EnumCommand.EquipmentList:
                {
                    foreach (var item in _commands.EquipmentCommands)
                    {
                        buttons.Add(item.Key);
                    }
                    break;
                }
                case EnumCommand.BookEquipment:
                {
                    foreach (var item in _commands.EquipmentCommands)
                    {
                        if (item.Value == EnumCommand.FullEquipmentList)
                        {
                            continue;
                        }
                        buttons.Add(item.Key);
                    }
                    break;
                }
                case EnumCommand.Equipment:
                case EnumCommand.Clothes:
                case EnumCommand.Weapon:
                case EnumCommand.EquipmentCamp:
                {
                    buttons.Add(_commands.BookingEquipments.First(x => x.Value == enumCommand).Key);
                    break;
                }
            }
            return buttons;
        }

        //private TemplateModel CreateListEquipment(IEnumerable<Thing> things)
        //{
        //    var template = new TemplateModel
        //    {
        //        Elements = new List<Element>(things.Count())
        //    };
        //    foreach (var thing in things)
        //    {
        //        var element = new Element
        //        {
        //            Title = thing.Name
        //        };
        //        var must = thing.IsMust ? "Обязательно" : "Необязательно";
        //        var description = $"{thing.Description}\n{must}";
        //        element.Description = description;
        //        if (!string.IsNullOrEmpty(thing.Link))
        //        {
        //            element.Buttons ??= new List<Button>();
        //            element.Buttons.Add(new Button
        //            {
        //                Action = new LinkAction
        //                {
        //                    ButtonType = Models.Enums.ButtonType.open_link,
        //                    Link = thing.Link,
        //                    Label = thing.Cost.ToString()
        //                }
        //            }); 
        //        }
        //        else
        //        {
        //            element.Buttons ??= new List<Button>();
        //            element.Buttons.Add(new Button
        //            {
        //                Action = new BaseAction
        //                {
        //                    ButtonType = Models.Enums.ButtonType.text,
        //                    Label = "Нет ссылки"
        //                }
        //            }); ;
        //        }
        //        if (!string.IsNullOrEmpty(thing.ImgPath))
        //        {
        //            if (string.IsNullOrEmpty(thing.ImgUrl))
        //            {

        //            }
        //        }
        //        template.Elements.Add(element);
        //    }
        //    return template;
        //}

        private async Task<string> CreateListEquipment(IEnumerable<Thing> things, IRepository<Thing> repository,
            StringBuilder attachments)
        {
            var messageEquip = new StringBuilder();
            foreach (var thing in things)
            {
                var link = thing.Link;
                var cost = thing.Cost;
                var isMust = thing.IsMust;

                messageEquip.AppendLine(thing.Name);
                messageEquip.AppendLine(thing.Description);
                if (link != null)
                {
                    messageEquip.AppendLine($"Ссылка на вещь в магазине {link}");
                }
                if (cost != null)
                {
                    messageEquip.AppendLine($"Цена {cost} рублей");
                }
                if (thing.ImgPath != null)
                {
                    if (thing.OwnerPhotoId == null)
                    {
                        await UploadPhoto(thing, repository);
                    }
                    attachments.Append($"photo{thing.OwnerPhotoId}_{thing.PhotoId},");

                }
                messageEquip.AppendLine(isMust ? "Обязательно" : "По желанию");

                messageEquip.AppendLine();
            }

            if (messageEquip.Length == 0)
            {
                messageEquip.Append("Список пуст");
            }

            return messageEquip.ToString();
        }

        private async Task<List<Model>> CreateBookList(IRepository<Equipment> equipmentRep, IRepository<Category> categoryRep,
            IRepository<User> userRep, EnumCommand command, string message)
        {
            var category = await categoryRep.GetAsync(x => x.Name == message);
            var equipments = await equipmentRep.GetAllAsync(x => x.CategoryId == category.Id && !x.IsBooked);
            var modelsEquipment = new List<Model>(equipments.Count());
            foreach (var equipment in equipments)
            {
                var model = new Model();
                var user = await userRep.GetAsync(x => x.Id == equipment.OwnerId);
                var mes = new StringBuilder();
                mes.AppendLine($"{user.Name} может дать вам {equipment.Name}");
                var countMes = string.Empty;
                if (equipment.Count == 1)
                {
                    countMes = "штука";
                }
                else
                {
                    if (equipment.Count > 1 && equipment.Count < 5)
                    {
                        countMes = "штуки";
                    }
                    else
                    {
                        countMes = "штук";
                    }
                }
                mes.AppendLine($"Имеется {equipment.Count} {countMes}");
                model.NameByttons.AddRange(AddButtons(command));

                model.Keyboard = _keybordCreaterService.CreateKeyboard(model.NameByttons.ToArray(),
                   Models.Enums.ButtonType.text, true, true);
                modelsEquipment.Add(model);
            }
            return modelsEquipment;
        }

        private async Task<Thing> UploadPhoto(Thing thing, IRepository<Thing> repository)
        {
            var photoData = await _photoLoader.UploadPhotoToVk(thing.ImgPath);
            var photo = photoData.Responces.First();
            thing.OwnerPhotoId = photo.OwnerId;
            thing.PhotoId = photo.PhotoId;
            await repository.UpdateAsync(thing);
            return thing;
        }
    }
}
