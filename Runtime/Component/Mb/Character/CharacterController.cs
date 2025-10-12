using System;
using QbGameLib_Utils.Attribute;
using QbGameLib_Utils.Component.Mb.Trigger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QbGameLib_Utils.Component.Mb.Character
{
    [Serializable]
    public class MoveParameters
    {
        public float speed = 1f;
        public float maxForce = 5f;
        public float jumpCount = 2f;
        public float jumpForce = 5f;
        public bool stayWithoutDump = true;
    }

    [Serializable]
    public class CameraParameters
    {
        public Camera attachCamera;
        public CameraType cameraType = CameraType.FirstPerson;
        public Vector3 cameraOffset = new Vector3(0f, 1.5f, 3f);
        public Vector2 sensitivity = new Vector2(1, 1);
        public float zoomOnSpeed = 30f;
        public float zoomSmooth = 1f;
        public Vector2 lookXLimit = new Vector2(-30f, -30f);
    }

    [Serializable]
    public class SprintParameters
    {
        public float sprintSpeed = 2f;
        public float sprintMaxForce = 10f;
        public float sprintTimeout = 2f;
        public float sprintReloadTimeout = 3f;
    }

    [Serializable]
    public class DashParameters
    {
        public float dashSpeed = 5f;
        public float dashMaxForce = 30f;
        public float dashTimeout = 0.5f;
        public float dashReloadTimeout = 3f;
    }

    [Serializable]
    public class TriggerParameters
    {
        public ColliderTrigger groundTrigger;
        public ColliderTrigger wallHookTrigger;
        [Tag] public string ladderTag;
    }

    [Serializable]
    public class Events
    {
        public UnityEvent<float> speedEvent;
        public UnityEvent<Vector3> moveVectorEvent;
        public UnityEvent<bool> onGroundedEvent;
        public UnityEvent<int> onJumpEvent;
        public UnityEvent<bool> onLadderEvent;
        public UnityEvent<float> onSprintCountAllowEvent;
        public UnityEvent<bool> onSprintReloadEvent;
        public UnityEvent<bool> onDashReloadEvent;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CharacterController : MonoBehaviour
    {
        public MoveParameters moveParameters;
        public CameraParameters cameraParameters;
        public SprintParameters sprintParameters;
        public DashParameters dashParameters;
        public TriggerParameters triggerParameters;

        public Events events;
        private Rigidbody _rb;

        private Vector2 _move, _look;
        private float _lookRotate;
        private int _jump = 0;

        private bool _jumpAllow = true,
            _sprint = false,
            _stay = true,
            _dash = false;

        private float _zoomDefault;

        private IMoveStrategy _moveStrategy;
        private IMoveTimedStrategy _sprintStrategy;
        private IMoveTimedStrategy _dashStrategy;
        private IMoveStrategy _ladderStrategy;

        private bool _grounded;
        private bool _ladder;
        private ICameraController _cameraController;

        public bool JumpAllow
        {
            get => _jumpAllow;
            set => _jumpAllow = value;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (triggerParameters.groundTrigger != null)
                triggerParameters.groundTrigger.OnEvent.AddListener((b, s, c) =>
                {
                    if (s != TriggerState.stay) events.onGroundedEvent.Invoke(b);
                    _grounded = b;
                    if (s == TriggerState.enter) _jump = 0;
                });
            if (triggerParameters.wallHookTrigger != null)
                triggerParameters.wallHookTrigger.OnEvent.AddListener((b, s, c) =>
                {
                    
                    _ladder = b && c.CompareTag(triggerParameters.ladderTag);
                    if (s == TriggerState.enter) events.onLadderEvent.Invoke(_ladder);
                });
            InitCameraType();
            ReinitStrategy();
        }

        private void OnValidate()
        {
            if (Application.isPlaying == true)
            {
                ReinitStrategy();
            }
        }

        public void ReinitStrategy()
        {
            _moveStrategy = new HorizontalMoveStrategy(_rb, moveParameters.speed, moveParameters.maxForce);
            _ladderStrategy = new VerticalMoveStrategy(_rb, moveParameters.speed, moveParameters.maxForce);
            _sprintStrategy = new TimedMoveStrategy(
                new HorizontalMoveStrategy(_rb, sprintParameters.sprintSpeed * moveParameters.speed,
                    sprintParameters.sprintMaxForce),
                sprintParameters.sprintTimeout, sprintParameters.sprintReloadTimeout, UseStrategy.part_full
            );
            _dashStrategy = new TimedMoveStrategy(
                new HorizontalMoveStrategy(_rb, dashParameters.dashSpeed * moveParameters.speed,
                    dashParameters.dashMaxForce),
                dashParameters.dashTimeout, dashParameters.dashReloadTimeout, UseStrategy.full
            );
        }

        private void InitCameraType()
        {
            if (cameraParameters.cameraType == CameraType.FirstPerson)
                _cameraController = new FirstPersonCameraController(gameObject, cameraParameters.attachCamera);
            else if (cameraParameters.cameraType == CameraType.ThirdPerson)
                _cameraController = new ThirdPersonCameraController(gameObject, cameraParameters.attachCamera,
                    cameraParameters.cameraOffset);
            _cameraController.LookXLimit = cameraParameters.lookXLimit;
            _cameraController.Sensitivity = cameraParameters.sensitivity;
            _zoomDefault = cameraParameters.attachCamera.fieldOfView;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnMove(Vector3 movement)
        {
            _move = movement;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _sprint = !context.canceled;
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (!_dashStrategy.Allow()) return;
            _dash = true;
            events.onDashReloadEvent.Invoke(true);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _look = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            Jump();
        }

        private void FixedUpdate()
        {
            Move();
            CameraZoom();
            ReloadTimers();
        }

        private void ReloadTimers()
        {
            bool sprintAllow = _sprintStrategy.Allow();
            bool dashAllow = _dashStrategy.Allow();
            _moveStrategy.Tick(Time.fixedDeltaTime);
            _sprintStrategy.Tick(Time.fixedDeltaTime);
            _dashStrategy.Tick(Time.fixedDeltaTime);
            _ladderStrategy.Tick(Time.fixedDeltaTime);
            events.onSprintCountAllowEvent.Invoke(_sprintStrategy.AllowPercent());
            if (sprintAllow != _sprintStrategy.Allow()) events.onSprintReloadEvent.Invoke(false);
            if (dashAllow != _dashStrategy.Allow()) events.onDashReloadEvent.Invoke(false);
        }

        private void LateUpdate()
        {
            transform.Rotate(Vector3.up, _look.x * cameraParameters.sensitivity.x);
            _cameraController.Rotate(_look);
        }

        private void Move()
        {
            if (_move != Vector2.zero || !_stay)
            {
                if (_dash && _dashStrategy.Allow())
                {
                    _dashStrategy.Move(Time.fixedDeltaTime, _move);
                    if (!_dashStrategy.Allow()) _dash = false;
                    return;
                }
                else if (_sprint && _sprintStrategy.Allow())
                {
                    _sprintStrategy.Move(Time.fixedDeltaTime, _move);
                    events.onSprintCountAllowEvent.Invoke(_sprintStrategy.AllowPercent());
                    if (!_sprintStrategy.Allow()) events.onSprintReloadEvent.Invoke(true);
                }
                else
                {
                    if (_ladder && _ladderStrategy.Allow())
                        _ladderStrategy.Move(Time.fixedDeltaTime, _move);
                    if ((!_stay || moveParameters.stayWithoutDump) && _moveStrategy.Allow())
                        _moveStrategy.Move(Time.fixedDeltaTime, _move);
                }

                _stay = _move == Vector2.zero;
                events.speedEvent.Invoke(_rb.linearVelocity.magnitude);
                events.moveVectorEvent.Invoke(_rb.linearVelocity.normalized);
            }
        }

        private void CameraZoom()
        {
            if (cameraParameters.attachCamera != null && cameraParameters.zoomOnSpeed > 0)
            {
                float magnitude = Vector3.Magnitude(_rb.linearVelocity);
                float zoom = MathF.Min((magnitude / moveParameters.maxForce),1) * cameraParameters.zoomOnSpeed;
                cameraParameters.attachCamera.fieldOfView = Mathf.Lerp(cameraParameters.attachCamera.fieldOfView,
                    _zoomDefault + zoom, cameraParameters.zoomSmooth * Time.fixedDeltaTime);
            }
        }

        private void Jump()
        {
            if (_jumpAllow && (_grounded || moveParameters.jumpCount > _jump))
            {
                _jump++;
                Vector3 jumpVector = Vector3.up * moveParameters.jumpForce;
                jumpVector -= _rb.linearVelocity;
                _rb.AddForce(jumpVector, ForceMode.VelocityChange);
                events.onJumpEvent.Invoke(_jump);
            }
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.color = new Color(0f, 255, 0f, 0.2f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
#endif
        }
    }
}