using UnityEngine;

public interface IHouseDetails
{
    public Vector3 DeliveryPoint { get; }
    public string DisplayName { get; }
    public string GetDescription();
}
