using Janito.EditorExtras;
using System;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
public class PackageSpawner : MonoBehaviour
{
    [SerializeField]
    private Spawner packageSpawner;

    [SerializeField]
    private FloatRange spawnDelay = new(5.0f, 10.0f);

    [SerializeField]
    private int minSpawnCount = 1;

    [SerializeField]
    private int initialSpawnCount = 1;

    private int spawnCount;

    private void Awake()
    {
        if (packageSpawner == null)
        {
            packageSpawner = GetComponent<Spawner>();
        }

        packageSpawner.OnDespawn += UpdateSpawnCount;
    }

    private void Start()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnPackage();
        }
    }

    private void UpdateSpawnCount(GameObject package)
    {
        spawnCount--;
        if (spawnCount < minSpawnCount)
        {
            SpawnPackage();
        }
    }

    public void SpawnPackage()
    {
        spawnCount++;
        packageSpawner.SpawnObject();
    }

}
