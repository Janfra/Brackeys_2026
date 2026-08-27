using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour, ISpawnableDespawner
{
    public event UnityAction<GameObject> OnSpawn;
    public event UnityAction<GameObject> OnDespawn;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnHeight;

    private List<SpawnArea> spawnAreas; // May need to replace with a more specialised container to avoid repetition
    private ObjectPool<GameObject> objectPool;

    private void Awake()
    {
        objectPool = new(OnCreate, OnGet, OnRelease);
        var areas = GetComponentsInChildren<SpawnArea>();
        spawnAreas = new List<SpawnArea>(areas);
    }

    [Button(ButtonExecutionModes.PlayMode)]
    public void SpawnObject()
    {
        var instance = objectPool.Get();
    }

    public T SpawnObject<T>() where T : Component
    {
        var instance = objectPool.Get();
        return instance.GetComponent<T>();
    }

    public void Despawn(GameObject @object)
    {
        objectPool.Release(@object);
    }

    private GameObject OnCreate()
    {
        var instance = Instantiate(prefab, GetRandomSpawnLocation(), GetRandomRotation());
        if (instance.TryGetComponent(out ISpawnable spawnable))
        {
            spawnable.Despawner = this;
        }

        return instance;
    }

    private void OnGet(GameObject @object)
    {
        @object.SetActive(true);
        @object.transform.SetParent(null);
        OnSpawn?.Invoke(@object);
    }

    private void OnRelease(GameObject @object)
    {
        @object.SetActive(false);
        @object.transform.SetParent(transform);
        @object.transform.rotation = GetRandomRotation();
        @object.transform.position = GetRandomSpawnLocation();
        OnDespawn?.Invoke(@object);
    }

    private Quaternion GetRandomRotation()
    {
        float yRotation = Random.Range(0, 360);
        return Quaternion.Euler(0, yRotation, 0);
    }

    private Vector3 GetRandomSpawnLocation()
    {
        int selectedIndex = Random.Range(0, spawnAreas.Count);
        SpawnArea spawnArea = spawnAreas[selectedIndex];
        return spawnArea.GetRandomPointAtHeight(spawnHeight);
    }

}
