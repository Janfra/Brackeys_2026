using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor))]
public class PlayerController : MonoBehaviour
{
    private Interactor interactor;

    private void Awake()
    {
        interactor = GetComponent<Interactor>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactor.TryInteract();
        }
    }
}
