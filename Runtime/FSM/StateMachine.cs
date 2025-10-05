using System;
using System.Collections.Generic;
using QbGameLib_Utils.Collection;
using QbGameLib_Utils.Component.Dispose;

namespace QbGameLib_Utils.FSM
{
    public class StateMachine : IDisposable
    {
        private static IState emptyState =  new EmptyState();
        private readonly Dictionary<string, IState> _statesDictionary;
        private readonly Dictionary<Type, IState> _statesTypeDictionary;
        private readonly DisposeContainer  _disposableContainer;
        private readonly Dictionary<IFsmTrigger, FsmTriggerAction> _triggers;
        private IState _currentState;
        
        public StateMachine()
        {
            _disposableContainer = new DisposeContainer();
            _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out _statesDictionary));
            _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out _statesTypeDictionary));
            _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out _triggers));
            _currentState = emptyState;
        }

        public StateMachine RegisterState(IState state)
        {
            _statesDictionary.Add(state.Name(), state);
            _statesTypeDictionary.Add(state.GetType(), state);
            state.StateMachine = this;
            return this;
        }
        
        public StateMachine RegisterTrigger(IFsmTrigger fsmTrigger, Type state)
        {
            if(_triggers.ContainsKey(fsmTrigger)) throw new AggregateException("Trigger already exists");
            FsmTriggerAction action = () => SwitchState(state);
            fsmTrigger.AddListener(action);
            _triggers.Add(fsmTrigger, action);
            return this;
        }
        public StateMachine UnregisterTrigger(IFsmTrigger fsmTrigger)
        {
            if (_triggers.TryGetValue(fsmTrigger, out FsmTriggerAction action))
            {
                fsmTrigger.RemoveListener(action);
                _triggers.Remove(fsmTrigger);
            }
            return this;
        }
        
        public void SwitchState(Type state, Dictionary<string,object> parameters = null)
        {
            if (state!=null && _statesTypeDictionary.TryGetValue(state, out var newState))
            {
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter(parameters);
            }
        }
        
        public void SwitchState(string state, Dictionary<string,object> parameters = null)
        {
            if (state!=null && _statesDictionary.TryGetValue(state, out var newState))
            {
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter(parameters);
            }
        }

        public void HandleEvent(string eventName)
        {
            _currentState.HandleEvent(eventName);
        }

        public void Update()
        {
            _currentState.Update();
        }

        public void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        public void Dispose()
        {
            foreach (var fsmTriggerAction in _triggers)
            {
                fsmTriggerAction.Key.RemoveListener(fsmTriggerAction.Value);
            }
            _triggers.Clear();
            _statesTypeDictionary.Clear();
            _statesDictionary.Clear();
        }
    }
}