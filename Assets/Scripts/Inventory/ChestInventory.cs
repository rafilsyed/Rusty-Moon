using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{   
    [SerializeField] private PlayerInventoryHolder playerInventory;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem);
        PlayerInventoryHolder.OnPlayerBackPackDisplayRequested?.Invoke(playerInventory.SecondaryInventorySystem);
        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        
    }
}
