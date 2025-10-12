#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using QbGameLib_Utils.Attribute;
using QbGameLib_Utils.Extension;
using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Button
{
    public readonly struct VerticalIndent : IDisposable
    {
        private const float SpacingHeight = 10f;
        private readonly bool _bottom;

        public VerticalIndent(bool top, bool bottom)
        {
            if (top)
                GUILayout.Space(SpacingHeight);

            _bottom = bottom;
        }

        public void Dispose()
        {
            if (_bottom)
                GUILayout.Space(SpacingHeight);
        }
    }
    
    public abstract class Button
    {
        internal bool _async;
        internal ParameterInfo[] _parameters;
        internal MethodInfo _method;
        internal ButtonAttribute _buttonAttribute;
        internal bool _disabled;

        internal static Button Create(MethodInfo method, ButtonAttribute buttonAttribute)
        {
            var parameters = method.GetParameters();
            return parameters.Length > 0
                ? new ButtonWithParameters(false, parameters, method, buttonAttribute)
                : new ButtonWithoutParameters(false, method, buttonAttribute);
        }
        
        public void Draw(IEnumerable<object> targets)
        {
            using (new EditorGUI.DisabledScope(_disabled))
            {
                using (new VerticalIndent(
                           _buttonAttribute.Spacing.ContainsFlag(ButtonSpacing.Before),
                           _buttonAttribute.Spacing.ContainsFlag(ButtonSpacing.After)))
                {
                    DrawInternal(targets);
                }
            }
        }

        internal string GetMethodName()
        {
            return _buttonAttribute.ButtonName ?? _method.Name;
        }

        protected abstract void DrawInternal(IEnumerable<object> targets);
        protected abstract void InvokeMethod(IEnumerable<object> targets);
    }
}
#endif