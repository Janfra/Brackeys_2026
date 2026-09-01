public interface ISpawnable
{
    public ISpawnableDespawner Despawner { get; set; }
}


public interface ISpawnable<T>
{
    public ISpawnableDespawner<T> Despawner { get; set; }
}