using UnityEngine;

namespace QbGameLib_Utils.Utils
{
    public class ConvertUtils
    {
        public static int ToInt(Color color) => (Mathf.RoundToInt(color.a * 255) << 24) +
                                                (Mathf.RoundToInt(color.r * 255) << 16) +
                                                (Mathf.RoundToInt(color.g * 255) << 8) +
                                                Mathf.RoundToInt(color.b * 255);

        public static Color ToColor(int value) => new Color(
            (value >> 24 & 0xFF) / 255f,
            (value >> 16 & 0xFF) / 255f,
            (value >> 8 & 0xFF) / 255f,
            (value & 0xFF) / 255f
        );

        public static Color ToColor(Vector3 value) => new Color(value.x, value.y, value.z);

        public static Color ToColor(object value)
        {
            if (value is Color) return (Color)value;
            if (value is Color32) return ToColor((Color32)value);
            if (value is Vector3) return ToColor((Vector3)value);
            if (value is Vector4) return ToColor((Vector4)value);
            return ToColor(ToInt(value));
        }

        public static Vector3 ToVector3(Color value) => new Vector3(value.r, value.g, value.b);
        public static Vector4 ToVector4(Color value) => new Vector4(value.r, value.g, value.b, value.a);

        #region ConvertToUInt

        public static uint ToUInt(sbyte value) => System.Convert.ToUInt32(value);
        public static uint ToUInt(byte value) => System.Convert.ToUInt32(value);
        public static uint ToUInt(short value) => System.Convert.ToUInt32(value);
        public static uint ToUInt(ushort value) => System.Convert.ToUInt32(value);
        public static uint ToUInt(int value) => System.Convert.ToUInt32(value & 0xffffffffu);
        public static uint ToUInt(uint value) => value;
        public static uint ToUInt(long value) => System.Convert.ToUInt32(value & 0xffffffffu);
        public static uint ToUInt(ulong value) => System.Convert.ToUInt32(value & 0xffffffffu);
        public static uint ToUInt(bool value) => value ? 1u : 0u;
        public static uint ToUInt(char value) => System.Convert.ToUInt32(value);

        public static uint ToUInt(object value)
        {
            if (value == null) return 0;
            if (!(value is System.IConvertible)) ToUInt(value.ToString());
            try
            {
                return System.Convert.ToUInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public static uint ToUInt(string value, System.Globalization.NumberStyles style) =>
            ToUInt(ToDouble(value, style));

        public static uint ToUInt(string value) => ToUInt(ToDouble(value, System.Globalization.NumberStyles.Any));

        #endregion

        #region ConvertToInt

        public static int ToInt(sbyte value) => System.Convert.ToInt32(value);
        public static int ToInt(byte value) => System.Convert.ToInt32(value);
        public static int ToInt(short value) => System.Convert.ToInt32(value);
        public static int ToInt(ushort value) => System.Convert.ToInt32(value);
        public static int ToInt(int value) => value;

        public static int ToInt(uint value)
        {
            if (value > int.MaxValue) return int.MinValue + System.Convert.ToInt32(value & 0x7fffffff);
            return System.Convert.ToInt32(value & 0xffffffff);
        }

        public static int ToInt(long value)
        {
            if (value > int.MaxValue) return int.MinValue + System.Convert.ToInt32(value & 0x7fffffff);
            return System.Convert.ToInt32(value & 0xffffffff);
        }

        public static int ToInt(ulong value)
        {
            if (value > int.MaxValue) return int.MinValue + System.Convert.ToInt32(value & 0x7fffffff);
            return System.Convert.ToInt32(value & 0xffffffff);
        }

        public static int ToInt(float value) => System.Convert.ToInt32(value);
        public static int ToInt(double value) => System.Convert.ToInt32(value);
        public static int ToInt(decimal value) => System.Convert.ToInt32(value);
        public static int ToInt(bool value) => value ? 1 : 0;
        public static int ToInt(char value) => System.Convert.ToInt32(value);

        public static int ToInt(object value)
        {
            if (value == null) return 0;
            else if (value is Color color) return ToInt(color);
            else if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToInt32(value);
                }
                catch
                {
                    return 0;
                }
            }

            return ToInt(value.ToString());
        }

        public static int ToInt(string value, System.Globalization.NumberStyles style) => ToInt(ToDouble(value, style));
        public static int ToInt(string value) => ToInt(ToDouble(value, System.Globalization.NumberStyles.Any));

        #endregion

        #region "ConvertToULong"

        public static ulong ToULong(sbyte value) => System.Convert.ToUInt64(value);
        public static ulong ToULong(byte value) => System.Convert.ToUInt64(value);
        public static ulong ToULong(short value) => System.Convert.ToUInt64(value);
        public static ulong ToULong(ushort value) => System.Convert.ToUInt64(value);
        public static ulong ToULong(int value) => System.Convert.ToUInt64(value & long.MaxValue);
        public static ulong ToULong(uint value) => System.Convert.ToUInt64(value);
        public static ulong ToULong(long value) => System.Convert.ToUInt64(value & long.MaxValue);
        public static ulong ToULong(ulong value) => value;
        public static ulong ToULong(bool value) => value ? 1ul : 0ul;
        public static ulong ToULong(char value) => System.Convert.ToUInt64(value);

        public static ulong ToULong(object value)
        {
            if (value == null) return 0;
            else if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToUInt64(value);
                }
                catch
                {
                    return 0;
                }
            }

            return ToULong(value.ToString());
        }

        public static ulong ToULong(string value, System.Globalization.NumberStyles style) =>
            ToULong(ToDouble(value, style));

        public static ulong ToULong(string value) => ToULong(ToDouble(value, System.Globalization.NumberStyles.Any));

        #endregion

        #region "ConvertToLong"

        public static long ToLong(sbyte value) => System.Convert.ToInt64(value);
        public static long ToLong(byte value) => System.Convert.ToInt64(value);
        public static long ToLong(short value) => System.Convert.ToInt64(value);
        public static long ToLong(ushort value) => System.Convert.ToInt64(value);
        public static long ToLong(int value) => System.Convert.ToInt64(value);
        public static long ToLong(uint value) => System.Convert.ToInt64(value);
        public static long ToLong(long value) => value;

        public static long ToLong(ulong value)
        {
            if (value > long.MaxValue) return int.MinValue + System.Convert.ToInt32(value & long.MaxValue);
            return System.Convert.ToInt64(value & long.MaxValue);
        }

        public static long ToLong(bool value) => value ? 1 : 0;
        public static long ToLong(char value) => System.Convert.ToInt64(value);

        public static long ToLong(object value)
        {
            if (value == null) return 0;
            if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToInt64(value);
                }
                catch
                {
                    return 0;
                }
            }

            return ToLong(value.ToString());
        }

        public static long ToLong(string value, System.Globalization.NumberStyles style) =>
            ToLong(ToDouble(value, style));

        public static long ToLong(string value) => ToLong(ToDouble(value, System.Globalization.NumberStyles.Any));

        #endregion

        #region "ToSingle"

        public static float ToSingle(sbyte value) => System.Convert.ToSingle(value);
        public static float ToSingle(byte value) => System.Convert.ToSingle(value);
        public static float ToSingle(short value) => System.Convert.ToSingle(value);
        public static float ToSingle(ushort value) => System.Convert.ToSingle(value);
        public static float ToSingle(int value) => System.Convert.ToSingle(value);
        public static float ToSingle(uint value) => System.Convert.ToSingle(value);
        public static float ToSingle(long value) => System.Convert.ToSingle(value);
        public static float ToSingle(ulong value) => System.Convert.ToSingle(value);
        public static float ToSingle(float value) => System.Convert.ToSingle(value);
        public static float ToSingle(double value) => (float)value;
        public static float ToSingle(decimal value) => System.Convert.ToSingle(value);
        public static float ToSingle(bool value) => value ? 1 : 0;
        public static float ToSingle(char value) => ToSingle(System.Convert.ToInt32(value));
        public static float ToSingle(Vector2 value) => value.x;
        public static float ToSingle(Vector3 value) => value.x;
        public static float ToSingle(Vector4 value) => value.x;

        public static float ToSingle(object value)
        {
            if (value == null) return 0;
            if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToSingle(value);
                }
                catch
                {
                    return 0;
                }
            }

            if (value is Vector2 v2) return ToSingle(v2);
            if (value is Vector3 v3) return ToSingle(v3);
            if (value is Vector4 v4) return ToSingle(v4);
            return ToSingle(value.ToString());
        }

        public static float ToSingle(string value, System.Globalization.NumberStyles style) =>
            System.Convert.ToSingle(ToDouble(value, style));

        public static float ToSingle(string value) =>
            System.Convert.ToSingle(ToDouble(value, System.Globalization.NumberStyles.Any));

        #endregion

        #region "ToDouble"

        public static double ToDouble(sbyte value) => System.Convert.ToDouble(value);
        public static double ToDouble(byte value) => System.Convert.ToDouble(value);
        public static double ToDouble(short value) => System.Convert.ToDouble(value);
        public static double ToDouble(ushort value) => System.Convert.ToDouble(value);
        public static double ToDouble(int value) => System.Convert.ToDouble(value);
        public static double ToDouble(uint value) => System.Convert.ToDouble(value);
        public static double ToDouble(long value) => System.Convert.ToDouble(value);
        public static double ToDouble(ulong value) => System.Convert.ToDouble(value);
        public static double ToDouble(float value) => System.Convert.ToDouble(value);
        public static double ToDouble(double value) => value;
        public static double ToDouble(decimal value) => System.Convert.ToDouble(value);
        public static double ToDouble(bool value) => value ? 1 : 0;
        public static double ToDouble(char value) => ToDouble(System.Convert.ToInt32(value));
        public static double ToDouble(Vector2 value) => value.x;
        public static double ToDouble(Vector3 value) => value.x;
        public static double ToDouble(Vector4 value) => value.x;

        public static double ToDouble(object value)
        {
            if (value == null) return 0;
            if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToDouble(value);
                }
                catch
                {
                    return 0;
                }
            }

            if (value is Vector2 v2) ToDouble(v2);
            if (value is Vector3 v3) return ToDouble(v3);
            if (value is Vector4 v4) return ToDouble(v4);
            return ToDouble(value.ToString(), System.Globalization.NumberStyles.Any, null);
        }

        public static double ToDouble(string value, System.Globalization.NumberStyles style,
            System.IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value)) return 0d;

            style = style & System.Globalization.NumberStyles.Any;
            double dbl = 0;
            if (double.TryParse(value, style, provider, out dbl))
            {
                return dbl;
            }
            else
            {
                //test hex
                int i;
                bool isNeg = false;
                for (i = 0; i < value.Length; i++)
                {
                    if (value[i] == ' ' || value[i] == '+') continue;
                    if (value[i] == '-')
                    {
                        isNeg = !isNeg;
                        continue;
                    }

                    break;
                }

                if (i < value.Length - 1 &&
                    (
                        (value[i] == '#') ||
                        (value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X')) ||
                        (value[i] == '&' && (value[i + 1] == 'h' || value[i + 1] == 'H'))
                    ))
                {
                    style = (style & System.Globalization.NumberStyles.HexNumber) |
                            System.Globalization.NumberStyles.AllowHexSpecifier;

                    if (value[i] == '#') i++;
                    else i += 2;
                    int j = value.IndexOf('.', i);

                    if (j >= 0)
                    {
                        long lng = 0;
                        long.TryParse(value.Substring(i, j - i), style, provider, out lng);

                        if (isNeg)
                            lng = -lng;

                        long flng = 0;
                        string sfract = value.Substring(j + 1).Trim();
                        long.TryParse(sfract, style, provider, out flng);
                        return System.Convert.ToDouble(lng) +
                               System.Convert.ToDouble(flng) / System.Math.Pow(16d, sfract.Length);
                    }
                    else
                    {
                        string num = value.Substring(i);
                        long l;
                        if (long.TryParse(num, style, provider, out l))
                            return System.Convert.ToDouble(l);
                        else
                            return 0d;
                    }
                }
                else
                {
                    return 0d;
                }
            }
        }

        public static double ToDouble(string value, System.Globalization.NumberStyles style) =>
            ToDouble(value, style, null);

        public static double ToDouble(string value) => ToDouble(value, System.Globalization.NumberStyles.Any, null);

        #endregion

        #region "ToString"

        public static string ToString(sbyte value) => System.Convert.ToString(value);
        public static string ToString(byte value) => System.Convert.ToString(value);
        public static string ToString(short value) => System.Convert.ToString(value);
        public static string ToString(ushort value) => System.Convert.ToString(value);
        public static string ToString(int value) => System.Convert.ToString(value);
        public static string ToString(uint value) => System.Convert.ToString(value);
        public static string ToString(long value) => System.Convert.ToString(value);
        public static string ToString(ulong value) => System.Convert.ToString(value);
        public static string ToString(float value) => System.Convert.ToString(value);
        public static string ToString(double value) => System.Convert.ToString(value);
        public static string ToString(decimal value) => System.Convert.ToString(value);

        public static string ToString(bool value, string sFormat)
        {
            switch (sFormat)
            {
                case "num":
                    return (value) ? "1" : "0";
                case "normal":
                case "":
                case null:
                    return System.Convert.ToString(value);
                default:
                    return System.Convert.ToString(value);
            }
        }

        public static string ToString(bool value) => System.Convert.ToString(value);
        public static string ToString(char value) => System.Convert.ToString(value);
        public static string ToString(object value) => System.Convert.ToString(value);
        public static string ToString(string str) => str;

        public static string Stringify(object obj)
        {
            if (obj == null)
                return null;

            if (obj is string)
                return obj as string;
            if (obj is System.Enum e) return e.ToString();
            if (obj is bool) return (bool)obj ? "1" : "0";
            if (obj is Vector2 v2) return v2.ToString();
            if (obj is Vector3 v3) return v3.ToString();
            if (obj is Vector4 v4) return v4.ToString();
            if (obj is Quaternion q) return q.ToString();
            if (obj is Color) return ToInt((Color)obj).ToString();
            return System.Convert.ToString(obj);
        }

        #endregion

        #region "ToBool"

        public static bool ToBool(sbyte value) => value != 0;
        public static bool ToBool(byte value) => value != 0;
        public static bool ToBool(short value) => value != 0;
        public static bool ToBool(ushort value) => value != 0;
        public static bool ToBool(int value) => value != 0;
        public static bool ToBool(uint value) => value != 0;
        public static bool ToBool(long value) => value != 0;
        public static bool ToBool(ulong value) => value != 0;
        public static bool ToBool(float value) => value != 0;
        public static bool ToBool(double value) => value != 0;
        public static bool ToBool(decimal value) => value != 0;
        public static bool ToBool(bool value) => value;
        public static bool ToBool(char value) => System.Convert.ToInt32(value) != 0;

        public static bool ToBool(object value)
        {
            if (value == null) return false;
            if (value is string str) return ToBool(str);
            if (value is System.IConvertible)
            {
                try
                {
                    return System.Convert.ToBoolean(value);
                }
                catch
                {
                    return false;
                }
            }

            return ToBool(value.ToString());
        }

        #endregion

        #region ToVector2

        public static Vector2 ToVector2(string value)
        {
            if (System.String.IsNullOrEmpty(value)) return Vector2.zero;
            var arr = value.Replace(" ", "").Split(',');
            return new Vector2(
                ToSingle(ArrayUtils.GetOrDefault(arr, 0, "0")),
                ToSingle(ArrayUtils.GetOrDefault(arr, 1, "0"))
            );
        }

        public static Vector2 ToVector2(float value)
        {
            return new Vector2(value, value);
        }

        public static Vector2 ToVector2(object value)
        {
            if (value == null) return Vector2.zero;
            if (value is Vector2 v2) return v2;
            if (value is Vector3 v3) return v3;
            if (value is Vector4 v4) return v4;
            if (value is Quaternion q) return new Vector2(q.x, q.y);
            if (value is Color c) return new Vector2(c.r, c.g);
            if (ValueIsNumericType(value)) return Vector2.one * ToSingle(value);
            return ToVector2(System.Convert.ToString(value));
        }

        #endregion

        #region ToVector3

        public static Vector3 ToVector3(string value)
        {
            if (System.String.IsNullOrEmpty(value)) return Vector3.zero;
            var arr = value.Replace(" ", "").Split(',');
            return new Vector3(
                ToSingle(ArrayUtils.GetOrDefault(arr, 0, "0")),
                ToSingle(ArrayUtils.GetOrDefault(arr, 1, "0")),
                ToSingle(ArrayUtils.GetOrDefault(arr, 2, "0"))
            );
        }

        public static Vector3 ToVector3(float value)
        {
            return new Vector3(value, value, value);
        }
        
        public static Vector3 ToVector3(object value)
        {
            if (value == null) return Vector2.zero;
            if (value is Vector2 v2) return v2;
            if (value is Vector3 v3) return v3;
            if (value is Vector4 v4) return v4;
            if (value is Quaternion q) return new Vector3(q.x, q.y, q.z);
            if (value is Color c) return new Vector3(c.r, c.g, c.b);
            if (ValueIsNumericType(value)) return Vector3.one * ToSingle(value);
            return ToVector2(System.Convert.ToString(value));
        }

        #endregion

        #region ToQuaternion

        public static Quaternion ToQuaternion(string value)
        {
            if (System.String.IsNullOrEmpty(value)) return Quaternion.identity;
            var arr = value.Replace(" ", "").Split(',');
            return new Quaternion(
                ToSingle(ArrayUtils.GetOrDefault(arr, 0, "0")),
                ToSingle(ArrayUtils.GetOrDefault(arr, 1, "0")),
                ToSingle(ArrayUtils.GetOrDefault(arr, 2, "0")),
                ToSingle(ArrayUtils.GetOrDefault(arr, 3, "0"))
            );
        }

        #endregion

        public static bool IsNumeric(object value,
            System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Any,
            System.IFormatProvider provider = null, bool bBlankIsZero = false)
        {
            if (value == null) return bBlankIsZero;
            if (ValueIsNumericType(value)) return true;

            string sval = System.Convert.ToString(value);
            if (string.IsNullOrEmpty(sval))
                return bBlankIsZero;

            sval = sval.Trim();

            if (IsHex(sval))
            {
                return true;
            }
            else
            {
                style = style & System.Globalization.NumberStyles.Any;
                double dbl = 0;
                return double.TryParse(sval, style, provider, out dbl);
            }
        }

        public static bool IsHex(string value)
        {
            int i;
            for (i = 0; i < value.Length; i++)
            {
                if (value[i] == ' ' || value[i] == '+' || value[i] == '-') continue;

                break;
            }

            return (i < value.Length - 1 &&
                    (
                        (value[i] == '#') ||
                        (value[i] == '0' && (value[i + 1] == 'x' || value[i + 1] == 'X')) ||
                        (value[i] == '&' && (value[i + 1] == 'h' || value[i + 1] == 'H'))
                    ));
        }

        public static bool ValueIsNumericType(object obj)
        {
            if (obj == null) return false;

            var tp = obj.GetType();
            return tp.IsEnum || IsNumericType(System.Type.GetTypeCode(tp));
        }

        public static bool IsNumericType(System.TypeCode code)
        {
            switch (code)
            {
                case System.TypeCode.SByte:
                case System.TypeCode.Byte:
                case System.TypeCode.Int16:
                case System.TypeCode.UInt16:
                case System.TypeCode.Int32:
                case System.TypeCode.UInt32:
                case System.TypeCode.Int64:
                case System.TypeCode.UInt64:
                case System.TypeCode.Single:
                case System.TypeCode.Double:
                case System.TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }
    }
}