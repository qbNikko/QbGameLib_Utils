using System.Collections.Generic;
using System.Linq;

namespace QbGameLib_Utils.Collection
{
    public abstract class DrawableDictionary
    {

#if UNITY_EDITOR
        public const string PROP_KEYS = "_keys";
        public const string PROP_VALUES = "_values";
#endif

    }

    [System.Serializable()]
    public abstract class SerializableDictionaryBase<TKey, TValue> : DrawableDictionary, IDictionary<TKey, TValue>, UnityEngine.ISerializationCallbackReceiver
    {
        [System.NonSerialized()]
        private Dictionary<TKey, TValue> _dict;
        [System.NonSerialized()]
        private IEqualityComparer<TKey> _comparer;


        public SerializableDictionaryBase()
        {
            _dict = new Dictionary<TKey, TValue>();
        }
        public SerializableDictionaryBase(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(_comparer);
        }

        public IEqualityComparer<TKey> Comparer => _comparer;
        
        public int Count => _dict.Count;
        public void Add(TKey key, TValue value) => _dict.Add(key, value);
        public bool ContainsKey(TKey key) => _dict.ContainsKey(key);
        public ICollection<TKey> Keys=> _dict.Keys;
        public bool Remove(TKey key) => _dict.Remove(key);
        public bool TryGetValue(TKey key, out TValue value) => _dict.TryGetValue(key, out value);
        public ICollection<TValue> Values => _dict.Values;

        public TValue this[TKey key]
        {
            get=> _dict[key];
            set
            {
                _dict[key] = value;
            }
        }
        public void Clear() => _dict.Clear();
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)=>_dict.Contains(item);
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            (_dict as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => _dict.Remove(item.Key);
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        public Dictionary<TKey, TValue>.Enumerator GetEnumerator() => _dict.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _dict.GetEnumerator();
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => _dict.GetEnumerator();

        
        [UnityEngine.SerializeField()]
        private TKey[] _keys;
        [UnityEngine.SerializeField()]
        private TValue[] _values;

        void UnityEngine.ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_keys != null && _values != null)
            {
                if (_dict == null) _dict = new Dictionary<TKey, TValue>(_keys.Length, _comparer);
                else _dict.Clear();
                for (int i = 0; i < _keys.Length; i++)
                {
                    if (i < _values.Length)
                        _dict[_keys[i]] = _values[i];
                    else
                        _dict[_keys[i]] = default(TValue);
                }
            }
            _keys = null;
            _values = null;
        }

        void UnityEngine.ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (_dict.Count == 0)
            {
                _keys = null;
                _values = null;
            }
            else
            {
                int cnt = _dict.Count;
                _keys = new TKey[cnt];
                _values = new TValue[cnt];
                int i = 0;
                var e = _dict.GetEnumerator();
                while (e.MoveNext())
                {
                    _keys[i] = e.Current.Key;
                    _values[i] = e.Current.Value;
                    i++;
                }
            }
        }
    }

    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue> { }
}