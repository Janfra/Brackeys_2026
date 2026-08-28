using Janito.EditorExtras;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Timer
{
    public event UnityAction OnCompleted;

    public float Duration;
    public bool IsLooping;
    public bool IsRunning;

    [field: SerializeField]
    [field: Space]
    [ReadOnly]
    public float ElapsedTime { get; private set; }

    public float NormalisedTime => Mathf.Min(ElapsedTime, Duration) / Duration;
    public float InversedNormalisedTime => 1 - NormalisedTime;

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
