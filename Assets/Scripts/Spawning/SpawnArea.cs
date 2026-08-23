using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnExtents = Vector2.one;

    // Use half to centre the extents around the transform
    private Vector2 halfSpawnExtents;

    public Vector3 FlatSpawnExtents => new Vector3(spawnExtents.x, 0, spawnExtents.y);

    private void Awake()
    {
        halfSpawnExtents = spawnExtents / 2;
    }

    public Vector3 GetRandomPointAtHeight(float height)
    {
        float xOffset = Random.Range(-halfSpawnExtents.x, halfSpawnExtents.x);
        float zOffset = Random.Range(-halfSpawnExtents.y, halfSpawnExtents.y);
        Vector3 position = new Vector3(xOffset + transform.position.x, height, zOffset + transform.position.y);
        return position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, FlatSpawnExtents);
    }
}
