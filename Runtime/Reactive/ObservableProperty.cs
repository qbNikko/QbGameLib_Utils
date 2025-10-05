using System;
using System.Collections.Generic;

namespace QbGameLib_Utils.Reactive
{
    /**
     * Реализация паттерна слушателя для проперти
     */
    public abstract class ObservableProperty<T> : IReactiveObservable<T>
    {
        protected HashSet<IObserver<T>> _observers = new();
        private bool IsDisposed;

        public void AllUnsubscribed()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
            _observers.Clear();
        }
        
        public void Dispose()
        {
            foreach (IObserver<T> observer in _observers)
            {
                observer.OnCompleted();
            }
            _observers.Clear();
            IsDisposed = true;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (IsDisposed)
            {
                observer.OnCompleted();
                return Empty;
            }
            if(!_observers.Add(observer)) return Empty;
            return new Unsubscriber(_observers, observer);
        }
        
        public static readonly IDisposable Empty = new EmptyDisposed();
        internal class EmptyDisposed : IDisposable
        {
            public void Dispose(){}
        }
        public class Unsubscriber : IDisposable
        {
            private HashSet<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber(HashSet<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers =  observers;
                _observer = observer;
            }

            public void Dispose()
            {
                _observer.OnCompleted();
                _observers.Remove(_observer);
            }
        }
    }
}