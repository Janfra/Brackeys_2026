using UnityEngine.Events;

public interface IReadOnlyTimer
{
    public event UnityAction OnCompleted;
    public float Duration { get; }
    public bool IsLooping { get; }
    public bool IsRunning { get; }
    public float ElapsedTime { get; }
    public float NormalisedTime { get; }
    public float InversedNormalisedTime { get; }
}
