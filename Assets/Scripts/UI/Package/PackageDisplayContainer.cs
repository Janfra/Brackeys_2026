using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageDisplayContainer : MonoBehaviour, ISpawnableDespawner<PackageDisplay>
{
    [SerializeField]
    private PackageDisplay prefab;
    [SerializeField]
    private DeliveryRegistrySO deliveryRegistry;
    [SerializeField]
    private SpawningConfigurationSO packagesSpawningInfo;
    [SerializeField]
    private int bufferSize = 2;

    private Queue<PackageDisplay> availablePackages = new();
    private Dictionary<PackageDetailsSO, PackageDisplay> packageDetailsDisplayMap = new();

    private void Awake()
    {
        for (int i = 0; i < packagesSpawningInfo.MaxSpawnCount + bufferSize; i++)
        {
            CreatePackageDisplay();
        }
    }

    private void OnEnable()
    {
        deliveryRegistry.OnNewOrderRegistered += DisplayPackageInformation;
        deliveryRegistry.OnOrderRemoved += FreeAssignedPackageDisplay;
    }

    private void OnDisable()
    {
        deliveryRegistry.OnNewOrderRegistered -= DisplayPackageInformation;
        deliveryRegistry.OnOrderRemoved -= FreeAssignedPackageDisplay;
    }

    public void Despawn(PackageDisplay package)
    {
        if (package == null) return;
        FreePackageDisplay(package);
    }

    private void DisplayPackageInformation(PackageDetailsSO packageDetails)
    {
        if (packageDetails == null) return;

        var display = ReservePackageDisplay();
        display.AssignPackage(packageDetails);
        if (packageDetailsDisplayMap.TryAdd(packageDetails, display))
        {
            packageDetailsDisplayMap[packageDetails] = display;
        }
    }

    private void FreeAssignedPackageDisplay(PackageDetailsSO packageDetails, DeliveryResult result)
    {
        if (packageDetails == null) return;

        if (packageDetailsDisplayMap.TryGetValue(packageDetails, out var display))
        {
            if (display != null)
            {
                NotifyPackageDisplayOfDelivery(display, result);  
                packageDetailsDisplayMap[packageDetails] = null;
            }
        }
    }

    private void NotifyPackageDisplayOfDelivery(PackageDisplay package, DeliveryResult result)
    {
        if (package == null) return;
        package.OnPackageDelivered(result == DeliveryResult.Success);
    }

    private PackageDisplay ReservePackageDisplay()
    {
        var package = availablePackages.Dequeue();
        package.gameObject.SetActive(true);
        return package;
    }

    private void FreePackageDisplay(PackageDisplay package)
    {
        package.gameObject.SetActive(false);
        availablePackages.Enqueue(package);
    }

    private void CreatePackageDisplay()
    {
        var instance = Instantiate(prefab, transform);
        instance.Despawner = this;
        instance.gameObject.SetActive(false);
        availablePackages.Enqueue(instance);
    }
}
