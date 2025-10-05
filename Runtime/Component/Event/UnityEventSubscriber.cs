using UnityEngine.Events;

namespace QbGameLib_Utils.Component.Event
{
    public class UnityEventSubscriber : IEventSubscriber
    {
        private UnityAction _action;
        private UnityEvent _event;
        public bool IsSubscribe { get; } = true;

        public static UnityEventSubscriber Create(UnityAction action, UnityEvent @event)
        {
            return new UnityEventSubscriber(action, @event);
        }

        public UnityEventSubscriber(UnityAction action, UnityEvent @event)
        {
            _action = action;
            _event = @event;
            @event.AddListener(action);
        }

        public void Subscribe()
        {
            if(!IsSubscribe) _event.AddListener(_action);
        }

        public void Unsubscribe()
        {
            if(IsSubscribe) _event.RemoveListener(_action);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
    
    public class UnityEventSubscriber<T> : IEventSubscriber
    {
        private UnityAction<T> _action;
        private UnityEvent<T> _event;
        public bool IsSubscribe { get; } = true;

        public static UnityEventSubscriber<T> Create(UnityAction<T> action, UnityEvent<T> @event)
        {
            return new UnityEventSubscriber<T>(action, @event);
        }

        public UnityEventSubscriber(UnityAction<T> action, UnityEvent<T> @event)
        {
            _action = action;
            _event = @event;
            @event.AddListener(action);
        }

        public void Subscribe()
        {
            if(!IsSubscribe) _event.AddListener(_action);
        }

        public void Unsubscribe()
        {
            if(IsSubscribe) _event.RemoveListener(_action);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
    
    public class UnityEventSubscriber<T0,T1> : IEventSubscriber
    {
        private UnityAction<T0,T1> _action;
        private UnityEvent<T0,T1> _event;
        public bool IsSubscribe { get; } = true;

        public static UnityEventSubscriber<T0,T1> Create(UnityAction<T0,T1> action, UnityEvent<T0,T1> @event)
        {
            return new UnityEventSubscriber<T0,T1>(action, @event);
        }

        public UnityEventSubscriber(UnityAction<T0,T1> action, UnityEvent<T0,T1> @event)
        {
            _action = action;
            _event = @event;
            @event.AddListener(action);
        }

        public void Subscribe()
        {
            if(!IsSubscribe) _event.AddListener(_action);
        }

        public void Unsubscribe()
        {
            if(IsSubscribe) _event.RemoveListener(_action);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
    
    public class UnityEventSubscriber<T0,T1,T2> : IEventSubscriber
    {
        private UnityAction<T0,T1,T2> _action;
        private UnityEvent<T0,T1,T2> _event;
        public bool IsSubscribe { get; } = true;

        public static UnityEventSubscriber<T0,T1,T2> Create(UnityAction<T0,T1,T2> action, UnityEvent<T0,T1,T2> @event)
        {
            return new UnityEventSubscriber<T0,T1,T2>(action, @event);
        }

        public UnityEventSubscriber(UnityAction<T0,T1,T2> action, UnityEvent<T0,T1,T2> @event)
        {
            _action = action;
            _event = @event;
            @event.AddListener(action);
        }

        public void Subscribe()
        {
            if(!IsSubscribe) _event.AddListener(_action);
        }

        public void Unsubscribe()
        {
            if(IsSubscribe) _event.RemoveListener(_action);
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}