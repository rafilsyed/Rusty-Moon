using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;

    protected override void Start()
    {
        base.Start();

        if(inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning("No inventory holder assigned to StaticInventoryDisplay on " + gameObject.name);

        AssignSlot(inventorySystem);
    }

    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictonary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if(slots.Length != inventorySystem.InventorySize)
        {
            Debug.LogWarning("Inventory size and UI slot size do not match on " + gameObject.name);
        }

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictonary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}
