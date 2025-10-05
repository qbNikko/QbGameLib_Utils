
namespace QbGameLib_Utils.FSM
{
    public class EmptyState : IState
    {
        private StateMachine _stateMachine;

        public string Name()
        {
            return "Empty";
        }

        StateMachine IState.StateMachine
        {
            get => _stateMachine;
            set => _stateMachine = value;
        }
    }
}