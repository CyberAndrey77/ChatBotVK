using ChatBotVK.Events;
using ChatBotVK.Models;
using System.Security.Principal;
using System.Timers;

namespace ChatBotVK.Services
{
    public class SessionService
    {
        //public delegate void ClearModelHandler(object sender, ClearModelEvent e);
        //public event ClearModelHandler ClearModel;
        private readonly List<Session> _listModels;
        private readonly System.Timers.Timer _timer;

        public SessionService()
        {
            _listModels = new List<Session>();
            _timer = new System.Timers.Timer(1800000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public void SetModel(long userId, Model currentModel)
        {
            var session = _listModels.FirstOrDefault(x => x.UserId == userId);
            if (session == null)
            {
                session = new Session { LastModel = currentModel, LastRequest = DateTime.Now, UserId = userId };
                _listModels.Add(session);
            }
        }

        public Model? GetModel(long userId)
        {
            return _listModels.FirstOrDefault(x => x.UserId == userId)?.LastModel;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < _listModels.Count; i++)
            {
                if (DateTime.Now - _listModels[i].LastRequest > TimeSpan.FromMinutes(30))
                {
                    //ClearModel?.Invoke(this, new ClearModelEvent(_listModels[i].UserId, GetModel(_listModels[i].UserId)));
                    _listModels.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
