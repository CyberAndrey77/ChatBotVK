using ChatBotVK.Models;
using ChatBotVK.Models.Actions;
using ChatBotVK.Models.Buttons;
using ChatBotVK.Services;
using Database.Models;
using Database.Repositories;
using System.Text;

namespace ChatBotVK.Factories
{
    public class ModelFactory
    {
        private readonly KeybordCreaterService _keybordCreaterService;
        private readonly Commands.Commands _commands;
        private readonly PhotoLoaderService _photoLoader;

        //public object KeybordCreater { get; private set; }
        public ModelFactory(KeybordCreaterService keybordCreaterService, Commands.Commands commands, 
            PhotoLoaderService photoLoader)
        {
            _keybordCreaterService = keybordCreaterService;
            _commands = commands;
            _photoLoader = photoLoader;
        }
        public async Task<Model> CreateModel(string message, long userId, Model? parent,
            IRepository<Category> categoryRep, IRepository<Thing> thingRep)
        {
            var model = new Model
            {
                NameModel = message,
                Parent = parent,
                NameByttons = new List<string>(),
                Childs = new List<Model>()
            };

            if (_commands.IsMainCommans)
            {
                if (message == _commands.MainCommands[0] || message == _commands.MainCommands[1])
                {
                    model.Message = "Чем могу помочь?";
                }
                if (message == _commands.MainCommands[2])
                {
                    model.Message = "Какой список вам нужен?";
                    var nameButtons = await categoryRep.GetAllAsync();
                    model.NameByttons.AddRange(_commands.EquipmentCommands);
                }
            }
            else if (_commands.IsEquipmentCommands)
            {
                var attachments = new StringBuilder();
                if (message == _commands.EquipmentCommands[0])
                {
                    var things = await thingRep.GetAllAsync();
                    model.Message = await CreateListEquipment(things, thingRep, attachments);
                    model.IsEndPoint = true;
                }
                else
                {
                    var category = await categoryRep.GetAsync(x => x.Name == message);
                    var things = await thingRep.GetAllAsync(x => x.CategotyId == category.Id);
                    model.Message = await CreateListEquipment(things, thingRep, attachments);
                    model.IsEndPoint = true;
                }
                //var category = await categoryRep.GetAsync(x => x.Name == message);
                //var things = await thingRep.GetAllAsync(x => x.CategotyId == category.Id);
                //model.Message = $"{message}";
                //model.Template = CreateListEquipment(things);
                //model.IsEndPoint = true;
                model.Attachments = attachments.ToString();
            }

            if (model.Parent == null || model.IsEndPoint)
            {
                model.NameByttons.Add(_commands.MainCommands[2]);
            }
            else
            {
                model.NameByttons.Add(_commands.MainCommands[3]);
            }

            if (model.NameByttons.Count > 0)
            {
                model.Keyboard = _keybordCreaterService.CreateKeyboard(model.NameByttons.ToArray(),
                    Models.Enums.ButtonType.text);
            }
            return model;
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
