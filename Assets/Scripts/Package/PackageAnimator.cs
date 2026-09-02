using Janito.Animations;
using Janito.EditorExtras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Package))]
public class PackageAnimator : MonoBehaviour
{
    [SerializeField]
    private float minSpeedForDust;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private AnimatorParameterHasher appearParameter;
    [SerializeField]
    private AnimatorParameterHasher disappearParameter;
    [SerializeField]
    private AnimationClip deliverClip;
    [SerializeField]
    private ParticleSystem dustCloud;

    private Animator animator;
    private Package package;

    private float minSqrSpeedForDust;
    private List<ContactPoint> contactPoints = new();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        package = GetComponent<Package>();

        if (rb == null)
        {
            if (!TryGetComponent(out rb))
            {
                this.LogErrorInDevelopment($"Unable to find Rigidbody to determine which effects and animations to play. Please assign it.");
            }
        }

        if (appearParameter == null)
        {
            this.LogErrorInDevelopment($"{nameof(AnimatorParameterHasher)} reference is missing, unable to update animations information. Please assign it.");
        }

        if (dustCloud == null)
        {
            if (!TryGetComponent(out dustCloud))
            {
                dustCloud = GetComponentInChildren<ParticleSystem>();
                if (dustCloud == null)
                {
                    this.LogErrorInDevelopment($"Unable to find Particle System to play dust particle effect. Please assign it.");
                }
            }
        }

        minSqrSpeedForDust = minSpeedForDust * minSpeedForDust;

        package.OnDelivered += PlayDeliverAnimation;
    }

    private void OnEnable()
    {
        animator.SetTrigger(appearParameter);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.linearVelocity.sqrMagnitude > minSqrSpeedForDust)
        {
            if (collision.TryGetMiddlePoint(contactPoints, out Vector3 middlePoint))
            {
                dustCloud.transform.position = middlePoint;
                dustCloud.Play();
            }
        }
    }

    private void PlayDeliverAnimation(DeliveryResult result)
    {
        rb.isKinematic = true;
        animator.SetTrigger(disappearParameter);
        StartCoroutine(DespawnAfterAnimation());
    }

    private IEnumerator DespawnAfterAnimation()
    {
        yield return new WaitForSeconds(deliverClip.averageDuration);
        rb.isKinematic = false;
        package.Despawn();
    }
}
