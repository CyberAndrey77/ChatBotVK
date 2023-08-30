using ChatBotVK.Models.Dtos;
using ChatBotVK.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ChatBotVK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallBackController : ControllerBase
    {
        private readonly VkService _vkService;
        private readonly ILogger<CallBackController> _logger;

        public CallBackController(VkService vkService, ILogger<CallBackController> logger)
        {
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
                        return Ok("414578b8");

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
