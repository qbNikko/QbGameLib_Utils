using QbGameLib_Utils.Utils;
using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Extension
{
    public static class SerializedPropertyValueUtils
    {
        public static void SetPropertyValue(this SerializedProperty property, object value)
        {
            if (property == null) throw new System.ArgumentNullException("property");

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = ConvertUtils.ToInt(value);
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = ConvertUtils.ToBool(value);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = ConvertUtils.ToSingle(value);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = ConvertUtils.ToString(value);
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = ConvertUtils.ToColor(value);
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = value as UnityEngine.Object;
                    break;
                case SerializedPropertyType.LayerMask:
                    property.intValue = (value is LayerMask) ? ((LayerMask)value).value : ConvertUtils.ToInt(value);
                    break;
                case SerializedPropertyType.Enum:
                    property.SetEnumValue(value);
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = ConvertUtils.ToVector2(value);
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = ConvertUtils.ToVector3(value);
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = (Rect)value;
                    break;
                case SerializedPropertyType.ArraySize:
                    property.arraySize = ConvertUtils.ToInt(value);
                    break;
                case SerializedPropertyType.Character:
                    property.intValue = ConvertUtils.ToInt(value);
                    break;
                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = value as AnimationCurve;
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = (Bounds)value;
                    break;
            }
        }

        public static object GetPropertyValue(this SerializedProperty property)
        {
            if (property == null) throw new System.ArgumentNullException("property");

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.Boolean:
                    return property.boolValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Color:
                    return property.colorValue;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue;
                case SerializedPropertyType.LayerMask:
                    return (LayerMask)property.intValue;
                case SerializedPropertyType.Enum:
                    return property.intValue;
                case SerializedPropertyType.Vector2:
                    return property.vector2Value;
                case SerializedPropertyType.Vector3:
                    return property.vector3Value;
                case SerializedPropertyType.Vector4:
                    return property.vector4Value;
                case SerializedPropertyType.Rect:
                    return property.rectValue;
                case SerializedPropertyType.ArraySize:
                    return property.arraySize;
                case SerializedPropertyType.Character:
                    return (char)property.intValue;
                case SerializedPropertyType.AnimationCurve:
                    return property.animationCurveValue;
                case SerializedPropertyType.Bounds:
                    return property.boundsValue;
                // case SerializedPropertyType.Generic:
                // case SerializedPropertyType.ManagedReference:
                //     return GetTargetObjectOfProperty(property);
            }
            return null;
        }

        public static T GetPropertyValue<T>(this SerializedProperty property)
        {
            var obj = GetPropertyValue(property);
            if (obj is T) return (T)obj;
            var type = typeof(T);
            try
            {
                return (T)System.Convert.ChangeType(obj, type);
            }
            catch (System.Exception)
            {
                return default(T);
            }
        }
    }
}