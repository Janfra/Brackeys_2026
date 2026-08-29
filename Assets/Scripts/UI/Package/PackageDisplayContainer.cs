using System;
using System.Collections.Generic;
using UnityEngine;

public class PackageDisplayContainer : MonoBehaviour
{
    [SerializeField]
    private DeliveryRegistrySO deliveryRegistry;
    [SerializeField]
    private PackageDisplay prefab;
    [SerializeField]
    private SpawningConfigurationSO packagesSpawningInfo;

    private Queue<PackageDisplay> availablePackages = new();
    private Dictionary<PackageDetailsSO, PackageDisplay> packageDetailsDisplayMap = new();

    private void Awake()
    {
        for (int i = 0; i < packagesSpawningInfo.MaxSpawnCount; i++)
        {
            CreatePackageDisplay();
        }

        deliveryRegistry.OnNewOrderRegistered += DisplayPackageInformation; 
        deliveryRegistry.OnOrderRemoved += FreeAssignedPackageDisplay;
    }

    private void DisplayPackageInformation(PackageDetailsSO packageDetails)
    {
        var display = ReservePackageDisplay();
        display.AssignPackage(packageDetails);
        if (packageDetailsDisplayMap.TryAdd(packageDetails, display))
        {
            packageDetailsDisplayMap[packageDetails] = display;
        }
    }

    private void FreeAssignedPackageDisplay(PackageDetailsSO packageDetails)
    {
        if (packageDetailsDisplayMap.TryGetValue(packageDetails, out var Display))
        {
            if (Display != null)
            {
                FreePackageDisplay(Display);
                packageDetailsDisplayMap[packageDetails] = null;
            }
        }
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
        instance.gameObject.SetActive(false);
        availablePackages.Enqueue(instance);
    }
}
