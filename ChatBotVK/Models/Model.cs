namespace ChatBotVK.Models
{
    public class Model
    {
        public List<Model> Childs { get; set; }
        public string NameModel { get; set; }
        public Model Parent { get; set; }
        public List<string> NameByttons { get; set; }
        public KeyboardModel Keyboard { get; set; }
        public bool IsEndPoint { get; set; }
        //public List<MessageModel> Messages { get; set; }
        public string Message { get; set; }
    }
}
