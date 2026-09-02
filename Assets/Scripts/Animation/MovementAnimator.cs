using Janito.Animations;
using System;
using UnityEngine;

[Serializable]
public class MovementAnimator
{
    [Header("Animator References")]
    [field: SerializeField]
    public Animator Animator { get; private set; }
    public AnimatorParameterHasher SpeedParameter;
    [Tooltip("This parameter is used to control the speed of the movement animation, scaling up to their max speed. It is multiplied by the value of the Move Multiplier Curve.")]
    public AnimatorParameterHasher MoveMultiplierParameter;

    [Header("Configuration")]
    [SerializeField]
    private AnimationCurve moveMultiplierCurve;
    [SerializeField]
    private float minVelocity = 0.2f;

    public IReadOnlyMovement MovementData { get; private set; }

    public MovementAnimator(Animator animator, IReadOnlyMovement movementData, AnimatorParameterHasher velocityParameter, AnimatorParameterHasher moveMultiplierParameter, AnimationCurve moveMultiplierCurve, float minVelocity)
    {
        Animator = animator;
        SpeedParameter = velocityParameter;
        MoveMultiplierParameter = moveMultiplierParameter;
        MovementData = movementData;
        this.moveMultiplierCurve = moveMultiplierCurve;
        this.minVelocity = minVelocity;
    }

    public void Initialize(Animator animator, IReadOnlyMovement movementData)
    {
        if (animator == null)
        {
            throw new ArgumentNullException(nameof(animator), $"Animator reference cannot be null inside {nameof(MovementAnimator)}.");
        }

        Animator = animator;
        Initialize(movementData);
    }

    public void Initialize(IReadOnlyMovement movementData)
    {
        if (movementData == null)
        {
            throw new ArgumentNullException(nameof(movementData), $"Movement data reference cannot be null inside {nameof(MovementAnimator)}.");
        }

        MovementData = movementData;
    }

    public void UpdateMoveAnimation(bool isPlayedBackwards)
    {
        Animator.SetFloat(SpeedParameter, MovementData.Velocity.sqrMagnitude > minVelocity ? MovementData.Speed : 0.0f);
        var newMoveMultiplier = moveMultiplierCurve.Evaluate(MovementData.Speed / MovementData.MaxSpeed);
        // Make multiplier negative if input is moving backwards to play inverse animation to show as moving back
        Animator.SetFloat(MoveMultiplierParameter, isPlayedBackwards ? -newMoveMultiplier : newMoveMultiplier); 
    }
}
