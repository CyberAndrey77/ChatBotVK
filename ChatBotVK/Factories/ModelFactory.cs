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
            IRepository<Category> categoryRep, IRepository<Thing> thingRep, EnumCommand command)
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
                    model.NameByttons.AddRange(AddButtons(_commands));
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
                    var attachments = new StringBuilder();
                    var category = await categoryRep.GetAsync(x => x.Name == message);
                    var things = await thingRep.GetAllAsync(x => x.CategotyId == category.Id);
                    model.Message = await CreateListEquipment(things, thingRep, attachments);
                    model.IsEndPoint = true;
                    model.Attachments = attachments.ToString();
                    break;
                }
            }

            if (model.Parent == null || model.IsEndPoint)
            {
                model.NameByttons.Add(_commands.MainCommands.First(x => x.Value == EnumCommand.EquipmentList).Key);
            }
            else
            {
                model.NameByttons.Add(_commands.MainCommands.First(x => x.Value == EnumCommand.Back).Key);
            }

            if (model.NameByttons.Count > 0)
            {
                model.Keyboard = _keybordCreaterService.CreateKeyboard(model.NameByttons.ToArray(),
                    Models.Enums.ButtonType.text);
            }
            return model;
        }

        private List<string> AddButtons(Command command)
        {
            var buttons = new List<string>();
            if (command.IsMainCommans)
            {
                foreach (var item in command.EquipmentCommands)
                {
                    buttons.Add(item.Key);
                }
            }
            else if (command.IsEquipmentCommands)
            {
                
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
