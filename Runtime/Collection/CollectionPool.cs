using System.Collections.Generic;
using UnityEngine.Pool;

namespace QbGameLib_Utils.Collection
{
    public static class CollectionPool
    {
        public static PooledObject<List<T>> GetList<T>(out List<T> obj)
        {
            return CollectionPool<List<T>,T>.Get(out obj);
        }
        
        public static PooledObject<HashSet<T>> GetSet<T>(out HashSet<T> obj)
        {
            return CollectionPool<HashSet<T>,T>.Get(out obj);
        }
        
        public static PooledObject<Dictionary<K,V>> GetDictionary<K,V>(out Dictionary<K,V>  obj)
        {
            return DictionaryPool<K,V>.Get(out obj);
        }
    }
}