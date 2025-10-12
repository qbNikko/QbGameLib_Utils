using System;
using System.Collections.Generic;
using QbGameLib_Utils.Component.DI;

namespace QbGameLib_Utils.Component
{
    public class GlobalPauseManager
    {
        private List<string> _paused = new List<string>(); 
        private bool _globalPause = false;
        public event Action<bool, string> OnPause;
        
        public GlobalPauseManager()
        {
            DIService.RegisterSingleton(this);
        }

        public void Pause(string id = null)
        {
            if (id != null) _paused.Add(id);
            else _globalPause = true;
            OnPause?.Invoke(true, id);
        }

        public void UnPause(string id = null)
        {
            if (id != null) _paused.Remove(id);
            else _globalPause = false;
            OnPause?.Invoke(false, id);
        }

        public bool IsPaused(string id)
        {
            return _paused.Contains(id);
        }

        public bool IsPaused()
        {
            return _globalPause;
        }
    }
}