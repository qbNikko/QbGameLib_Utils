using System.Collections.Generic;

namespace QbGameLib_Utils.FSM
{
    public abstract class AbstractCompositeState : IState
    {
        private StateMachine _stateMachine;
        List<IEnterSubState> _enterSubStates;
        List<IExitSubState> _exitSubStates;
        List<IUpdateSubState> _updateSubStates;
        List<IFixUpdateSubState> _fixUpdateSubStates;
        List<IHandleEventSubState> _handleEventSubStates;
        
        public abstract string Name();

        StateMachine IState.StateMachine
        {
            get => _stateMachine;
            set => _stateMachine = value;
        }

        public AbstractCompositeState(int countSubstate)
        {
            _enterSubStates = new List<IEnterSubState>(countSubstate);
            _exitSubStates = new List<IExitSubState>(countSubstate);
            _updateSubStates = new List<IUpdateSubState>(countSubstate);
            _fixUpdateSubStates = new List<IFixUpdateSubState>(countSubstate);
            _handleEventSubStates = new List<IHandleEventSubState>(countSubstate);
        }

        public AbstractCompositeState RegisterSubState(ISubState subState)
        {
            subState.State = this;
            if(subState is  IEnterSubState enterSubState) _enterSubStates.Add(enterSubState);
            if(subState is  IExitSubState exitSubState) _exitSubStates.Add(exitSubState);
            if(subState is  IUpdateSubState updateSubState) _updateSubStates.Add(updateSubState);
            if(subState is  IFixUpdateSubState fixUpdateSubState) _fixUpdateSubStates.Add(fixUpdateSubState);
            if(subState is  IHandleEventSubState handleEventSubState) _handleEventSubStates.Add(handleEventSubState);
            return this;
        }

        public void Enter(Dictionary<string, object> parameters = null)
        {
            foreach (IEnterSubState subState in _enterSubStates)
            {
                if(subState.Enabled) subState.Enter(parameters);
            }
            Enter_(parameters);
        }

        

        public void Exit()
        {
            foreach (IExitSubState subState in _exitSubStates)
            {
                if(subState.Enabled) subState.Exit();
            }
            Exit_();
        }

        public void Update()
        {
            foreach (IUpdateSubState subState in _updateSubStates)
            {
                if(subState.Enabled) subState.Update();
            }
            Update_();
        }

        public void FixedUpdate()
        {
            foreach (IFixUpdateSubState subState in _fixUpdateSubStates)
            {
                if(subState.Enabled) subState.FixedUpdate();
            }
            FixedUpdate_();
        }

        public void HandleEvent(string eventName)
        {
            foreach (IHandleEventSubState subState in _handleEventSubStates)
            {
                if(subState.Enabled) subState.HandleEvent(eventName);
            }
            HandleEvent_(eventName);
        }

        protected virtual void Enter_(Dictionary<string, object> parameters){}
        protected virtual void Exit_(){}
        protected virtual void Update_(){}
        protected virtual void FixedUpdate_(){}
        protected virtual void HandleEvent_(string eventName){}
    
    }
}