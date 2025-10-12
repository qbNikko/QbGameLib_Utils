using System;
using UnityEngine;

namespace QbGameLib_Utils.Component.Mb
{
    public class RotateComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _direction;
        [SerializeField] private float _speed;

        private void Update()
        {
            transform.Rotate(_direction * _speed);
        }
    }
}