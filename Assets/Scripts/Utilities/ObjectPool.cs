using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> where T : Component
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>Get an object from the pool (creates a new one if pool is empty).</summary>
    public T Get()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>Return an object to the pool instead of destroying it.</summary>
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
