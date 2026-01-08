using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;
    [SerializeField] private PlayerHand playerHand;

    private int currentSelectedSlotIndex = -1;

    public InventoryItemData GetCurrentItem()
    {
        if (currentSelectedSlotIndex < 0 || currentSelectedSlotIndex >= slots.Length)
        {
            return null;
        }

        InventorySlot selectedInventorySlot = slots[currentSelectedSlotIndex].AssignedInventorySlot;
        return selectedInventorySlot.ItemData;
    }

    public InventorySlot GetCurrentInventorySlot()
    {
        if (currentSelectedSlotIndex < 0 || currentSelectedSlotIndex >= slots.Length)
        {
            return null;
        }

        return slots[currentSelectedSlotIndex].AssignedInventorySlot;
    }

    public int GetCurrentIndex()
    {
        return currentSelectedSlotIndex;
    }

    public int GetMaxSlots()
    {
        return slots.Length;
    }

    protected override void Start()
    {
        base.Start();

        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }

        AssignSlot(inventorySystem);

        SetIndexActive(0);
    }

    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictonary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (slots.Length != inventorySystem.InventorySize)
            Debug.LogWarning("Inventory size mismatch!");

        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictonary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }

    public void SetIndexActive(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        if (currentSelectedSlotIndex == index) return;

        if (currentSelectedSlotIndex != -1)
            slots[currentSelectedSlotIndex].ToggleHighlight(false);

        currentSelectedSlotIndex = index;
        slots[currentSelectedSlotIndex].ToggleHighlight(true);

        if (playerHand != null)
        {
            if (GetCurrentItem() != null && GetCurrentItem().itemPrefab != null)
                playerHand.ShowItemInHand(GetCurrentItem());
            else
                playerHand.ShowItemInHand(null);
        }
    }
}