using Janito.EditorExtras;
using UnityEngine;

public class SpawningConfigurationSO : ScriptableObject
{
    [SerializeField]
    private FloatRange spawnDelay = new(5.0f, 10.0f);

    [field: SerializeField]
    public int MinSpawnCount = 1;

    [field: SerializeField]
    public int MaxSpawnCount = 3;

    [field: SerializeField]
    public int InitialSpawnCount = 1;

    [ReadOnly]
    public float LastDelay { get; private set; }

    public float GetRandomDelay()
    {
        LastDelay = spawnDelay.GetRandomInRange();
        return LastDelay;
    }
}
