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
    [SerializeField]
    private AnimatorParameterHasher isHighlight;

    [Header("Debug")]
    [SerializeField]
    [ReadOnly]
    private DeliveryDetailsSO packageDetails;

    public DeliveryDetailsSO PackageDetails => packageDetails;

    public ISpawnableDespawner<PackageDisplay> Despawner { get; set; }

    private Animator animator;
    private GrabInformation grabInformation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetTrigger(appearParameter);
    }

    private void OnDisable()
    {
        TryClearGrabListener();
    }

    private void LateUpdate()
    {
        if (PackageDetails != null)
        {
            ExpirationSlider.value = PackageDetails.ExpirationTimer.InversedNormalisedTime;
        }
    }

    public void AssignPackage(DeliveryDetailsSO assignedPackageDetails)
    {
        packageDetails = assignedPackageDetails;
    }

    public void OnPackageDelivered(bool isSuccess)
    {
        animator.SetBool(isSuccessParameter, isSuccess);
        animator.SetTrigger(despawnParameter);
    }

    public void OnHeld(GrabInformation grabInformation)
    {
        TryClearGrabListener();
        this.grabInformation = grabInformation;
        grabInformation.ReleaseNotifier.OnReleased += EndHighlight;
        Highlight();
    }

    private void Highlight()
    {
        animator.SetBool(isHighlight, true);
    }

    private void EndHighlight()
    {
        animator.SetBool(isHighlight, false);
        TryClearGrabListener();
    }

    public void OnDespawnAnimationComplete()
    {
        Despawner?.Despawn(this);
    }

    private void TryClearGrabListener()
    {
        if (grabInformation != null)
        {
            grabInformation.ReleaseNotifier.OnReleased -= EndHighlight;
            grabInformation = null;
        }
    }
}
