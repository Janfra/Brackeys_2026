using UnityEngine;

[CreateAssetMenu(fileName = "New Grab Configuration")]
public class GrabConfiguration : ScriptableObject
{
    [field: SerializeField]
    [Tooltip("Optional height offset to be applied when object is picked up")]
    public float HeightOffset = 0.5f;

    [field: SerializeField]
    public DirectionalThrowForce ThrowForce { get; private set; }
}
