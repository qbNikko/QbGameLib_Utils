using System;

namespace QbGameLib_Utils.Component
{
    public class TimeAction
    {
        private readonly Action _action;
        private float _timeout;
        private float _currentTime;
        private bool _pause;
        private bool _loop;

        public TimeAction(Action action)
        {
            _action = action;
        }

        public TimeAction SetTimeout(float timeout)
        {
            _timeout = timeout;
            return this;
        }
        
        public TimeAction SetLoop(bool loop)
        {
            _loop = loop;
            return this;
        }
        
        public TimeAction Start()
        {
            _pause = false;
            return this;
        }

        public bool Pause
        {
            get => _pause;
            set => _pause = value;
        }

        public void Tick(float time)
        {
            if(_pause) return;
            _currentTime+=time;
            if (_currentTime >= _timeout)
            {
                _action.Invoke();
                _currentTime = 0;
                if (!_loop) _pause = true;
            }
        }
    }
}