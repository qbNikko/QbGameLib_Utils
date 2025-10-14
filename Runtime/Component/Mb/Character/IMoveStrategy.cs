using System.Collections.Generic;
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

    public struct CalculateData
    {
        public Vector3 moveVector;
        public Vector3 snapSurvace;
        public float radius;
        public List<SlideData> slideData;
    }

    public struct SlideData
    {
        public Vector3 snapSurface;
        public RaycastHit hit;
        public Vector3 ower;

        public SlideData(Vector3 snapSurface, RaycastHit hit,Vector3 ower)
        {
            this.snapSurface = snapSurface;
            this.hit = hit;
            this.ower = ower;
        }
    }

    public struct SlideParameters
    {
        public LayerMask LayerMask;
        public Vector3 CollisionDetectOffset;
        public float AllowAngleLimit;
        public float FreezAngleLimit;
        public float StopAngleLimit;

        public SlideParameters(LayerMask layerMask, Vector3 collisionDetectOffset, float allowAngleLimit = 10f, float freezAngleLimit = 25f, float stopAngleLimit = 85f)
        {
            LayerMask = layerMask!= null ? layerMask : ~(1 << LayerMask.NameToLayer("Default"));
            CollisionDetectOffset = collisionDetectOffset;
            AllowAngleLimit = allowAngleLimit;
            FreezAngleLimit = freezAngleLimit;
            StopAngleLimit = stopAngleLimit;
        }
    }
    public class HorizontalMoveStrategy : RigidbodyMoveStrategy
    {
        private const float _skinWeight = 0.015f;
        private float slopeAngle = 55;
        private float _radius;
        private SlideParameters _slideParameters;
        public CalculateData MoveData;
        private bool allowSlide = false;
        
        public HorizontalMoveStrategy(Rigidbody rb, float speed, float maxSpeed) : base(rb, speed,  maxSpeed)
        {
            allowSlide = false;
            Collider component = rb.GetComponent<Collider>();
            _radius = component.bounds.extents.x;
            MoveData = new CalculateData();
            MoveData.radius = _radius;
            MoveData.slideData = new List<SlideData>();
        }

        public HorizontalMoveStrategy SetSlide(SlideParameters slideParameters)
        {
            _slideParameters = slideParameters;
            allowSlide = true;
            return this;
        }
        

        public override void Move(float deltaTime, Vector3 move)
        {
            
            Vector3 moveVector = new Vector3(move.x,0,move.y)*_speed;
            moveVector = _transform.TransformDirection(moveVector);
            Vector3 velocityChange = moveVector-_rb.linearVelocity;
            velocityChange.y = 0;
            velocityChange = Vector3.ClampMagnitude(velocityChange, _maxSpeed);
            if (move != Vector3.zero && allowSlide)
            {
                MoveData.moveVector = velocityChange;
                MoveData.slideData.Clear();
                velocityChange = CalculateSlide(velocityChange, _transform.position);
            }
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private Vector3 CalculateSlide(Vector3 velocityChange, Vector3 position)
        {
            Vector3 direction = velocityChange.normalized;
            if (Physics.SphereCast(_slideParameters.CollisionDetectOffset + position, _radius-_skinWeight, direction, out RaycastHit hit, velocityChange.magnitude+_skinWeight, _slideParameters.LayerMask))
            {
                Vector3 snapSurface = velocityChange.normalized*(hit.distance-_skinWeight);
                if(snapSurface.magnitude<=_skinWeight) snapSurface = Vector3.zero;
                Vector3 ower = velocityChange - snapSurface;
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle <= _slideParameters.AllowAngleLimit)
                {
                    ower = ProjectAndScale(ower, hit.normal);
                }
                else if (angle <= _slideParameters.FreezAngleLimit)
                {
                    ower = Vector3.zero;
                }
                else if (angle <= _slideParameters.StopAngleLimit)
                {
                    snapSurface.y = -0.2f;
                    ower = ProjectAndScale(ower, hit.normal);
                    ower.y = 0;
                }
                else
                {
                    ower = ProjectAndScale(ower, hit.normal);
                    ower.y = 0;
                }
                MoveData.slideData.Add(new SlideData(snapSurface,hit, ower));
                return snapSurface + ower;
            }

            return velocityChange;
        }

        private static Vector3 ProjectAndScale(Vector3 ower, Vector3 normal)
        {
            float magnitude = ower.magnitude;
            ower = Vector3.ProjectOnPlane(ower,normal).normalized;
            ower *= magnitude;
            return ower;
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