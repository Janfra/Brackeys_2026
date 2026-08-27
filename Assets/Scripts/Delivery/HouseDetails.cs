using System;
using UnityEngine;

[Serializable]
public struct HouseDetails
{
    public Vector3 DeliveryPoint;
    public string DisplayName;

    public HouseDetails(Vector3 deliveryPoint, string displayName)
    {
        DeliveryPoint = deliveryPoint;
        DisplayName = displayName;
    }
}
