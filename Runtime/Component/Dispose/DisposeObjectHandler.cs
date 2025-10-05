using System;

namespace QbGameLib_Utils.Component.Dispose
{
    public class DisposeObjectHandler : IDisposable
    {
        public static DisposeObjectHandler Create(Action disposeAction)
        {
            return new DisposeObjectHandler(disposeAction);
        }
        
        private Action m_Dispose;

        private DisposeObjectHandler(Action disposeAction)
        {
            m_Dispose = disposeAction;
        }

        public void Dispose()
        {
            m_Dispose.Invoke();
        }
    }
}