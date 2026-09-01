using Janito.Animations;
using Janito.EditorExtras;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class PackageDisplay : MonoBehaviour, ISpawnable<PackageDisplay>
{
    [field: SerializeField]
    public Slider ExpirationSlider { get; private set; }

    [SerializeField]
    private AnimatorParameterHasher appearParameter;
    [SerializeField]
    private AnimatorParameterHasher despawnParameter;
    [SerializeField]
    private AnimatorParameterHasher isSuccessParameter;

    [Header("Debug")]
    [SerializeField]
    [ReadOnly]
    private PackageDetailsSO packageDetails;

    public PackageDetailsSO PackageDetails => packageDetails;

    public ISpawnableDespawner<PackageDisplay> Despawner { get; set; }

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

    public void OnPackageDelivered(bool isSuccess)
    {
        animator.SetBool(isSuccessParameter, isSuccess);
        animator.SetTrigger(despawnParameter);
    }

    public void OnDespawnAnimationComplete()
    {
        Despawner?.Despawn(this);
    }

    private void OnEnable()
    {
        animator.SetTrigger(appearParameter);
    }
}
