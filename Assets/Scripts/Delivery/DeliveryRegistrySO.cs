using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Delivery Registry", menuName = "Scriptable Objects/Delivery/Delivery Registry")]
public class DeliveryRegistrySO : ScriptableObject
{
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
        return newOrder;
    }

    public void RemoveDeliveryOrder(PackageDetailsSO packageDetails)
    {
        deliveryOrders.Remove(packageDetails);
    }

    private void AssignOrderInformation(PackageDetailsSO newOrder)
    {
        int randomIndex = Random.Range(0, houseRegistry.RegisteredHouses.Count);
        newOrder.DeliveryHouse = houseRegistry.RegisteredHouses[randomIndex];
    }
}
