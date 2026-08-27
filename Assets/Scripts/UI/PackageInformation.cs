using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class PackageInformation : MonoBehaviour
{
    [SerializeField]
    private RegisteredHousesSO houseRegistry;
    [SerializeField]
    private DeliveryRegistrySO deliveryRegistry;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        var packageDetails = deliveryRegistry.DeliveryOrders[0];

        houseRegistry.TryGetHouseDetails(packageDetails.DeliveryHouse, out HouseDetails details);
        text.text = $"{details.DisplayName}";
    }
}
