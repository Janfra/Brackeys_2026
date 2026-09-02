using System.Collections.Generic;
using UnityEngine;

public class PackageDisplayContainer : MonoBehaviour, ISpawnableDespawner<PackageDisplay>
{
    [SerializeField]
    private DeliveryRegistrySO deliveryRegistry;
    [SerializeField]
    private SpawningConfigurationSO packagesSpawningInfo;
    [SerializeField]
    private PackageDisplayPooler packageDisplayPooler;

    private Dictionary<DeliveryDetailsSO, PackageDisplay> packageDetailsDisplayMap = new();

    private void Awake()
    {
        packageDisplayPooler.Initialize(this, packagesSpawningInfo.MaxSpawnCount);
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
        packageDisplayPooler.FreePackageDisplay(package);
    }

    private void DisplayPackageInformation(DeliveryDetailsSO packageDetails)
    {
        if (packageDetails == null) return;

        var display = packageDisplayPooler.ReservePackageDisplay();
        display.AssignPackage(packageDetails);
        if (packageDetailsDisplayMap.TryAdd(packageDetails, display))
        {
            packageDetailsDisplayMap[packageDetails] = display;
        }
    }

    private void FreeAssignedPackageDisplay(DeliveryDetailsSO packageDetails, DeliveryResult result)
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
}
