using Janito.Animations;
using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PackageDisplay : MonoBehaviour
{
    [field: SerializeField]
    public Slider ExpirationSlider { get; private set; }

    [SerializeField]
    private AnimatorParameterHasher appearParameter;

    [SerializeField]
    [ReadOnly]
    private PackageDetailsSO packageDetails;

    public PackageDetailsSO PackageDetails => packageDetails;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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

    private void OnEnable()
    {
        animator.SetTrigger(appearParameter);
    }
}
