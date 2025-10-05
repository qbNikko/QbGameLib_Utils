using System;

namespace UnityGameLib.Reactive
{
    public static class TryOperation
    {
        public static void Try(Action action, Action<Exception> onError = null)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }
        }
        
        public static void TryGet<T>(Func<T> action, out T result, Action<Exception> onError = null, T defaultValue = default)
        {
            try
            {
                result= action.Invoke();
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                result = defaultValue;
            }
        }
    }
}