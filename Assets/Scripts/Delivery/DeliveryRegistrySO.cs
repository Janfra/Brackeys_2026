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

    private List<PackageDetailsSO> deliveryOrders;
    public IReadOnlyList<PackageDetailsSO> DeliveryOrders => deliveryOrders;

    private void OnEnable()
    {
        deliveryOrders = new List<PackageDetailsSO>();
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
        }
    }

    private void AssignOrderInformation(PackageDetailsSO newOrder)
    {
        int randomIndex = Random.Range(0, houseRegistry.RegisteredHouses.Count);
        newOrder.DeliveryHouse = houseRegistry.RegisteredHouses[randomIndex];
    }
}
