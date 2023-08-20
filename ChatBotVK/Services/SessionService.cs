using ChatBotVK.Events;
using ChatBotVK.Models;
using System.Collections.Generic;
using System.Security.Principal;
using System.Timers;

namespace ChatBotVK.Services
{
    public class SessionService
    {
        //public delegate void ClearModelHandler(object sender, ClearModelEvent e);
        //public event ClearModelHandler ClearModel;
        private readonly Dictionary<long, Session> _listModels;
        private readonly System.Timers.Timer _timer;

        public SessionService()
        {
            _listModels = new Dictionary<long, Session>();
            _timer = new System.Timers.Timer(1800000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public void SetModel(long userId, Model currentModel)
        {
            if (!_listModels.TryGetValue(userId, out var session))
            {
                session = new Session { LastModel = currentModel, LastRequest = DateTime.Now, UserId = userId };
                _listModels.Add(userId, session);
            }
            else
            {
                session.LastModel = currentModel;
                session.LastRequest = DateTime.Now;
            }
        }

        public Model? GetModel(long userId)
        {
            if (_listModels.TryGetValue(userId, out var session))
            {
                return session.LastModel;
            }
            return null;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            var listRemove = new List<long>();
            foreach (var session in _listModels.Values)
            {
                if (DateTime.Now - session.LastRequest > TimeSpan.FromMinutes(30))
                {
                    listRemove.Add(session.UserId);
                }
            }

            for (int i = 0; i < listRemove.Count; i++)
            {
                _listModels.Remove(listRemove[i]);
            }
            listRemove.Clear();
        }
    }
}
