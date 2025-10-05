namespace QbGameLib_Utils.FSM
{
    public delegate void FsmTriggerAction();
    public interface IFsmTrigger
    {
        public void AddListener(FsmTriggerAction listener);
        public void Invoke();
        public void RemoveListener(FsmTriggerAction fsmTriggerAction);
    }

    public class FsmTrigger :  IFsmTrigger
    {
        public event FsmTriggerAction OnTrigger;
        public void AddListener(FsmTriggerAction listener)
        {
            OnTrigger += listener;
        }
        public void RemoveListener(FsmTriggerAction listener)
        {
            OnTrigger -= listener;
        }

        public void Invoke()
        {
            OnTrigger?.Invoke();
        }

        
    }
}