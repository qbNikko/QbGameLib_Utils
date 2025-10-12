using System;
using UnityEngine;

namespace QbGameLib_Utils.Attribute
{
    public enum ButtonMode
    {
        Enabled,
        EnabledInPlayMode,
        DisabledInPlayMode
    }
    
    [Flags]
    public enum ButtonSpacing
    {
        None = 0,
        Before = 1,
        After = 2
    }
    
    
    [AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonName;
        public ButtonMode Mode { get; set; } = ButtonMode.Enabled;
        public ButtonSpacing Spacing { get; set; } = ButtonSpacing.None;
    }
}