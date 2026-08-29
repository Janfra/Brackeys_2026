using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.UI;

public class PackageDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Slider ExpirationSlider { get; private set; }

    [ReadOnly]
    public PackageDetailsSO PackageDetails;

    private void LateUpdate()
    {
        if (PackageDetails != null)
        {
            ExpirationSlider.value = PackageDetails.ExpirationTimer.NormalisedTime;
        }
    }
}
