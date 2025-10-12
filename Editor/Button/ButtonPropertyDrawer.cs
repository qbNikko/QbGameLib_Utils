#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using QbGameLib_Utils.Attribute;
using UnityEditor;
using Object = UnityEngine.Object;

namespace QbGameLib_Utils.Editor.Button
{
    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    public class ButtonPropertyDrawer : UnityEditor.Editor
    {
        public readonly List<QbGameLib_Utils.Editor.Button.Button> Buttons = new List<QbGameLib_Utils.Editor.Button.Button>();
        private void OnEnable()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            var methods = target.GetType().GetMethods(flags);

            foreach (MethodInfo method in methods)
            {
                var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();

                if (buttonAttribute == null)
                    continue;

                Buttons.Add(QbGameLib_Utils.Editor.Button.Button.Create(method, buttonAttribute));
            }
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            foreach (QbGameLib_Utils.Editor.Button.Button button in Buttons)
            {
                button.Draw(targets);
            }
        }
    }
}
#endif