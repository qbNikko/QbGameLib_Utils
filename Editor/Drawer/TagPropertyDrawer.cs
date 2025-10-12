using QbGameLib_Utils.Attribute;
using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Drawer
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}