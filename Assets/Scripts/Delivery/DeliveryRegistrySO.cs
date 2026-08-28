using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Delivery Registry", menuName = "Scriptable Objects/Delivery/Delivery Registry")]
public class DeliveryRegistrySO : ScriptableObject
{
    public event UnityAction<PackageDetailsSO> OnNewOrderRegistered;
    public event UnityAction<PackageDetailsSO> OnOrderRemoved;

    [SerializeField]
    private RegisteredHousesSO houseRegistry;

    [SerializeField]
    [Min(1.0f)]
    private float expirationTime;

    private const float destroyDelay = 1.0f;

    private List<PackageDetailsSO> deliveryOrders;
    public IReadOnlyList<PackageDetailsSO> DeliveryOrders => deliveryOrders;

    private void OnEnable()
    {
        deliveryOrders = new();
    }

    private void OnDisable()
    {
        deliveryOrders = new();
    }

    public PackageDetailsSO GetNewDeliveryOrder()
    {
        var newOrder = CreateInstance<PackageDetailsSO>();
        AssignOrderInformation(newOrder);
        deliveryOrders.Add(newOrder);
        OnNewOrderRegistered?.Invoke(newOrder);
        return newOrder;
    }

    public void RemoveDeliveryOrder(PackageDetailsSO packageDetails)
    {
        if (deliveryOrders.Remove(packageDetails))
        {
            OnOrderRemoved?.Invoke(packageDetails);
            packageDetails.Dispose();
            Destroy(packageDetails, destroyDelay); // No pooling, just destroy the object once disposed
        }
    }

    private void AssignOrderInformation(PackageDetailsSO newOrder)
    {
        int count = houseRegistry.RegisteredHouses.Count;
        HouseSO house = null;
        string targetName = "Nowhere";
        if (count > 0)
        {
            int randomIndex = Random.Range(0, count);
            house = houseRegistry.RegisteredHouses[randomIndex];
            targetName = house.name;
        }

        newOrder.DeliveryHouse = house;
        newOrder.name = $"Delivery Order For {targetName}";
        newOrder.ExpirationTimer = new(expirationTime);
    }
}
