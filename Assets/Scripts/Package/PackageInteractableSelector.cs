using System.Collections.Generic;

public class PackageInteractableSelector : IInteractSelector
{
    public void SetSelectedInteract(List<IInteractable> interactables)
    {
        TrySelectPackageCompatibleInteractable(interactables);
    }

    private void TrySelectPackageCompatibleInteractable(List<IInteractable> interactablesInRange)
    {
        for (int i = 0; i < interactablesInRange.Count; i++)
        {
            var interactable = interactablesInRange[i];
            if (interactable is IPackageInteractable)
            {
                if (i == 0) return;

                // Swap it to first position and let it be interacted with
                var temp = interactablesInRange[0];
                interactablesInRange[i] = temp;
                interactablesInRange[0] = interactable;
                return;
            }
        }
    }
}
