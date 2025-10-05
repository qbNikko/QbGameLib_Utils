using System;

namespace QbGameLib_Utils.Reactive
{
    
    public static class ReactivePropertyExtensions
    {
        private static Action EmptyAction = () => { };
        private static Action<Exception> EmptyExceptionAction = (e) => { };
        
        public static IDisposable Subscribe<T>(this IReactiveProperty<T> source, Action<T> onNext)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, EmptyExceptionAction, EmptyAction));
        }
        
        public static IDisposable Subscribe<T>(this IReactiveProperty<T> source, Action<T> onNext, Action onCompleted)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, EmptyExceptionAction, onCompleted));
        }
        
        public static IDisposable Subscribe<T>(this IReactiveProperty<T> source, Action<T> onNext, Action<Exception> onErrorResume, Action onCompleted)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, onErrorResume, onCompleted));
        }
        
        
        public static IReactiveProperty<T> Subscribe<T>(this IReactiveProperty<T> source, Action<T> onNext, out IDisposable disposable)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, EmptyExceptionAction, EmptyAction), out disposable);
        }
        
        public static IReactiveProperty<T> Subscribe<T>(this IReactiveProperty<T> source, Action<T> onNext, Action onCompleted, out IDisposable disposable)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, EmptyExceptionAction, onCompleted), out disposable);
        }
        
        public static IReactiveProperty<T> Subscribe<T>(this IReactiveProperty<T> source, Action<T> onNext, Action<Exception> onErrorResume, Action onCompleted, out IDisposable disposable)
        {
            return source.Subscribe(new AnonymousObserver<T>(onNext, onErrorResume, onCompleted), out disposable);
        }
    }
}