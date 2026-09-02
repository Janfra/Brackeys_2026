using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Delivery Registry", menuName = "Scriptable Objects/Delivery/Delivery Registry")]
public class DeliveryRegistrySO : ScriptableObject, IDeliveryManager
{
    public event UnityAction<DeliveryDetailsSO> OnNewOrderRegistered;
    public event UnityAction<DeliveryDetailsSO, DeliveryResult> OnOrderRemoved;

    [SerializeField]
    private RegisteredHousesSO houseRegistry;

    [SerializeField]
    [Min(1.0f)]
    private float expirationTime;

    private const float destroyDelay = 1.0f;

    private List<DeliveryDetailsSO> deliveryOrders;
    public IReadOnlyList<DeliveryDetailsSO> DeliveryOrders => deliveryOrders;

    private void OnEnable()
    {
        deliveryOrders = new();
    }

    private void OnDisable()
    {
        deliveryOrders = new();
    }

    public DeliveryDetailsSO GetNewDeliveryOrder()
    {
        var newOrder = CreateInstance<DeliveryDetailsSO>();
        AssignOrderInformation(newOrder);
        deliveryOrders.Add(newOrder);
        OnNewOrderRegistered?.Invoke(newOrder);
        return newOrder;
    }

    public void RemoveDeliveryOrder(DeliveryDetailsSO packageDetails, DeliveryResult result)
    {
        if (deliveryOrders.Remove(packageDetails))
        {
            OnOrderRemoved?.Invoke(packageDetails, result);
            packageDetails.Dispose();
            Destroy(packageDetails, destroyDelay); // No pooling, just destroy the object once disposed
        }
    }

    private void AssignOrderInformation(DeliveryDetailsSO newOrder)
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
