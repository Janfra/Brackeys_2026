using Janito.EditorExtras;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PackageDisplayContainer : MonoBehaviour, ISpawnableDespawner<PackageDisplay>
{
    [SerializeField]
    private DeliveryRegistrySO deliveryRegistry;
    [SerializeField]
    private SpawningConfigurationSO packagesSpawningInfo;
    [SerializeField]
    private ComponentPooler<PackageDisplay> packageDisplayPooler;
    [SerializeField]
    [InlineInspector]
    private GrabTrackerSO playerGrabTracker;

    private Dictionary<DeliveryDetailsSO, PackageDisplay> packageDetailsDisplayMap = new();

    private void Awake()
    {
        packageDisplayPooler.Initialize(packagesSpawningInfo.MaxSpawnCount, this);
    }

    private void OnEnable()
    {
        deliveryRegistry.OnNewOrderRegistered += DisplayPackageInformation;
        deliveryRegistry.OnOrderRemoved += FreeAssignedPackageDisplay;
        playerGrabTracker.OnNewGrabbed += TryUpdateHeldPackage;
    }

    private void OnDisable()
    {
        deliveryRegistry.OnNewOrderRegistered -= DisplayPackageInformation;
        deliveryRegistry.OnOrderRemoved -= FreeAssignedPackageDisplay;
        playerGrabTracker.OnNewGrabbed -= TryUpdateHeldPackage;
    }

    public void Despawn(PackageDisplay package)
    {
        if (package == null) return;
        packageDisplayPooler.ReleaseComponent(package);
    }

    private void TryUpdateHeldPackage(GrabInformation grabInformation)
    {
        if (grabInformation == null || !grabInformation.IsValid)
        {
            return;
        }

        if (grabInformation.GrabbedObject.TryGetComponent(out IDeliveryDetailsHolder deliveryDetailsHolder))
        {
            if (packageDetailsDisplayMap.TryGetValue(deliveryDetailsHolder.DeliveryDetails, out var packageDisplay))
            {
                if (packageDisplay != null)
                {
                    packageDisplay.OnHeld(grabInformation);
                }
            }
        }
    }

    private void DisplayPackageInformation(DeliveryDetailsSO packageDetails)
    {
        if (packageDetails == null) return;

        var display = packageDisplayPooler.GetComponent();
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
