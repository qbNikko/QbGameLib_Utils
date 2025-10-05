using System;

namespace QbGameLib_Utils.Reactive
{
    public class AnonymousObserver<T> : IObserver<T>
    {
        private Action<T> _onNext;
        private Action<Exception> _onError;
        private Action _onCompleted;

        public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnCompleted()
        {
            _onCompleted.Invoke();
            
        }

        public void OnError(Exception error)
        {
            _onError.Invoke(error);
        }

        public void OnNext(T value)
        {
            _onNext.Invoke(value);
        }
    }
}