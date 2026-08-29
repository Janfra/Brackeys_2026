using Janito.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PackageAnimator : MonoBehaviour
{
    [SerializeField]
    private AnimatorParameterHasher appearParameter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    private void OnEnable()
    {
        animator.SetTrigger(appearParameter);
    }
}
