using UnityEngine;

namespace QbGameLib_Utils.Component.Mb.Character
{
    public interface IMoveStrategy
    {
        public void Move(float deltaTime, Vector3 move);
        public bool Allow();
        public void Tick(float deltaTime);
    }
    
    public interface IMoveTimedStrategy : IMoveStrategy
    {
        public float AllowPercent();
    }

    public abstract class RigidbodyMoveStrategy : IMoveStrategy
    {
        protected Rigidbody _rb;
        protected Transform _transform;
        protected float _speed;
        protected float _maxSpeed;

        protected RigidbodyMoveStrategy(Rigidbody rb,  float speed, float maxSpeed)
        {
            _rb = rb;
            _speed =  speed;
            _maxSpeed = maxSpeed;
            _transform = rb.transform;
        }

        public abstract void Move(float deltaTime, Vector3 move);
        public virtual bool Allow()
        {
            return true;
        }

        public virtual void Tick(float deltaTime)
        {}
    }

    public class HorizontalMoveStrategy : RigidbodyMoveStrategy
    {
        
        public HorizontalMoveStrategy(Rigidbody rb, float speed, float maxSpeed) : base(rb, speed,  maxSpeed)
        {
        }

        public override void Move(float deltaTime, Vector3 move)
        {
            Vector3 moveVector = new Vector3(move.x,0,move.y)*_speed;
            moveVector = _transform.TransformDirection(moveVector);
            Vector3 velocityChange = moveVector-_rb.linearVelocity;
            velocityChange.y = 0;
            velocityChange = Vector3.ClampMagnitude(velocityChange, _maxSpeed);
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
    
    public class VerticalMoveStrategy : RigidbodyMoveStrategy
    {
        public VerticalMoveStrategy(Rigidbody rb, float speed, float maxSpeed) : base(rb, speed,  maxSpeed)
        {
        }

        public override void Move(float deltaTime, Vector3 move)
        {
            Vector3 moveVector = new Vector3(0,move.y,0)*_speed;
            moveVector = _transform.TransformDirection(moveVector);
            Vector3 velocityChange = moveVector-_rb.linearVelocity;
            velocityChange.x = 0;
            velocityChange.z = 0;
            velocityChange = Vector3.ClampMagnitude(velocityChange, _maxSpeed);
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    public enum UseStrategy
    {
        full, part, part_full
    }
    
    public class TimedMoveStrategy : IMoveTimedStrategy
    {
        private IMoveStrategy _moveStrategy;
        private float _timeout;
        private float _reloadTime;
        private float _currentTime;
        private bool _moved;
        public UseStrategy _strategy;
        public bool _fullReload;
        
        public TimedMoveStrategy(IMoveStrategy moveStrategy, float timeout, float reloadTime, UseStrategy strategy)
        {
            _moveStrategy = moveStrategy;
            _timeout = timeout;
            _reloadTime = reloadTime;
            _strategy = strategy;
        }

        public void Move(float deltaTime, Vector3 move)
        {
            _currentTime = Mathf.Min(_currentTime+deltaTime, _timeout);
            if(_strategy!=UseStrategy.part && _currentTime==_timeout) _fullReload =  true;
            _moveStrategy.Move(deltaTime, move);
            _moved = true;
        }

        public bool Allow()
        {
            return !_fullReload;
        }

        public void Tick(float deltaTime)
        {
            if(_moved) _moved=false;
            else if ((_strategy==UseStrategy.part && _currentTime >= 0) || _fullReload)
            {
                _currentTime = Mathf.Max(_currentTime-(deltaTime/_reloadTime), 0f);
                if(_currentTime==0) _fullReload = false;
            }
        }

        public float AllowPercent()
        {
            return (_timeout - _currentTime) / _timeout;
        }
    }
}