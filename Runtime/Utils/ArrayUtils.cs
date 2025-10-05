namespace QbGameLib_Utils.Utils
{
    public static class ArrayUtils
    {
        public static T GetOrDefault<T>(this T[] arr, int idx, T defaultValue)
        {
            return arr.Length > idx ? arr[idx] : defaultValue;
        }
    }
}