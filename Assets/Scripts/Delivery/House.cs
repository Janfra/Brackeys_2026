using Janito.EditorExtras;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    private HouseSO houseIdentifier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Package package))
        {
            if (package.DeliveryDetails == null)
            {
                LogLibrary.LogErrorInDevelopment<Package>($"Package delivery details are null.", package);
                return;
            }

            if (IsForThisAddress(package.DeliveryDetails))
            {
                package.Deliver();
            }
        }  
    }

    public bool IsForThisAddress(PackageDetailsSO packageDetails)
    {
        return packageDetails.DeliveryHouse == houseIdentifier;
    }
}
