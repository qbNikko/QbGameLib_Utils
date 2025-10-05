using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace QbGameLib_Utils.Reactive
{
    public interface IReactiveProperty<T> : IReactiveObservable<T>
    {
        T Value { get; set; }
        
        IReactiveProperty<T> Equality([NotNull] IEqualityComparer<T> equality);
        
        IReactiveProperty<T> Validate([NotNull] Validate<T> validator);
        
        IReactiveProperty<T> Subscribe([NotNull] IObserver<T> observer,out IDisposable disposable);
        
        IReactiveProperty<T> AutoUnsubscribeOnException();
    }
} 