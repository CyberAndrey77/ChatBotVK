using System.Text.Json.Nodes;

namespace ChatBotVK.Models.Dtos
{
    [Serializable]
    public class UpdateDto
    {
        /// <summary>
        /// Тип события
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Объект, инициировавший событие
        /// Структура объекта зависит от типа уведомления
        /// </summary>
        public JsonObject Object { get; set; }

        /// <summary>
        /// ID сообщества, в котором произошло событие
        /// </summary>
        public long GroupId { get; set; }
    }
}
