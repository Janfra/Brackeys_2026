using System.Collections.Generic;
using UnityEngine.Events;

public interface IDeliveryManager
{
    public IReadOnlyList<DeliveryDetailsSO> DeliveryOrders { get; }

    public event UnityAction<DeliveryDetailsSO> OnNewOrderRegistered;
    public event UnityAction<DeliveryDetailsSO, DeliveryResult> OnOrderRemoved;

    public DeliveryDetailsSO GetNewDeliveryOrder();
    public void RemoveDeliveryOrder(DeliveryDetailsSO packageDetails, DeliveryResult result);
}