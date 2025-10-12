#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QbGameLib_Utils.Attribute;
using QbGameLib_Utils.Utils;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Button
{
    public class ButtonWithParameters : Button
    {
        private Parameter[] _parameter;
        public ButtonWithParameters(bool async, ParameterInfo[] parameters, MethodInfo method, ButtonAttribute buttonAttribute)
        {
            this._async = async;
            this._parameters = parameters;
            this._method = method;
            this._buttonAttribute = buttonAttribute;
            _parameter=parameters.Select(paramInfo => new Parameter(paramInfo)).ToArray();
        }


        protected override void DrawInternal(IEnumerable<object> targets)
        {
            (Rect foldoutRect, Rect buttonRect) = GetFoldoutAndButtonRects(GetMethodName());

            DrawInFoldout(foldoutRect, true, GetMethodName(), () =>
            {
                foreach (Parameter param in _parameter)
                {
                    param.Draw();
                }
            });

            if ( ! GUI.Button(buttonRect, "Invoke"))
                return;

            InvokeMethod(targets);
        }

        protected override void InvokeMethod(IEnumerable<object> targets)
        {
            var paramValues = _parameter.Select(param => param.Value).ToArray();

            foreach (object obj in targets) {
                _method.Invoke(obj, paramValues);
            }
        }
        
        
        public static (Rect foldoutRect, Rect buttonRect) GetFoldoutAndButtonRects(string header)
        {
            const float buttonWidth = 60f;

            Rect foldoutWithoutButton = GUILayoutUtility.GetRect(TempContent(header), EditorStyles.foldoutHeader);

            var foldoutRect = new Rect(
                foldoutWithoutButton.x,
                foldoutWithoutButton.y,
                foldoutWithoutButton.width - buttonWidth,
                foldoutWithoutButton.height);

            var buttonRect = new Rect(
                foldoutWithoutButton.xMax - buttonWidth,
                foldoutWithoutButton.y,
                buttonWidth,
                foldoutWithoutButton.height);

            return (foldoutRect, buttonRect);
        }
        
        private static readonly GUIContent _tempContent = new GUIContent();
        private static GUIContent TempContent(string text)
        {
            _tempContent.text = text;
            return _tempContent;
        }
        
        public static bool DrawInFoldout(Rect foldoutRect, bool expanded, string header, Action drawStuff)
        {
            expanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, expanded, header);
            if (expanded)
            {
                EditorGUI.indentLevel++;
                drawStuff();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            return expanded;
        }
    }
    
    
    internal class Parameter
    {
        private ParameterInfo paramInfo;
        private SerializedProperty field;
        private object value;
        private Func<object> editorDrawer;
        public Parameter(ParameterInfo paramInfo)
        {
            this.paramInfo = paramInfo;
            if(!ValueCache.valueCache.ContainsKey(paramInfo.GetHashCode())) ValueCache.valueCache.Add(paramInfo.GetHashCode(), null);
            value = ValueCache.valueCache[paramInfo.GetHashCode()];
            if (paramInfo.HasAttribute(typeof(ResourceListAttribute)))
            {
                ResourceListAttribute attribute = paramInfo.GetAttribute<ResourceListAttribute>();
                ResourceUtils.GetResourceWithoutExtensionList(attribute.Path, attribute.Pattern, out string[] files);
                Debug.Log(files[0]);
                editorDrawer = () =>
                {
                    int selected = Array.IndexOf(files, value);
                    selected =  EditorGUILayout.Popup(paramInfo.Name, selected==-1 ? 0 : selected, files);
                    return files[selected];
                };
            }
            else
            {
                if(paramInfo.ParameterType == typeof(string))
                    editorDrawer = () => EditorGUILayout.TextField(paramInfo.Name, value?.ToString());
                else if(paramInfo.ParameterType == typeof(int))
                    editorDrawer = () => EditorGUILayout.IntField(paramInfo.Name, value is int ? (int)value : 0);
                else if(paramInfo.ParameterType == typeof(bool))
                    editorDrawer = () => EditorGUILayout.Toggle(paramInfo.Name, value  is bool ? (bool)value : false);
                else if(paramInfo.ParameterType == typeof(float))
                    editorDrawer = () => EditorGUILayout.FloatField(paramInfo.Name, value  is float ? (float)value : 0f);
                else if(paramInfo.ParameterType == typeof(double))
                    editorDrawer = () => EditorGUILayout.DoubleField(paramInfo.Name, value   is double ? (double)value : 0d);
                else if(paramInfo.ParameterType == typeof(Color))
                    editorDrawer = () => EditorGUILayout.ColorField(paramInfo.Name, (Color)value);
                else if(paramInfo.ParameterType == typeof(Color32))
                    editorDrawer = () => EditorGUILayout.ColorField(paramInfo.Name, (Color32)value);
                else if(paramInfo.ParameterType == typeof(Color))
                    editorDrawer = () => EditorGUILayout.ColorField(paramInfo.Name, (Color)value);
                else if(paramInfo.ParameterType == typeof(Vector2))
                    editorDrawer = () => EditorGUILayout.Vector2Field(paramInfo.Name, (Vector2)value);
                else if(paramInfo.ParameterType == typeof(Vector2Int))
                    editorDrawer = () => EditorGUILayout.Vector2IntField(paramInfo.Name, (Vector2Int)value);
                else if(paramInfo.ParameterType == typeof(Vector3))
                    editorDrawer = () => EditorGUILayout.Vector3Field(paramInfo.Name, (Vector3)value);
                else if(paramInfo.ParameterType == typeof(Quaternion))
                    editorDrawer = () => Quaternion.Euler(EditorGUILayout.Vector3Field(paramInfo.Name, (Vector3)value));
            }
        }

        public object Value
        {
            get
            {
                return ValueCache.valueCache[paramInfo.GetHashCode()];
            }
        }

        public void Draw()
        {
            EditorGUI.BeginChangeCheck();
            value = editorDrawer.Invoke();
            if( EditorGUI.EndChangeCheck() )
            {
                ValueCache.valueCache[paramInfo.GetHashCode()]=value;
            }
        }

    }
}
#endif