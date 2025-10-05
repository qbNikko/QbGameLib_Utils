using UnityEngine;

namespace QbGameLib_Utils.Component.Mb
{
    public class MoveOnTargetComponent : MonoBehaviour
    {
        [SerializeField] public Transform Target;
        [SerializeField] public Vector3 offset;
        [SerializeField] public bool ZFreez;
        Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if(Target==null) return;
            Vector3 position = Target.position + offset;
            if (ZFreez) position.z = _transform.position.z;
            _transform.position = position;
        }
    }
}