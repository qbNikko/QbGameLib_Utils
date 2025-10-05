using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Extension
{
    public static class EditorUtils
    {
        public static bool CheckEditingMultiObjectNotSupportedHeight(SerializedProperty property, GUIContent label, out float height)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                height = EditorGUIUtility.singleLineHeight;
                return true;
            }

            height = 0f;
            return false;
        }
        
        public static bool CheckEditingMultiObjectNotSupported(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                EditorGUI.LabelField(position, label, TempContent("Multi-Object editing is not supported."));
                return true;
            }
            return false;
        }
        
        public static GUIContent TempContent(string text, string tooltip=null)
        {
            var c = new GUIContent();
            c.text = text;
            c.tooltip = tooltip;
            return c;
        }

    }
}