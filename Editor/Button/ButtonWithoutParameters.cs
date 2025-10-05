#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using QbGameLib_Utils.Runtime.Attribute;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Button
{
    public class ButtonWithoutParameters : QbGameLib_Utils.Editor.Button.Button
    {
        public ButtonWithoutParameters(bool async,  MethodInfo method, ButtonAttribute buttonAttribute)
        {
            this._async = async;
            this._method = method;
            this._buttonAttribute = buttonAttribute;
        }
        
        protected override void DrawInternal(IEnumerable<object> targets)
        {
            if (!GUILayout.Button(GetMethodName()))
                return;
            InvokeMethod(targets);
        }

        protected override void InvokeMethod(IEnumerable<object> targets)
        {
            foreach (object obj in targets)
            {
                _method.Invoke(obj, null);
            }
        }
    }
}
#endif