using Janito.Animations;
using UnityEngine;
using UnityEngine.Events;

public class DeliveryNoteButton : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimatorParameterHasher onShowParameter;

    public UnityAction OnClicked;

    public void Show()
    {
        animator.SetBool(onShowParameter, true);
    }

    public void Hide()
    {
        animator.SetBool(onShowParameter, false);
    }

    public void NotifyOfClicked()
    {
        Hide();
        OnClicked?.Invoke();
    }
}
