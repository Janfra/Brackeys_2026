using Janito.EditorExtras;
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
    private int maxSpawnCount = 3;

    [SerializeField]
    private int initialSpawnCount = 1;

    [Header("Debug")]
    [SerializeField]
    [ReadOnly]
    private Timer spawnTimer;
    
    [SerializeField]
    [ReadOnly]
    private int spawnCount;

    private bool canSpawnMore => spawnCount < maxSpawnCount;
    private bool needsMoreSpawned => spawnCount < minSpawnCount;

    private void Awake()
    {
        if (packageSpawner == null)
        {
            packageSpawner = GetComponent<Spawner>();
        }

        packageSpawner.OnDespawn += UpdateSpawnCount;

        spawnTimer = new(spawnDelay.GetRandomInRange(), true, OnSpawnTimer);
    }

    private void Start()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnPackage();
        }

        spawnTimer.IsRunning = true;
    }

    private void Update()
    {
        if (canSpawnMore)
        {
            spawnTimer.Update(Time.deltaTime);
        }
    }

    private void UpdateSpawnCount(GameObject package)
    {
        spawnCount--;
        if (needsMoreSpawned)
        {
            SpawnPackage(); // Could line up with timer spawn
        }
    }

    public void SpawnPackage()
    {
        spawnCount++;
        packageSpawner.SpawnObject();
    }

    private void OnSpawnTimer()
    {
        SpawnPackage();
        spawnTimer.Duration = spawnDelay.GetRandomInRange();
    }
}
