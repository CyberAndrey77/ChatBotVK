using ChatBotVK.Models;
using System.Timers;

namespace ChatBotVK.Services
{
    public class SessionService
    {
        private readonly Dictionary<long, Model> _listModels;
        private readonly System.Timers.Timer _timer;

        public SessionService()
        {
            _listModels = new Dictionary<long, Model>();
            _timer = new System.Timers.Timer(1800000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public void SetModel(long userId, Model currentModel)
        {
            if (!_listModels.TryAdd(userId, currentModel))
            {
                _listModels[userId] = currentModel;
            }
        }

        public Model GetModel(long userId)
        {
            if (_listModels.TryGetValue(userId, out var model))
            {
                return model;
            }
            return null;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            _listModels.Clear();
        }
    }
}
