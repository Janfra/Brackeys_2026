using Janito.EditorExtras;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Timer : IReadOnlyTimer, IDisposable
{
    public event UnityAction OnCompleted;

    [field: SerializeField]
    [Min(0.0f)]
    public float Duration { get; set; }
    [field: SerializeField]
    public bool IsLooping { get; set; }
    [field: SerializeField]
    public bool IsRunning { get; set; }

    [field: SerializeField]
    [field: Space]
    [field: ReadOnly]
    public float ElapsedTime { get; private set; }

    public float NormalisedTime => Mathf.Min(ElapsedTime, Duration) / Duration;
    public float InversedNormalisedTime => 1 - NormalisedTime;

    public Timer(float duration)
    {
        Duration = duration;
    }

    public Timer(float duration, UnityAction onComplete)
    {
        Duration = duration;
        OnCompleted = onComplete;
    }

    public Timer(float duration, bool isLooping, UnityAction onComplete)
    {
        Duration = duration;
        IsLooping = isLooping;
        OnCompleted = onComplete;
    }

    public void Reset()
    {
        ElapsedTime = 0;
    }

    public void Update(float deltaTime)
    {
        if (!IsRunning)
        {
            return;
        }

        ElapsedTime += deltaTime;
        if (ElapsedTime >= Duration)
        {
            Ended();
        }
    }

    public void Dispose()
    {
        OnCompleted = null;
    }

    private void Ended()
    {
        if (!IsLooping)
        {
            IsRunning = false;
        }

        ElapsedTime = 0.0f;
        OnCompleted?.Invoke();
    }
}
