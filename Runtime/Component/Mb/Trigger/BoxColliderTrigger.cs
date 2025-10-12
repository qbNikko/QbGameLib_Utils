using System;
using UnityEngine;

namespace QbGameLib_Utils.Component.Mb.Trigger
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoxColliderTrigger : ColliderTrigger
    {
        protected override void Awake()
        {
            base.Awake();
            BoxCollider component = GetComponent<BoxCollider>();
            component.isTrigger = true;
            component.includeLayers = _layerMask;
        }
    }
}