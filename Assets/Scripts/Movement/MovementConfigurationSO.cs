using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Configuration", menuName = "Scriptable Objects/Configurations/Movement")]
public class MovementConfigurationSO : ScriptableObject
{
    [field: SerializeField]
    public SpeedStats SpeedStats { get; private set; } = new SpeedStats(12.0f, 10.0f, 11.0f);

    [field: SerializeField]
    public RotationStats RotationStats { get; private set; } = new RotationStats(200.0f, 170.0f);

    public float MaxSpeed => SpeedStats.MaxSpeed;
    public float AccelerationRate => SpeedStats.AccelerationRate;
    public float DecelerationRate => SpeedStats.DecelerationRate;
    public float RotationStep => RotationStats.RotationStep;
    public float RotationSpeed => RotationStats.RotationSpeed;
}
