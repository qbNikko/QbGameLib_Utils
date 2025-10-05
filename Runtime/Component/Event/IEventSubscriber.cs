using System;

namespace QbGameLib_Utils.Component.Event
{
    public interface IEventSubscriber : IDisposable
    {
        public void Subscribe();

        public void Unsubscribe();
    }
}