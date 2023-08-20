using ChatBotVK.Models.Dtos;
using ChatBotVK.Services;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using VkNet;
using VkNet.Model;
using VkNet.Utils;

namespace ChatBotVK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallBackController : ControllerBase
    {
        private readonly VkApi _vkApi;
        private readonly VkService _vkService;
        private readonly ILogger<CallBackController> _logger;

        public CallBackController(VkApi vkApi, VkService vkService, ILogger<CallBackController> logger)
        {
            _vkApi = vkApi;
            _vkService = vkService;
            _logger = logger;
        }

        //public CallBackController(VkApi vkApi)
        //{
        //    _vkApi = vkApi;
        //}

        [HttpPost]
        public async Task<IActionResult> Callback(UpdateDto update)
        {
            try
            {
                _logger.LogInformation($"Новый запрос {update.Type}");
                switch (update.Type)
                {
                    case "confirmation":
                        return Ok("dd92e280");

                    case "message_new":
                        {
                            var str = JsonSerializer.Serialize(update);
                            await _vkService.SendAnswer(update.Object.Message);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка в запросе {update.Type} : {ex.Message}");
            }

            return Ok("ok");

        }
    }
}
