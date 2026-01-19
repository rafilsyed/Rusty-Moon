using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    [Header("Debug")]
    [SerializeField] private InventoryItemData debugItemToSet;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackPackDisplayRequested;

    protected override void Awake()
    {
        base.Awake();
        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && !Interactor.IsInteracting)
        {
            OnPlayerBackPackDisplayRequested?.Invoke(secondaryInventorySystem);
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (debugItemToSet != null)
            {
                const int SLOT_INDEX = 0;

                InventorySlot targetSlot = primaryInventorySystem.InventorySlots[SLOT_INDEX];
                targetSlot.UpdateInventorySlot(debugItemToSet, debugItemToSet.MaxStackSize);
                primaryInventorySystem.OnInventorySlotChanged?.Invoke(targetSlot);
            }
        }
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        
        else if (secondaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
