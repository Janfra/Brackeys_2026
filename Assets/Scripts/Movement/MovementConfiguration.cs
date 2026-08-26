using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Configuration", menuName = "Scriptable Objects/Configurations/Movement")]
public class MovementConfiguration : ScriptableObject
{
    [field: SerializeField]
    public float Speed { get; private set; } = 12.0f;

    [field: SerializeField]
    public RotationStats RotationStats { get; private set; } = new RotationStats(200.0f, 170.0f);

    public float RotationStep => RotationStats.RotationStep;
    public float RotationSpeed => RotationStats.RotationSpeed;

    private void OnValidate()
    {
        RotationStats = new RotationStats(Mathf.Max(0, RotationStats.RotationSpeed), RotationStep);
        Speed = Mathf.Max(0, Speed);
    }
}
