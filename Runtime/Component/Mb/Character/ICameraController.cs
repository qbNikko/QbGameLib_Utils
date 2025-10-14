using System;
using QbGameLib_Utils.Extension;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QbGameLib_Utils.Component.Mb.Character
{
    public enum CameraType
    {
        FirstPerson,
        ThirdPerson
    }

    public interface ICameraController : IDisposable
    {
        public void Rotate(Vector3 rotation);
        public Vector2 Sensitivity { get; set; }
        public Vector2 LookXLimit { get; set; }
    }

    public abstract class PersonCameraController : ICameraController
    {
        protected Camera _camera;
        protected GameObject _parent;
        private Transform _prevParent;
        public Vector2 Sensitivity { get; set; }
        public Vector2 LookXLimit { get; set; }

        public PersonCameraController(GameObject parent, Camera camera)
        {
            _camera = camera;
            _parent = parent;
            _prevParent = _camera.transform.parent;
        }
        
        public virtual void Dispose()
        {
            _camera.transform.parent = _prevParent;
        }

        public abstract void Rotate(Vector3 rotation);

    }

    public class FirstPersonCameraController : PersonCameraController
    {
        private float _lookRotate;

        public FirstPersonCameraController(GameObject parent, Camera camera) : base(parent, camera)
        {
        }

        public override void Rotate(Vector3 rotation)
        {
            Transform cameraTransform = _camera.transform;
            _lookRotate += rotation.y * Sensitivity.y;
            _lookRotate = Mathf.Clamp(_lookRotate, LookXLimit.x, LookXLimit.y);
            cameraTransform.eulerAngles =
                new Vector3(-_lookRotate, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
        }
    }

    public class ThirdPersonCameraController : PersonCameraController
    {
        private float _lookRotate;
        private Vector3 _offset;
        private GameObject _cameraRotateAnchor;
        private bool _cameraRotateAnchorCreate;

        public ThirdPersonCameraController(GameObject parent, Camera camera, Vector3 offset) : base(parent, camera)
        {
            _camera = camera;
            SetOffset(offset);
        }

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
            if (_cameraRotateAnchor != null)
            {
                _camera.transform.position = _offset;
            }
            else if (!_camera.transform.CheckParent(_parent.transform))
            {
                _cameraRotateAnchor = new GameObject("CameraRotateAnchor");
                _cameraRotateAnchor.transform.parent = _parent.transform;
                // _cameraRotateAnchor.transform.Rotate(Vector3.up,180);
                _cameraRotateAnchor.transform.localPosition = Vector3.zero;
                _camera.transform.parent = _cameraRotateAnchor.transform;
                _camera.transform.localPosition = _offset;
                _camera.transform.localRotation = Quaternion.identity;
                _cameraRotateAnchorCreate = true;
            }
            else if(_parent.TryFindFirstByName("CameraRotateAnchor", out GameObject result))
            {
                _cameraRotateAnchor = result;
                _camera.transform.parent = _cameraRotateAnchor.transform;
                _camera.transform.position = _offset;
            }
        }

        public override void Rotate(Vector3 rotation)
        {
            Transform cameraTransform = _cameraRotateAnchor.transform;
            _lookRotate += rotation.y * Sensitivity.y;
            _lookRotate = Mathf.Clamp(_lookRotate, LookXLimit.x, LookXLimit.y);
            cameraTransform.eulerAngles =
                new Vector3(-_lookRotate, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
            _camera.transform.LookAt(_parent.transform);
        }

        public override void Dispose()
        {
            base.Dispose();
            if(_cameraRotateAnchorCreate) Object.Destroy(_cameraRotateAnchor);
        }
    }
}