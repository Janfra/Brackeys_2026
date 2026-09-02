using UnityEngine.Events;

public interface IGrabReleaseNotifier
{
    public event UnityAction OnReleased;
}
