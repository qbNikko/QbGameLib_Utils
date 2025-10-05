using System;
using System.Collections;
using System.Collections.Generic;

namespace QbGameLib_Utils.Utils
{
    public static class TypeUtils
    {
        /**
         * Получение типа коллекции
         */
        public static Type GetTypeOfCollection(this Type type)
        {
            if (type == null) return null;
            if (type.IsArray) return type.GetElementType();
            var interfaces = type.GetInterfaces();
            if (Array.IndexOf(interfaces, typeof(ICollection)) >= 0 ||
                Array.IndexOf(interfaces, typeof(ICollection)) >= 0)
            {
                if (type.IsGenericType) return type.GetGenericArguments()[0];
                return typeof(object);
            }

            return null;
        }

        /**
         * Рекурсивный анализ соответствия типа, включая назначения в качестве Generic
         */
        public static bool IsType(Type type, Type assignable)
        {
            if (type == null || assignable == null) return false;
            if (type == assignable) return true;
            if (!assignable.IsGenericTypeDefinition) return assignable.IsAssignableFrom(type);

            Type currentType;
            if (assignable.IsInterface)
            {
                currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (currentType == assignable) return true;
                foreach (var itp in type.GetInterfaces())
                {
                    currentType = itp.IsGenericType ? itp.GetGenericTypeDefinition() : itp;
                    if (currentType == assignable) return true;
                }

                return false;
            }

            while (type != null && type != typeof(object))
            {
                currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (currentType == assignable) return true;
                type = type.BaseType;
            }

            return false;
        }

        /**
         * Рекурсивный анализ соответствия типа для множества совпадений, включая назначения в качестве Generic
         */
        public static bool IsType(Type type, params Type[] assignables)
        {
            foreach (var otp in assignables)
            {
                if (IsType(type, otp)) return true;
            }

            return false;
        }
        
        /**
         * Проверка что тип является коллекцией
         */
        public static bool IsListType(this Type tp)
        {
            if (tp == null) return false;
            if (tp.IsArray) return tp.GetArrayRank() == 1;
            var interfaces = tp.GetInterfaces();
            if (Array.IndexOf(interfaces, typeof(IList)) >= 0 
                || Array.IndexOf(interfaces, typeof(IList<>)) >= 0)
            {
                return true;
            }
            return false;
        }
    }
}