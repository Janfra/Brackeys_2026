using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

[Serializable]
public class ComponentPooler<T> : ISpawnableDespawner<T>
    where T : Component, ISpawnable<T>
{
    [SerializeField]
    private T prefab;
    [SerializeField]
    [Tooltip("The parent transform under which the pooled components will be instantiated.")]
    private Transform parentTransform;
    [SerializeField]
    [Tooltip("The number of extra components to create in addition to the pool size. Only available through editor inspector.")]
    private int bufferSize;
    [SerializeField]
    [Tooltip("The maximum number of components that can be created in the pool.")]
    private int maxSize = 10000;
    [SerializeField]
    [Tooltip("If true, the pool will check if the component is already in the pool before releasing it. This can help prevent errors, but may have a performance impact.")]
    private bool hasCollectionCheck = true;

    private ObjectPool<T> objectPool;
    private ISpawnableDespawner<T> despawner;

    public ComponentPooler(T prefab, Transform parentTransform, int poolSize = 10, int maxSize = 10000, bool hasCollectionCheck = true, ISpawnableDespawner<T> despawner = null)
    {
        this.prefab = prefab;
        this.parentTransform = parentTransform;  
        this.maxSize = maxSize;
        this.hasCollectionCheck = hasCollectionCheck;
        Initialize(poolSize, despawner);
    }

    public void Initialize(int poolSize = 10, ISpawnableDespawner<T> despawner = null)
    {
        this.despawner = despawner == null ? this : despawner;
        objectPool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDestroy, hasCollectionCheck, poolSize + bufferSize, maxSize);
    }

    public T GetComponent()
    {
        return objectPool.Get();
    }

    public void ReleaseComponent(T instance)
    {
        objectPool.Release(instance);
    }

    /// <summary>
    /// Used as fallback for despawner if instance is not provided
    /// </summary>
    /// <param name="instance"></param>
    public void Despawn(T instance)
    {
        ReleaseComponent(instance);
    }

    protected virtual void OnGet(T instance)
    {
        instance.gameObject.SetActive(true);
    }

    protected virtual T OnCreate()
    {
        var instance = Object.Instantiate(prefab, parentTransform);
        instance.gameObject.SetActive(false);
        instance.Despawner = despawner;
        return instance;
    }

    protected virtual void OnRelease(T instance)
    {
        instance.gameObject.SetActive(false);
        // Make sure the instance is parented to the pooler transform to avoid cluttering the scene hierarchy and to ensure that the instance is not destroyed when the parent is destroyed.
        instance.transform.SetParent(parentTransform);
    }

    protected virtual void OnDestroy(T instance)
    {

    }
}
