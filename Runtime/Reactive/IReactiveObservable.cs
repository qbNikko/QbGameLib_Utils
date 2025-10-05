using System;

namespace QbGameLib_Utils.Reactive
{
    public interface IReactiveObservable<T> : IObservable<T>, IDisposable
    {
    }
}