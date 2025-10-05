using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using CollectionPool = QbGameLib_Utils.Collection.CollectionPool;
using DisposeContainer = QbGameLib_Utils.Component.Dispose.DisposeContainer;
using DisposeObjectHandler = QbGameLib_Utils.Component.Dispose.DisposeObjectHandler;

namespace QbGameLib_Utils.Component.DI
{
    public class DIService
    {
        private static DIService _instance;
        private DisposeContainer  _disposableContainer;
        private Dictionary<Type, object> _singletonDictionary;
        private Dictionary<Type, List<object>> _multiDictionary;
        private Dictionary<Type, Dictionary<string,object>> _namedDictionary;

        private DIService()
        {
            _disposableContainer = new DisposeContainer();
            _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out _singletonDictionary));
            _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out _multiDictionary));
            _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out _namedDictionary));
            
        }

        private static DIService Instance
        {
            get
            {
                if (_instance == null) _instance = new DIService();
                return _instance;
            }
        }

        public static IDisposable RegisterSingleton<T>(T instance) where T : class
        {
            return Instance.RegisterSingleton_(instance);
        }
        
        public static IDisposable RegisterNamed<T>(string name, T instance) where T : class
        {
            return Instance.RegisterNamed_(name, instance);
        }

        public static IDisposable RegisterMulti<T>(T instance) where T : class
        {
            return Instance.RegisterMulti_(instance);
        }

        public static bool TryGetSingleton<T>(out T instance) where T : class
        {
            return Instance.TryGetSingleton_(out instance);
        }
        
        public static bool TryGetAll<T>(out ReadOnlyCollection<T> instance) where T:class
        {
            return Instance.TryGetAll_(out instance);
        }
        
        public static bool TryGetNamed<T>(string name, out T instance) where T : class
        {
            return Instance.TryGetNamed_(name, out instance);
        }
        
        public static T GetSingleton<T>() where T : class
        {
            Instance.TryGetSingleton_(out T instance);
            return instance;
        }
        
        public static ReadOnlyCollection<T> GetAll<T>() where T:class
        {
            Instance.TryGetAll_(out ReadOnlyCollection<T> instance);
            return instance;
        }
        
        public static T GetNamed<T>(string name) where T : class
        {
            Instance.TryGetNamed_(name, out T instance);
            return instance;
        }

        public IDisposable RegisterSingleton_<T>(T instance) where T:class
        {
            Type type = typeof(T);
            if(_singletonDictionary.ContainsKey(type)) throw new Exception("Singleton "+type+"object already registered");
            _singletonDictionary.Add(type, instance);
            Debug.LogFormat("Singleton {0} registered", type);
            return DisposeObjectHandler.Create(() =>
            {
                Debug.LogFormat("Singleton {0} unregistered", type);
                _singletonDictionary.Remove(type);
            });
        }
        
        public IDisposable RegisterNamed_<T>(string name, T instance) where T:class
        {
            Type type = typeof(T);
            Dictionary<string, object> namedDictionary;
            if (!_namedDictionary.TryGetValue(type, out namedDictionary))
            {
                _disposableContainer.AddDisposable(CollectionPool.GetDictionary(out namedDictionary));
                _namedDictionary.Add(type,namedDictionary);
            }
            if (namedDictionary.ContainsKey(name)) throw new Exception("Object "+type+":"+name+" already registered");
            namedDictionary.Add(name, instance);
            Debug.LogFormat("Named Singleton {0}:{1} registered", type, name);
            return DisposeObjectHandler.Create(() =>
            {
                Debug.LogFormat("Named Singleton {0}:{1} unregistered", type, name);
                namedDictionary.Remove(name);
            });
        }
        
        public IDisposable RegisterMulti_<T>(T instance) where T:class
        {
            Type type = typeof(T);
            List<object> list;
            if (!_multiDictionary.TryGetValue(type,out list))
            {
                _disposableContainer.AddDisposable(CollectionPool.GetList(out list));
                _multiDictionary.Add(type,list);
            }
            list.Add(instance);
            Debug.LogFormat("Register {0}:{1} count: {2}", type, instance, list.Count);
            return DisposeObjectHandler.Create(() =>
            {
                Debug.LogFormat("Unregister {0}:{1} count: {2}", type, instance, list.Count);
                list.Remove(instance);
            });
        }

        public bool TryGetSingleton_<T>(out T instance) where T:class
        {
            Type type = typeof(T);
            if (_singletonDictionary.TryGetValue(type, out object value))
            {
                instance = value as T;
                return true;
            }
            instance = null;
            return false;
        }
        
        public bool TryGetAll_<T>(out ReadOnlyCollection<T> instance) where T:class
        {
            Type type = typeof(T);
            if (_multiDictionary.TryGetValue(type, out List<object> value))
            {
                instance = new ReadOnlyCollection<T>(value.Cast<T>().ToList());
                return true;
            }
            instance = null;
            return false;
        }
        
        public bool TryGetNamed_<T>(string name, out T instance) where T:class
        {
            Type type = typeof(T);
            Dictionary<string, object> namedDictionary;
            if (_namedDictionary.TryGetValue(type, out namedDictionary))
            {
                if (namedDictionary.TryGetValue(name, out object value))
                {
                    instance = value as T;
                    return true;
                }
            }
            instance = null;
            return false;
        }
    }
}