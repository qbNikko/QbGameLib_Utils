using System;
using System.Collections.Generic;
using UnityEngine;

namespace QbGameLib_Utils.Component.Pool
{
    public class MbPooledObject : MonoBehaviour, IPooledObject
    {
        private Action<IPooledObject> _realisePoolAction;
        protected List<Action<IPooledObject>> _releaseActionSubscribers;
        
        public virtual void Reset()
        {
            gameObject.SetActive(true);
        }

        public virtual void Release()
        {
            _releaseActionSubscribers?.ForEach(a=>a.Invoke(this));
            _releaseActionSubscribers?.Clear();
            gameObject.SetActive(false);
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
        
        public virtual void Initialize()
        {
        }

        void IPooledObject.SetRealisePoolAction(Action<IPooledObject> realisePoolAction)
        {
            _realisePoolAction = realisePoolAction;
        }

        public void ReleaseInPool()
        {
            _realisePoolAction?.Invoke(this);
        }

        public void SubscribeOnRelease(Action<IPooledObject> action)
        {
            if (_releaseActionSubscribers == null) _releaseActionSubscribers = new List<Action<IPooledObject>>();
            _releaseActionSubscribers.Add(action);
        }
    }
}