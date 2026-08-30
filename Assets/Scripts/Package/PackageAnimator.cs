using Janito.Animations;
using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PackageAnimator : MonoBehaviour
{
    [SerializeField]
    private float minSpeedForDust;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private AnimatorParameterHasher appearParameter;
    [SerializeField]
    private ParticleSystem dustCloud;

    private Animator animator;

    private float minSqrSpeedForDust;
    private List<ContactPoint> contactPoints = new();

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
    }

    private void OnEnable()
    {
        animator.SetTrigger(appearParameter);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.linearVelocity.sqrMagnitude > minSqrSpeedForDust)
        {
            var contactCount = collision.GetContacts(contactPoints);
            GetMiddlePoint(out Vector3 middlePoint);
            dustCloud.transform.position = middlePoint;
            dustCloud.Play();
        }
    }

    private void GetMiddlePoint(out Vector3 middlePoint)
    {
        if (contactPoints.Count == 1)
        {
            middlePoint = contactPoints[0].point;
            return;
        }

        Vector3 sum = Vector3.zero;
        foreach (var contact in contactPoints)
        {
            sum += contact.point;
        }

        middlePoint = sum / contactPoints.Count;
    }
}
