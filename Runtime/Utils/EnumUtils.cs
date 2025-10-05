using System.Runtime.CompilerServices;

namespace QbGameLib_Utils.Utils
{
    public class EnumUtils
    {
        public static T ToEnum<T>(string val, T defaultValue) where T : struct, System.Enum
        {
            try
            {
                T result = (T)System.Enum.Parse(typeof(T), val, true);
                return result;
            }
            catch
            {
                return defaultValue;
            }
        }
        
        public static T ToEnum<T>(int val, T defaultValue) where T : struct, System.Enum
        {
            try
            {
                return (T)System.Enum.ToObject(typeof(T), val);
            }
            catch
            {
                return defaultValue;
            }
        }
        
        public static T ToEnum<T>(long val, T defaultValue) where T : struct, System.Enum
        {
            try
            {
                return (T)System.Enum.ToObject(typeof(T), val);
            }
            catch
            {
                return defaultValue;
            }
        }
        
        public static T ToEnum<T>(object val, T defaultValue) where T : struct, System.Enum
        {
            return ToEnum<T>(System.Convert.ToString(val), defaultValue);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToEnum<T>(string val) where T : struct, System.Enum
        {
            return ToEnum<T>(val, default(T));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToEnum<T>(int val) where T : struct, System.Enum
        {
            return ToEnum<T>(val, default(T));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToEnum<T>(object val) where T : struct, System.Enum
        {
            return ToEnum<T>(System.Convert.ToString(val), default(T));
        }

        public static System.Enum ToEnumOfType(System.Type enumType, object value)
        {
            if (value == null)
                return System.Enum.ToObject(enumType, 0) as System.Enum;
            else if (ConvertUtils.IsNumeric(value))
                return System.Enum.ToObject(enumType, ConvertUtils.ToInt(value)) as System.Enum;
            else
                return System.Enum.Parse(enumType, System.Convert.ToString(value), true) as System.Enum;

        }
    }
}