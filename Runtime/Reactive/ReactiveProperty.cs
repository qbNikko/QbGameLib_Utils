using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityGameLib.Reactive;

namespace QbGameLib_Utils.Reactive
{
    /**
     * Реализация паттерна слушателя для проперти
     */
    public class ReactiveProperty<T> : ObservableProperty<T>, IReactiveProperty<T>
    {
        private T _value;
        private IEqualityComparer<T>  _equatable;
        private Validate<T> _validator;
        private bool _autoUnsubscribeOnException;

        public ReactiveProperty(T value)
        {
            _value = value;
            _equatable = EqualityComparer<T>.Default;
            _validator = v => true;
            
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_equatable != null && _equatable.Equals(_value, value))return;
                if (!_validator.Invoke(value))return;
                _value = value;
                OnChangeValue();
            }
        }

        public IReactiveProperty<T> Equality([NotNull] IEqualityComparer<T> equality)
        {
            this._equatable = equality;
            return this;
        }

        public IReactiveProperty<T> Validate([NotNull] Validate<T> validator)
        {
            this._validator = validator;
            return this;
        }

        public IReactiveProperty<T> Subscribe(IObserver<T> observer, out IDisposable disposable)
        {
            disposable = Subscribe(observer);
            return this;
        }

        public IReactiveProperty<T> AutoUnsubscribeOnException()
        {
            _autoUnsubscribeOnException = true;
            return this;
        }

        List<IObserver<T>> exceptions = new List<IObserver<T>>();
        private void OnChangeValue()
        {
            
            foreach (IObserver<T> observer in _observers)
            {
                TryOperation.Try((() => observer.OnNext(Value)), (e)=>exceptions.Add(observer));
            }
            //Удаление слушателей с ошибками
            if ( exceptions.Count > 0)
            {
                if(_autoUnsubscribeOnException) exceptions.ForEach(o=>_observers.Remove(o));
                exceptions.Clear();
            }
        }
        
    }
}