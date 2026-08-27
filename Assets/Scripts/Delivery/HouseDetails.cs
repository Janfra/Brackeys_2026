using System;
using UnityEngine;

[Serializable]
public struct HouseDetails
{
    public Vector3 DeliveryPoint;

    public HouseDetails(Vector3 deliveryPoint)
    {
        DeliveryPoint = deliveryPoint;
    }
}
