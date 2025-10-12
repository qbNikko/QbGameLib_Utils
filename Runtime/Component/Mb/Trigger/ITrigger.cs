using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QbGameLib_Utils.Component.Mb.Trigger
{
    public enum TriggerState
    {
        enter,exit,stay,none
    }
    public interface ITrigger<T>
    {
        public TriggerState  LastState { get; set; }
        public List<T>  Object { get; set; }

        public void OnTriggerEnter(T other);
        public void OnTriggerExit(T other);
        public void OnTriggerStay(T other);
    }
}