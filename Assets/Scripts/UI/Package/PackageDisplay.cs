using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.UI;

public class PackageDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Slider ExpirationSlider { get; private set; }

    [SerializeField]
    [ReadOnly]
    private PackageDetailsSO packageDetails;

    public PackageDetailsSO PackageDetails => packageDetails;

    private void LateUpdate()
    {
        if (PackageDetails != null)
        {
            ExpirationSlider.value = PackageDetails.ExpirationTimer.InversedNormalisedTime;
        }
    }

    public void AssignPackage(PackageDetailsSO assignedPackageDetails)
    {
        packageDetails = assignedPackageDetails;
    }
}
