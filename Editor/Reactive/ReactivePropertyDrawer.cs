using QbGameLib_Utils.Reactive;
using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Reactive
{
    [CustomPropertyDrawer(typeof(ReactiveProperty<int>))]
    public class ReactivePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty findPropertyRelative = property.FindPropertyRelative("_value");
            EditorGUI.PropertyField(position, findPropertyRelative, GUIContent.none);
        }
    }
}