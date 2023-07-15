using ChatBotVK.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotVK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallBackController : ControllerBase
    {
        [HttpPost]
        public IActionResult Callback(UpdateDto update)
        {
            // Проверяем, что находится в поле "type" 
            switch (update.Type)
            {
                // Если это уведомление для подтверждения адреса
                case "confirmation":
                    // Отправляем строку для подтверждения 
                    return Ok("355d55c7");
            }
            // Возвращаем "ok" серверу Callback API
            return Ok("ok");
        }
    }
}
