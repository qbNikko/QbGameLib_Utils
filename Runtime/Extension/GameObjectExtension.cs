using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QbGameLib_Utils.Extension
{
    public static class GameObjectExtensions
    {
        public static void DestroyChildren(this GameObject t)
        {
            t.transform.Cast<Transform>().ToList().ForEach(c => Object.Destroy(c.gameObject));
        }

        public static void DestroyChildrenImmediate(this GameObject t)
        {
            t.transform.Cast<Transform>().ToList().ForEach(c => Object.DestroyImmediate(c.gameObject));
        }
        
        public static bool CheckParent(this Transform t, Transform parent)
        {
            Transform parentGameObject = t.parent;
            while (parentGameObject!=null)
            {
                if(parentGameObject ==parent) return true;
                parentGameObject = parentGameObject.parent;
            }
            return false;
        }
        
        public static bool TryFindFirstByName(this GameObject t, string name, out GameObject result)
        {
            Transform transform = t.transform;
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Equals(name)) 
                {
                    result = child.gameObject;
                    return true;
                }
            }
            foreach (Transform child in transform)
            {
                if (TryFindFirstByName(child.gameObject, name, out result)) 
                {
                    return true;
                }
            }
            result = null;
            return false;
        }
        
        public static bool TryFindAllByName(this GameObject t, string name, out List<GameObject> result)
        {
            Transform transform = t.transform;
            result = new List<GameObject>();
            foreach (Transform child in transform)
            {
                if (child.gameObject.name.Equals(name)) 
                {
                    result.Add(child.gameObject);
                }
            }
            foreach (Transform child in transform)
            {
                if (TryFindAllByName(child.gameObject, name, out result)) 
                {
                    return true;
                }
            }
            return result.Count>0;
        }
    }
}