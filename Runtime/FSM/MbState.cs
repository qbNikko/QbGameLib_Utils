using System.Collections.Generic;
using UnityEngine;

namespace QbGameLib_Utils.FSM
{
    public class MbState : MonoBehaviour, IState{
        private StateMachine _stateMachine;
        [SerializeField] string _name;
        public string Name()
        {
            return _name;
        }

        StateMachine IState.StateMachine
        {
            get => _stateMachine;
            set => _stateMachine = value;
        }

        public void Enter(Dictionary<string, object> parameters = null)
        {
            enabled = true;
        }

        public void Exit()
        {
            enabled = false;
        }

    }
}