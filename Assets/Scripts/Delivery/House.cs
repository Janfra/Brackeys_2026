using Janito.EditorExtras;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class House : MonoBehaviour
{
    [SerializeField]
    private RegisteredHousesSO houseRegistry;

    [Header("Debug")]
    [ReadOnly]
    [SerializeField]
    private HouseSO houseIdentifier;

    private void Awake()
    {
        houseIdentifier = houseRegistry.CreateAndRegisterHouse();

        // Make the house static just in case
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Requires rigidbody in order to receive message
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
