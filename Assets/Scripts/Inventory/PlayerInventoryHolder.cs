using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;
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

    public bool RemoveFromInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.RemoveFromInventory(data, amount))
        {
            return true;
        }
        
        else if (secondaryInventorySystem.RemoveFromInventory(data, amount))
        {
            return true;
        }

        return false;
    }

    public bool HasItem(InventoryItemData data, int amount, out InventorySystem inventorySystem)
    {
        if (primaryInventorySystem.HasItem(data, amount, out _))
        {
            inventorySystem = primaryInventorySystem;
            return true;
        }
        
        else if (secondaryInventorySystem.HasItem(data, amount, out _))
        {
            inventorySystem = secondaryInventorySystem;
            return true;
        }

        inventorySystem = null;
        return false;
    }
}
