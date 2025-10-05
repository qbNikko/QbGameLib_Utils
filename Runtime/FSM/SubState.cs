namespace QbGameLib_Utils.FSM
{
    public abstract class SubState : ISubState
    {
        private IState _state;
        public bool Enabled { get; set; }

        IState ISubState.State
        {
            get => _state;
            set => _state = value;
        }
    }
}