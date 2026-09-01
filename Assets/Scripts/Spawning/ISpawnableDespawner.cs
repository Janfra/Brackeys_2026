using UnityEngine;

public interface ISpawnableDespawner
{
    public void Despawn(GameObject obj);
}

public interface ISpawnableDespawner<T>
{
    public void Despawn(T obj);
}