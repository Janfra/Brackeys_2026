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
            ProcessDeliveredPackage(package);
        }
        else if (other.TryGetComponent(out Interactor interactor))
        {
            // In case we collide with the interactor holding the package instead of the package itself
            package = interactor.GetComponentInChildren<Package>();
            if (package)
            {
                ProcessDeliveredPackage(package);
            }
        }
    }

    public void ProcessDeliveredPackage(Package package)
    {
        if (package.DeliveryDetails == null)
        {
            LogLibrary.LogErrorInDevelopment<Package>($"Package delivery details are null.", package);
            return;
        }

        bool isCorrect = IsForThisAddress(package.DeliveryDetails);
        package.Deliver(isCorrect);
    }

    private bool IsForThisAddress(PackageDetailsSO packageDetails)
    {
        return packageDetails.DeliveryHouse == houseIdentifier;
    }
}
