using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnHeight;

    private List<SpawnArea> spawnAreas; // May need to replace with a more specialised container to avoid repetition

    private void Awake()
    {
        var areas = GetComponentsInChildren<SpawnArea>();
        spawnAreas = new List<SpawnArea>(areas);
    }

    [Button(ButtonExecutionModes.PlayMode)]
    public void SpawnObject()
    {
        Vector3 position = GetRandomSpawnLocation();
        var instance = Instantiate(prefab, position, GetRandomRotation());
    }

    public Quaternion GetRandomRotation()
    {
        float yRotation = Random.Range(0, 360);
        return Quaternion.Euler(0, yRotation, 0);
    }

    public Vector3 GetRandomSpawnLocation()
    {
        int selectedIndex = Random.Range(0, spawnAreas.Count);
        SpawnArea spawnArea = spawnAreas[selectedIndex];
        return spawnArea.GetRandomPointAtHeight(spawnHeight);
    }
}
