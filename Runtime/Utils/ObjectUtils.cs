using System;
using System.Reflection;

namespace QbGameLib_Utils.Utils
{
    public static class ObjectUtils
    {
        public static MemberInfo GetMemberFromType(Type type, string name, bool includeNonPublic = false,bool includeInterface = false, MemberTypes mask = MemberTypes.Field | MemberTypes.Property | MemberTypes.Method)
        {
            const BindingFlags PUBLIC_FLAG = BindingFlags.Public | BindingFlags.Instance;
            const BindingFlags PRIVATE_FLAG = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (type == null) throw new ArgumentNullException("type");
            try
            {
                MemberInfo[] members = type.GetMember(name, PUBLIC_FLAG);
                foreach (var member in members)
                {
                    if ((member.MemberType & mask) != 0) return member;
                }
                if (includeInterface && type.IsInterface)
                {
                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        members = interfaceType.GetMember(name, PUBLIC_FLAG);
                        foreach (var member in members)
                        {
                            if ((member.MemberType & mask) != 0) return member;
                        }
                    }
                }
                else if (includeNonPublic)
                {
                    while (type != null)
                    {
                        members = type.GetMember(name, PRIVATE_FLAG);
                        type = type.BaseType;
                        if (members == null || members.Length == 0) continue;

                        foreach (var member in members)
                        {
                            if ((member.MemberType & mask) != 0) return member;
                        }
                    }
                }
            }
            catch
            {

            }
            return null;
        }
    }
}