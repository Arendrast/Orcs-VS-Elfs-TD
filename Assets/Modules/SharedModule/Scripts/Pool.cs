using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Modules.SharedModule.Scripts
{
    public class Pool<T> where T : Component
    {
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;
        private readonly bool _createMissingPoolForPrefabOnGet;
        private readonly int _defaultCapacity;
        private readonly int _maxSize;

        private readonly Dictionary<T, ObjectPool<T>> _poolByPrefab = new Dictionary<T, ObjectPool<T>>();
        private readonly Dictionary<T, T> _prefabByInstance = new Dictionary<T, T>();
        private readonly Transform _parent;

        public Pool(Action<T> onGet, Action<T> onRelease, Action<T> onDestroy,
            bool createMissingPoolForPrefabOnGet = true, int defaultCapacity = 30, int maxSize = 40)
        {
            _parent = new GameObject($"Pool<{typeof(T).Name}>").transform;
            _onGet = onGet;
            _onRelease = onRelease;
            _onDestroy = onDestroy;
            _createMissingPoolForPrefabOnGet = createMissingPoolForPrefabOnGet;
            _defaultCapacity = defaultCapacity;
            _maxSize = maxSize;
        }

        public T TryGet(T prefab)
        {
            if (!_poolByPrefab.TryGetValue(prefab, out var pool))
            {
                if (!_createMissingPoolForPrefabOnGet)
                {
                    return null;
                }

                pool = GetObjectPool(prefab);
                _poolByPrefab[prefab] = pool;
            }

            var instance = pool.Get();
            _prefabByInstance[instance] = prefab;

            return instance;
        }

        public void TryRelease(T component)
        {
            if (!_prefabByInstance.TryGetValue(component, out var prefab) ||
                !_poolByPrefab.TryGetValue(prefab, out var pool))
            {
                return;
            }

            pool.Release(component);
        }

        private ObjectPool<T> GetObjectPool(T prefab)
        {
            return new ObjectPool<T>(
                createFunc: () => CreateComponent(prefab),
                OnGet,
                OnRelease,
                OnDestroy,
                collectionCheck: true,
                defaultCapacity: _defaultCapacity,
                maxSize: _maxSize);
        }

        private T CreateComponent(T prefab)
        {
            return UnityEngine.Object.Instantiate(prefab, _parent);
        }
        
        private void OnGet(T component)
        {
            component.gameObject.SetActive(true);
            _onGet?.Invoke(component);
        }

        private void OnRelease(T component)
        {
            component.gameObject.SetActive(false);
            _onRelease?.Invoke(component);
        }

        private void OnDestroy(T component)
        {
            UnityEngine.Object.Destroy(component.gameObject);
            _onDestroy?.Invoke(component);
        }
    }
}