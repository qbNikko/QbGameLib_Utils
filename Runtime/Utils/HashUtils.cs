using System;
using System.Reflection;

namespace QbGameLib_Utils.Utils
{
    public static class HashUtils
    {
        public static string HashOfType(this Type type)
        {
            if (type != null) return type.Assembly.GetName().Name + "$" + type.FullName;
            return null;
        }
        

        public static Type ParseType(string hash)
        {
            if (string.IsNullOrEmpty(hash)) return null;
            var arr = hash.Split('$');
            var tp = ParseType(ArrayUtils.GetOrDefault(arr, 0,string.Empty),
                ArrayUtils.GetOrDefault(arr, 1,string.Empty));
            return tp;
        }
        
        public static Type ParseType(string assembName, string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name == assembName || assembly.FullName == assembName)
                {
                    foreach (Type t in assembly.GetTypes())
                    {
                        if (t.FullName == typeName) return t;
                    }
                    break;
                }
            }
            return null;
        }
    }
}