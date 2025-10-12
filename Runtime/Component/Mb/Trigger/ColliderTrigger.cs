using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QbGameLib_Utils.Component.Mb.Trigger
{
    public class ColliderTrigger : MonoBehaviour, ITrigger<Collider>
    {
        [SerializeField] public LayerMask _layerMask;
        [SerializeField] public UnityEvent<bool, TriggerState, Collider> OnEvent;
        public TriggerState LastState { get; set; }
        public List<Collider> Object { get; set; }

        protected virtual void Awake()
        {
            Object = new List<Collider>();
        }

        public void OnTriggerEnter(Collider other)
        {
            OnEvent.Invoke(true,TriggerState.enter, other);
            LastState = TriggerState.enter;
            Object.Add(other);
        }
        public void OnTriggerExit(Collider other)
        {
            OnEvent.Invoke(false,TriggerState.exit,other);
            LastState = TriggerState.exit;
            Object.Remove(other);
        }
        public void OnTriggerStay(Collider other)
        {
            OnEvent.Invoke(true,TriggerState.stay,other);
            LastState = TriggerState.stay;
        }
    }
}