using System.Collections.Generic;
using Code.Player;
using UnityEngine;
using Zenject;

namespace Code.Pooling
{

    public abstract class ObjectPool<T> where T : Component
    {
        private DiContainer _container;
        private Queue<T> _objectPool = new Queue<T>();
        private T _prefab;

        public ObjectPool(T prefab, int initialSize,DiContainer container)
        {
            _prefab = prefab;
            _container = container;
            for (int i = 0; i < initialSize; i++)
            {
                var obj = _container.InstantiatePrefabForComponent<T>(_prefab);
                obj.gameObject.SetActive(false);
                _objectPool.Enqueue(obj);
            }
        }

        public T GetObject()
        {
            T obj;
            if (_objectPool.Count > 0)
            {
                obj = _objectPool.Dequeue();
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = _container.InstantiatePrefabForComponent<T>(_prefab);
            }
            return obj;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }

}