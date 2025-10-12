using System;
using QbGameLib_Utils.Component.DI;
using UnityEngine;

namespace QbGameLib_Utils.Component
{
    public abstract class PausedMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _paused;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public bool Paused
        {
            get => _paused;
            set => _paused = value;
        }

        protected virtual void Start()
        {
            GlobalPauseManager globalPauseManager = DIService.GetSingleton<GlobalPauseManager>();
            if(globalPauseManager!=null) globalPauseManager.OnPause += SwitchPause;
        }
        
        protected virtual void OnDestroy()
        {
            GlobalPauseManager globalPauseManager = DIService.GetSingleton<GlobalPauseManager>();
            if(globalPauseManager!=null) globalPauseManager.OnPause -= SwitchPause;
        }

        private void SwitchPause(bool pause, string id)
        {
            if (id == null) _paused = pause;
            else if(id.Equals(_id)) _paused = pause;
        }

        private void Update()
        {
            if(_paused) return;
            Update_();
        }
        protected abstract void Update_();
    }
}