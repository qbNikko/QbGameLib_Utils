using System;
using UnityEngine;

namespace QbGameLib_Utils.Attribute
{
    [AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class TagAttribute :PropertyAttribute
    {
        
    }
}