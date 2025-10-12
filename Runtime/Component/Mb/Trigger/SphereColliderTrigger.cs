using System;
using UnityEngine;

namespace QbGameLib_Utils.Component.Mb.Trigger
{
    [RequireComponent(typeof(SphereCollider))]
    public class SphereColliderTrigger : ColliderTrigger
    {
        protected override void Awake()
        {
            base.Awake();
            SphereCollider component = GetComponent<SphereCollider>();
            component.isTrigger = true;
            component.includeLayers = _layerMask;
        }
    }
}