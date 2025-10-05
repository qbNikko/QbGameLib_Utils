using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using CollectionPool = QbGameLib_Utils.Collection.CollectionPool;

namespace QbGameLib_Utils.Component.Dispose
{
    public class DisposeContainer : IDisposable
    {
        private List<IDisposable> disposables;
        private List<Action> disposableActions;
        private PooledObject<List<IDisposable>>? disposablePool;
        private PooledObject<List<Action>>? disposableActionsPool;
        
        public void AddDisposable(IDisposable disposable)
        {
            if (disposables == null)
                disposablePool = CollectionPool.GetList(out disposables);
            disposables.Add(disposable);
        }
        
        public void AddDisposableAction(Action action)
        {
            if (disposableActions == null)
                disposableActionsPool = CollectionPool.GetList(out disposableActions);
            disposableActions.Add(action);
        }

        public void Dispose()
        {
            disposables?.ForEach(d=>d.Dispose());
            disposableActions?.ForEach(a=>
            {
                try
                {
                    a.Invoke();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError(e.Message);
                }
            });
            (disposablePool as IDisposable)?.Dispose();
            (disposableActionsPool as IDisposable)?.Dispose();
        }
        void IDisposable.Dispose()
        {
            Dispose();
        }
    }
}