using System;
using System.Linq;
using System.Reflection;
using QbGameLib_Utils.Utils;
using UnityEditor;
using UnityEngine;

namespace QbGameLib_Utils.Editor.Extension
{
    public static class SerializedPropertyUtils
    {
        
        public static System.Type GetTargetType(this SerializedObject obj)
        {
            if (obj == null) return null;
            if (obj.isEditingMultipleObjects)return obj.targetObjects[0].GetType();
            return obj.targetObject.GetType();
        }
        
        public static Type GetTargetType(this SerializedProperty prop)
        {
            if (prop == null) return null;
            FieldInfo field;
            Type fieldType;
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Generic:
                    {
                        field = GetFieldOfProperty(prop, out fieldType);
                        if (fieldType != null) return fieldType;
                        return typeof(object);
                    }
                case SerializedPropertyType.Integer:
                    return prop.type == "long" ? typeof(long) : typeof(int);
                case SerializedPropertyType.Boolean:
                    return typeof(bool);
                case SerializedPropertyType.Float:
                    return prop.type == "double" ? typeof(double) : typeof(float);
                case SerializedPropertyType.String:
                    return typeof(string);
                case SerializedPropertyType.Color:
                    {
                        field = GetFieldOfProperty(prop, out fieldType);
                        if (fieldType != null) return fieldType;
                        return typeof(Color);
                    }
                case SerializedPropertyType.ObjectReference:
                    {
                        field = GetFieldOfProperty(prop, out fieldType);
                        if (fieldType != null) return fieldType;
                        return typeof(UnityEngine.Object);
                    }
                case SerializedPropertyType.LayerMask:
                    return typeof(LayerMask);
                case SerializedPropertyType.Enum:
                    {
                        field = GetFieldOfProperty(prop, out fieldType);
                        if (fieldType != null) return fieldType;
                        return typeof(System.Enum);
                    }
                case SerializedPropertyType.Vector2:
                    return typeof(Vector2);
                case SerializedPropertyType.Vector3:
                    return typeof(Vector3);
                case SerializedPropertyType.Vector4:
                    return typeof(Vector4);
                case SerializedPropertyType.Rect:
                    return typeof(Rect);
                case SerializedPropertyType.ArraySize:
                    return typeof(int);
                case SerializedPropertyType.Character:
                    return typeof(char);
                case SerializedPropertyType.AnimationCurve:
                    return typeof(AnimationCurve);
                case SerializedPropertyType.Bounds:
                    return typeof(Bounds);
                case SerializedPropertyType.Gradient:
                    return typeof(Gradient);
                case SerializedPropertyType.Quaternion:
                    return typeof(Quaternion);
                case SerializedPropertyType.ExposedReference:
                    {
                        field = GetFieldOfProperty(prop, out fieldType);
                        if (fieldType != null) return fieldType;
                        return typeof(UnityEngine.Object);
                    }
                case SerializedPropertyType.FixedBufferSize:
                    return typeof(int);
                case SerializedPropertyType.Vector2Int:
                    return typeof(Vector2Int);
                case SerializedPropertyType.Vector3Int:
                    return typeof(Vector3Int);
                case SerializedPropertyType.RectInt:
                    return typeof(RectInt);
                case SerializedPropertyType.BoundsInt:
                    return typeof(BoundsInt);
                default:
                    {
                        field = GetFieldOfProperty(prop, out fieldType);
                        if (fieldType != null) return fieldType;
                        return typeof(object);
                    }
            }
        }
        
        public static Type GetPropertyValueType(this SerializedProperty property, bool ignoreSpecialWrappers = false)
        {
            if (property == null) throw new System.ArgumentNullException("property");
            var fieldType = property.GetTargetType();
            return fieldType ?? typeof(object);
        }
        
        public static Type GetManagedReferenceType(this SerializedProperty property)
        {
            var referenceFullTypename = property.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(referenceFullTypename)) return null;
            var arr = referenceFullTypename.Split(' ');
            if (arr.Length != 2) return null;
            return Type.GetType($"{arr[1]}, {arr[0]}");
        }

        public static Type GetManagedReferenceFieldType(this SerializedProperty property)
        {
            var referenceFieldTypename = property.managedReferenceFieldTypename;
            if (string.IsNullOrEmpty(referenceFieldTypename)) return null;

            var arr = referenceFieldTypename.Split(' ');
            if (arr.Length != 2) return null;
            return Type.GetType($"{arr[1]}, {arr[0]}");
        }
        
        public static SerializedProperty GetParentProperty(this SerializedProperty prop)
        {
            var elements = prop.propertyPath.Split('.');
            if (elements.Length <= 1) return null;

            if (elements[^1].StartsWith("data["))
                return prop.serializedObject.FindProperty(string.Join('.', elements.Take(elements.Length - 2)));
            return prop.serializedObject.FindProperty(string.Join('.', elements.Take(elements.Length - 1)));
        }
        
        public static FieldInfo GetFieldOfProperty(SerializedProperty prop, out Type fieldType)
        {
            fieldType = null;
            if (prop == null) return null;
            var roottype = GetTargetType(prop.serializedObject);
            if (roottype == null) return null;

            if (!prop.propertyPath.Contains('.'))
            {
                var result = ObjectUtils.GetMemberFromType(roottype, prop.propertyPath, true, mask:MemberTypes.Field) as FieldInfo;
                fieldType = result?.FieldType;
                return result;
            }

            SerializedProperty parent = prop.GetParentProperty();
            if (parent == null) return null;

            switch (parent.propertyType)
            {
                case SerializedPropertyType.ManagedReference:
                    {
                        fieldType = parent.GetManagedReferenceType() ?? parent.GetManagedReferenceFieldType();
                        var result = ObjectUtils.GetMemberFromType(fieldType, prop.name, true, mask:MemberTypes.Field) as FieldInfo;
                        fieldType = result?.FieldType;
                        return result;
                    }
                default:
                    if (parent.propertyType != SerializedPropertyType.Generic && parent.boxedValue != null)
                    {
                        var result = ObjectUtils.GetMemberFromType(parent.boxedValue.GetType(), prop.name, true, mask:MemberTypes.Field) as FieldInfo;
                        fieldType = result?.FieldType;
                        return result;
                    }
                    break;
            }

            var elements = prop.propertyPath.Split('.');
            var field = ObjectUtils.GetMemberFromType(roottype, elements[0], true, mask:MemberTypes.Field) as FieldInfo;
            fieldType = field.FieldType;
            parent = prop.serializedObject.FindProperty(elements[0]);
            for (int i = 1; i < elements.Length; i++)
            {
                var element = elements[i];
                if (element == "Array" && (i + 1) < elements.Length && elements[i + 1].StartsWith("data") && TypeUtils.IsListType(fieldType))
                {
                    var sindex = elements[i + 1];
                    element = "Array." + sindex;
                    int index = Convert.ToInt32(sindex.Substring(sindex.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    i++;
                    field = null;
                    fieldType = TypeUtils.GetTypeOfCollection(fieldType);
                    parent = parent.FindPropertyRelative(element);
                }
                else
                {
                    field = ObjectUtils.GetMemberFromType(fieldType, element, true, mask:MemberTypes.Field) as FieldInfo;
                    if (field == null)
                    {
                        fieldType = null;
                        return null;
                    }

                    parent = parent.FindPropertyRelative(elements[i]);
                    if (parent.propertyType == SerializedPropertyType.ManagedReference)
                    {
                        fieldType = parent.GetManagedReferenceType() ?? field.FieldType;
                    }
                    else
                    {
                        fieldType = field.FieldType;
                    }
                }
            }
            return field;
        }
        
        public static void SetEnumValue<T>(this SerializedProperty property, T value) where T : struct
        {
            if (property == null) throw new ArgumentNullException("property");
            if (property.propertyType != SerializedPropertyType.Enum) throw new ArgumentException("SerializedProperty is not an enum type.", "property");
            property.intValue = ConvertUtils.ToInt(value);
        }

        public static void SetEnumValue(this SerializedProperty property, Enum value)
        {
            if (property == null) throw new ArgumentNullException("property");
            if (property.propertyType != SerializedPropertyType.Enum) throw new ArgumentException("SerializedProperty is not an enum type.", "property");

            if (value == null)
            {
                property.enumValueIndex = 0;
                return;
            }

            //int i = prop.enumNames.IndexOf(System.Enum.GetName(value.GetType(), value));
            //if (i < 0) i = 0;
            //prop.enumValueIndex = i;
            property.intValue = ConvertUtils.ToInt(value);
        }
        
        public static void SetEnumValue(this SerializedProperty property, object value)
        {
            if (property == null) throw new ArgumentNullException("property");
            if (property.propertyType != SerializedPropertyType.Enum) throw new ArgumentException("SerializedProperty is not an enum type.", "property");
            if (value == null)
            {
                property.enumValueIndex = 0;
                return;
            }
            property.intValue = ConvertUtils.ToInt(value);
        }
        
        public static T GetEnumValue<T>(this SerializedProperty property) where T : struct, System.Enum
        {
            if (property == null) throw new ArgumentNullException("property");
            try
            {
                return EnumUtils.ToEnum<T>(property.intValue);
            }
            catch
            {
                return default(T);
            }
        }
        
        public static Enum GetEnumValue(this SerializedProperty property, Type type)
        {
            if (property == null) throw new ArgumentNullException("property");
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsEnum) throw new ArgumentException("Type must be an enumerated type.");
            try
            {
                return EnumUtils.ToEnumOfType(type, property.intValue);
            }
            catch
            {
                return Enum.GetValues(type).Cast<Enum>().First();
            }
        }
    }
}