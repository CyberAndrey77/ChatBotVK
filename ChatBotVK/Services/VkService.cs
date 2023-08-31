using ChatBotVK.Factories;
using ChatBotVK.Models;
using ChatBotVK.Commands;
using Database.Models;
using Database.Repositories;

namespace ChatBotVK.Services
{
    public class VkService
    {
        private readonly IRepository<Category> _categoryRep;
        private readonly IRepository<Thing> _thingRep;
        private readonly IRepository<User> _userRep;
        private readonly IRepository<Equipment> _equipmentRep;
        private readonly MessageCreaterService _messageCreaterService;
        private readonly ModelFactory _modelFactory;
        private readonly SessionService _sessionService;
        private readonly Commands.Command _commands;
        private readonly SenderMessageService _senderMessage;
        private readonly ILogger<VkService> _logger;

        public VkService(IRepository<Category> categoryRep, IRepository<Thing> thingRep, IRepository<User> userRep,
            IRepository<Equipment> equipmentRep, MessageCreaterService messageCreaterService,
            ModelFactory modelFactory, SessionService sessionService, Command commands,
            SenderMessageService senderMessage, ILogger<VkService> logger)
        {
            _categoryRep = categoryRep;
            _thingRep = thingRep;
            _userRep = userRep;
            _equipmentRep = equipmentRep;
            _messageCreaterService = messageCreaterService;
            _modelFactory = modelFactory;
            _sessionService = sessionService;
            _commands = commands;
            _senderMessage = senderMessage;
            _logger = logger;
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
            MessageModel message;


            _commands.IsMainCommans = _commands.MainCommands.TryGetValue(messageNew, out var command);
            _commands.IsEquipmentCommands = !_commands.IsMainCommans &&
                _commands.EquipmentCommands.TryGetValue(messageNew, out command);
            _commands.IsBookingEquipments = !_commands.IsMainCommans &&
                !_commands.IsEquipmentCommands &&
                _commands.EquipmentCommands.TryGetValue(messageNew, out command);

            if (_commands.IsMainCommans || _commands.IsEquipmentCommands || _commands.IsBookingEquipments)
            {
                Model model;
                model = _sessionService.GetModel(userId);
                bool reset = false;
                if (command != EnumCommand.Back)
                {
                    if(model == null)
                    {
                        _commands.IsMainCommans = true; 
                        _commands.IsEquipmentCommands = false;
                        command = EnumCommand.Start;
                        var newModel = await _modelFactory.CreateModel(messageNew,
                            userId, model, _categoryRep, _thingRep, _userRep, _equipmentRep, command);
                        model = newModel;
                        reset = model.IsEndPoint;
                    }
                    else
                    {
                        var childModel = model.Childs.FirstOrDefault(x => x.NameModel == messageNew);
                        if (childModel == null)
                        {
                            childModel = await _modelFactory.CreateModel(messageNew, userId, model, 
                                _categoryRep, _thingRep, _userRep, _equipmentRep, command);
                            model.Childs.Add(childModel);
                        }
                        model = childModel;
                        reset = model.IsEndPoint;
                    }
                }
                else
                {
                    if (model?.Parent != null)
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
                await _senderMessage.SendMessage(message);
                if (model.BookModels != null)
                {
                    for (int i = 0; i < model.BookModels.Count; i++)
                    {
                        message = _messageCreaterService.CreateMessage(model.BookModels[i], userId);
                        await _senderMessage.SendMessage(message);
                    }
                }
                if (reset)
                {
                    while (model.Parent != null)
                    {
                        model = model.Parent;
                    }
                    _sessionService.SetModel(userId, model);
                    message = _messageCreaterService.CreateMessage(model, userId);
                    await _senderMessage.SendMessage(message);
                }
                _commands.IsMainCommans = _commands.IsEquipmentCommands = _commands.IsBookingEquipments = false;
            }
        }

        //private void ClearModel(object sender, ClearModelEvent e)
        //{
        //    if (e.Model != null)
        //    {
        //        if (e.Model.IsEndPoint)
        //        {
        //            return;
        //        }
        //        var message = _messageCreaterService
        //    }
        //}
    }
}
