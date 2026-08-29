using Janito.EditorExtras;
using UnityEngine;

[RequireComponent(typeof(Spawner))]
public class PackageSpawner : MonoBehaviour
{
    [SerializeField]
    private Spawner packageSpawner;

    [SerializeField]
    [CreateButton(savePath: PathUtils.ProjectConfigurationPath + "/Spawner")]
    [InlineInspector]
    private SpawningConfigurationSO spawningConfiguration;

    [Header("Debug")]
    [SerializeField]
    [ReadOnly]
    private Timer spawnTimer;
    
    [SerializeField]
    [ReadOnly]
    private int spawnCount;

    private bool canSpawnMore => spawnCount < spawningConfiguration.MaxSpawnCount;
    private bool needsMoreSpawned => spawnCount < spawningConfiguration.MinSpawnCount;

    private void Awake()
    {
        if (packageSpawner == null)
        {
            packageSpawner = GetComponent<Spawner>();
        }

        packageSpawner.OnDespawn += UpdateSpawnCount;

        spawnTimer = new(spawningConfiguration.GetRandomDelay(), true, OnSpawnTimer);
    }

    private void Start()
    {
        for (int i = 0; i < spawningConfiguration.InitialSpawnCount; i++)
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
        spawnTimer.Duration = spawningConfiguration.GetRandomDelay();
    }
}
