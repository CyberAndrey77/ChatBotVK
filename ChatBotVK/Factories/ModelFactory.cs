using ChatBotVK.Models;
using ChatBotVK.Services;
using Database.Models;
using Database.Repositories;
using System.Text;
using VkNet.Enums.StringEnums;

namespace ChatBotVK.Factories
{
    public class ModelFactory
    {
        private readonly KeybordCreaterService _keybordCreaterService;
        private readonly Commands.Commands _commands;

        //public object KeybordCreater { get; private set; }
        public ModelFactory(KeybordCreaterService keybordCreaterService, Commands.Commands commands)
        {
            _keybordCreaterService = keybordCreaterService;
            _commands = commands;
        }
        public async Task<Model> CreateModel(string request, long userId, Model? parent,
            IRepository<Category> categoryRep, IRepository<Thing> thingRep)
        {
            var model = new Model
            {
                NameModel = request,
                Parent = parent,
                NameByttons = new List<string>(),
                Childs = new List<Model>()
            };

            if (_commands.IsMainCommans)
            {
                if (request == _commands.MainCommands[0] || request == _commands.MainCommands[1])
                {
                    model.Message = "Чем могу помочь?";
                }
                if (request == _commands.MainCommands[2])
                {
                    model.Message = "Какой список вам нужен?";
                    var nameButtons = await categoryRep.GetAllAsync();
                    model.NameByttons.AddRange(_commands.EquipmentCommands);
                }
            }
            else if (_commands.IsEquipmentCommands)
            {
                if (request == _commands.EquipmentCommands[0])
                {
                    var things = await thingRep.GetAllAsync();
                    model.Message = CreateListEquipment(things);
                    model.IsEndPoint = true;
                }
                else
                {
                    var category = await categoryRep.GetAsync(x => x.Name == request);
                    var things = await thingRep.GetAllAsync(x => x.CategotyId == category.Id);
                    model.Message = CreateListEquipment(things);
                    model.IsEndPoint = true;
                }
            }

            //switch (request)
            //{
            //    //case "Список снаряжения":
            //    //    {
            //    //        model.Message = "Какой список вам нужен?";
            //    //        var nameButtons = await categoryRep.GetAllAsync();
            //    //        model.NameByttons.AddRange(_commands.EquipmentCommands);
            //    //        if (userId == 178084478)
            //    //        {
            //    //            model.NameByttons.Add("Добавить");
            //    //        }
            //    //        break;
            //    //    }
            //    case "Полный список":
            //        {
            //            var things = await thingRep.GetAllAsync();
            //            model.Message = CreateListEquipment(things);
            //            model.IsEndPoint = true;
            //            break;
            //        }
            //    case "Одежда":
            //    case "Оружие":
            //    case "Снаряжение":
            //    case "Бивачное снаряжение":
            //        {
            //            var category = await categoryRep.GetAsync(x => x.Name == request);
            //            var things = await thingRep.GetAllAsync(x => x.CategotyId == category.Id);
            //            model.Message = CreateListEquipment(things);
            //            model.IsEndPoint = true;
            //            break;
            //        }
            //    default:
            //        {
            //            model.Message = "Чем могу помочь?";
            //        }
            //        break;
            //}

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
                var buttons = _keybordCreaterService.CreateButtons(model.NameByttons.ToArray(), KeyboardButtonActionType.Text);
                model.Keyboard = _keybordCreaterService.CreateKeyboard(buttons);
            }
            return model;
        }

        //private static List<MessageModel> CreateListEquipment(IEnumerable<Thing> things)
        //{
        //    var messageModels = new List<MessageModel>();
        //    var messageEquip = new StringBuilder();
        //    var model = new MessageModel();
        //    foreach (var thing in things)
        //    {

        //        var link = thing.Link;
        //        var cost = thing.Cost;
        //        var isMust = thing.IsMust;

        //        messageEquip.AppendLine(thing.Name);
        //        messageEquip.AppendLine(thing.Description);
        //        if (link != null)
        //        {
        //            messageEquip.AppendLine($"Ссылка на вещь в магазине {link}");
        //        }
        //        if (cost != null)
        //        {
        //            messageEquip.AppendLine($"Цена {cost} рублей");
        //        }
        //        messageEquip.AppendLine(isMust ? "Обязательно" : "По желанию");
        //        if (thing.ImgPath != null)
        //        {
        //            model.ImgPath ??= new List<string>();
        //            model.ImgPath.Add(thing.ImgPath);
        //            if (thing.PhotoId != null)
        //            {
        //                model.PhotoId ??= new List<ulong>();
        //                model.OwnerPhotoId ??= new List<ulong>();

        //                model.PhotoId.Add((ulong)thing.PhotoId);
        //                model.OwnerPhotoId.Add((ulong)thing.OwnerPhotoId);
        //            }
        //        }
        //        messageEquip.AppendLine();
        //        model.Message = messageEquip.ToString();
        //        messageModels.Add(model);
        //        messageEquip.Clear();
        //        model = new MessageModel();
        //    }

        //    if (messageModels.Count == 0)
        //    {
        //        model = new MessageModel();
        //        messageEquip.Append("Список пуст");
        //        model.Message = messageEquip.ToString();
        //        messageModels.Add(model);
        //    }
        //    return messageModels;
        //}

        private string CreateListEquipment(IEnumerable<Thing> things)
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
                messageEquip.AppendLine(isMust ? "Обязательно" : "По желанию");

                messageEquip.AppendLine();
            }

            if (messageEquip.Length == 0)
            {
                messageEquip.Append("Список пуст");
            }
            return messageEquip.ToString();
        }
    }
}
