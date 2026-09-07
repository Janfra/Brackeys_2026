using Janito.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DeliveryNoteDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text deliveryNoteText;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimatorParameterHasher onShowParameter;

    public UnityAction OnClosed;

    public void SetDescription(string description)
    {
        deliveryNoteText.text = description;
    }

    public void Show()
    {
        animator.SetBool(onShowParameter, true);
    }

    public void Hide()
    {
        animator.SetBool(onShowParameter, false);
    }
    
    public void NotifyOfClosed()
    {
        Hide();
        OnClosed?.Invoke();
    }
}
