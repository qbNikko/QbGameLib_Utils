using UnityEngine.InputSystem;

namespace QbGameLib_Utils.Interface
{
    public interface IStartInputHandler
    {
        public void OnStart(InputAction.CallbackContext context);
    }
    public interface ICancelInputHandler
    {
        public void OnCancel(InputAction.CallbackContext context);
    }
    public interface IPerformedInputHandler
    {
        public void OnPerformed(InputAction.CallbackContext context);
    }
    
    public interface IStartCancelInputHandler :  IStartInputHandler, ICancelInputHandler
    {
    }
    
    public interface IInputHandler :  IStartInputHandler, ICancelInputHandler, IPerformedInputHandler
    {
    }
}