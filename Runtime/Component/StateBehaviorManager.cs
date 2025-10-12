using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QbGameLib_Utils.Component
{
    public class StateBehaviorManager : MonoBehaviour
    {
        private List<Action> _onEnable = new List<Action>();
        private List<Action> _onDisable = new List<Action>(); 
        private List<Action> _OnDestroy = new List<Action>();

        public void Subscribe(
            Action onEnable = null, Action onDisable = null, Action OnDestroy = null
            )
        {
            if(onEnable!=null) _onEnable.Add(onEnable);
            if(onDisable!=null) _onDisable.Add(onDisable);
            if(OnDestroy!=null) _OnDestroy.Add(OnDestroy);
        }

        private void OnEnable()
        {
            _onEnable.ForEach(a=>a.Invoke());
        }

        private void OnDisable()
        {
            _onDisable.ForEach(a=>a.Invoke());
        }

        private void OnDestroy()
        {
            _OnDestroy.ForEach(a=>a.Invoke());
        }
        
    }
}