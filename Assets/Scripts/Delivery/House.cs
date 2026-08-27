using Janito.EditorExtras;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class House : MonoBehaviour
{
    [SerializeField]
    private RegisteredHousesSO houseRegistry;

    [SerializeField]
    private Transform deliveryPoint;

    [Header("Debug")]
    [ReadOnly]
    [SerializeField]
    private HouseSO houseIdentifier;

    private void Awake()
    {
        if (houseRegistry == null)
        {
            this.LogErrorInDevelopment($"House house registry reference is null. Provide house registry reference to be able to register and find house information.");
            return;
        }

        if (deliveryPoint == null)
        {
            this.LogErrorInDevelopment($"House delivery point reference is null. Provide transform reference to be able to determine delivery location.");
            return;
        }

        houseIdentifier = houseRegistry.CreateAndRegisterHouse(new(deliveryPoint.position, null));

        // Make the house static just in case
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnDisable()
    {
        houseRegistry.UnregisterHouse(houseIdentifier);
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
