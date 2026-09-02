using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class PackageDisplayPooler
{
    [SerializeField]
    private PackageDisplay prefab;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    [Tooltip("The number of extra package displays to create in addition to the pool size. Only available through editor inspector.")]
    private int bufferSize;

    private Queue<PackageDisplay> availablePackages;
    private ISpawnableDespawner<PackageDisplay> despawner;

    public PackageDisplayPooler(PackageDisplay prefab, Transform parentTransform, ISpawnableDespawner<PackageDisplay> despawner, int poolSize)
    {
        this.prefab = prefab;
        this.parentTransform = parentTransform;
        Initialize(despawner, poolSize);
    }

    public void Initialize(ISpawnableDespawner<PackageDisplay> despawner, int poolSize)
    {
        this.despawner = despawner;
        availablePackages = new Queue<PackageDisplay>(poolSize + bufferSize);
        for (int i = 0; i < poolSize + bufferSize; i++)
        {
            CreatePackageDisplay();
        }
    }

    public PackageDisplay ReservePackageDisplay()
    {
        if (availablePackages.Count == 0)
        {
            CreatePackageDisplay();
        }
        var package = availablePackages.Dequeue();
        package.gameObject.SetActive(true);
        return package;
    }

    public void FreePackageDisplay(PackageDisplay package)
    {
        package.gameObject.SetActive(false);
        availablePackages.Enqueue(package);
    }

    private void CreatePackageDisplay()
    {
        var instance = Object.Instantiate(prefab, parentTransform);
        instance.gameObject.SetActive(false);
        instance.Despawner = despawner;
        availablePackages.Enqueue(instance);
    }
}
