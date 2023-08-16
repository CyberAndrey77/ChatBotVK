using ChatBotVK.Factories;
using ChatBotVK.Models;
using Database.Models;
using Database.Repositories;
using VkNet;
using VkNet.Model;

namespace ChatBotVK.Services
{
    public class VkService
    {
        private readonly VkApi _vkApi;
        private readonly IRepository<Category> _categoryRep;
        private readonly IRepository<Thing> _thingRep;
        private readonly KeybordCreaterService _keybordCreaterService;
        private readonly MessageCreaterService _messageCreaterService;
        private readonly ModelFactory _modelFactory;
        private readonly SessionService _sessionService;

        private readonly ILogger<VkService> _logger;
        private readonly List<string> _listCommand;

        public VkService(VkApi vkApi, IRepository<Category> categoryRep, IRepository<Thing> thingRep,
            KeybordCreaterService keybordCreaterService, MessageCreaterService messageCreaterService,
            ModelFactory modelFactory, SessionService sessionService, ILogger<VkService> logger)
        {
            _vkApi = vkApi;
            _categoryRep = categoryRep;
            _thingRep = thingRep;
            _keybordCreaterService = keybordCreaterService;
            _messageCreaterService = messageCreaterService;
            _modelFactory = modelFactory;
            _sessionService = sessionService;
            _logger = logger;

            _listCommand = new List<string>()
            {
                "Бот",
                "Начать",
                "Список снаряжения",
                "Полный список",
                "Одежда",
                "Оружие",
                "Снаряжение",
                "Бивачное снаряжение",
                "Назад"
            };
        }

        public async Task SendAnswer(Models.Dtos.Message messageData)
        {
            if (messageData == null || messageData.FromId == null || string.IsNullOrEmpty(messageData.Text))
            {
                return;
            }
            var messageNew = messageData.Text;

            long userId = messageData.FromId;
            _logger.LogInformation($"User {userId} : {messageNew}");
            MessagesSendParams message;

            if (_listCommand.Contains(messageNew))
            {
                Model model;
                model = _sessionService.GetModel(userId);
                bool reset = false;
                if (messageNew != "Назад")
                {
                    if (model == null)
                    {
                        var newModel = await _modelFactory.CreateModel("Начало", userId, model, _categoryRep, _thingRep);
                        model = newModel;
                        reset = model.IsEndPoint;
                    }
                    else if (model.Childs?.Count != 0)
                    {
                        var childModel = model.Childs.FirstOrDefault(x => x.NameModel == messageNew);
                        if (childModel == null)
                        {
                            childModel = await _modelFactory.CreateModel(messageNew, userId, model, _categoryRep, _thingRep);
                            model.Childs.Add(childModel);
                        }
                        model = childModel;
                        reset = model.IsEndPoint;
                    }
                    else
                    {
                        var newModel = await _modelFactory.CreateModel(messageNew, userId, model, _categoryRep, _thingRep);
                        model.Childs.Add(newModel);
                        model = newModel;
                        reset = model.IsEndPoint;
                    }
                }
                else
                {
                    if (model != null)
                    {
                        model = model.Parent;
                    }
                    else
                    {
                        return;
                    }
                }

                _sessionService.SetModel(userId, model);
                message = _messageCreaterService.CreateMessage(model, userId);
                SendMessage(message);
                if (reset)
                {
                    while (true)
                    {
                        if (model.Parent != null)
                        {
                            model = model.Parent;
                        }
                        else
                        {
                            break;
                        }
                    }
                    _sessionService.SetModel(userId, model);
                    message = _messageCreaterService.CreateMessage(model, userId);
                    SendMessage(message);
                }
            }
        }

        private void SendMessage(MessagesSendParams message)
        {
            _vkApi.Messages.Send(message);
            //Console.WriteLine($"Awnswer: {message.Message}");
        }
    }
}
