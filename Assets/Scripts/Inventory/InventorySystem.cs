using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
        }

        return false;
    }

    public bool RemoveFromInventory(InventoryItemData itemToRemove, int amountToRemove)
    {
        if (!HasItem(itemToRemove, amountToRemove, out _)) return false;
        int remainingToRemove = amountToRemove;
        var slots = inventorySlots.Where(i => i.ItemData == itemToRemove).ToList();

        foreach (var slot in slots)
        {
            if (slot.StackSize >= remainingToRemove)
            {
                slot.RemoveFromStack(remainingToRemove);
                if (slot.StackSize == 0) slot.ClearSlot();

                OnInventorySlotChanged?.Invoke(slot);
                return true;
            }
            else
            {
                remainingToRemove -= slot.StackSize;
                slot.ClearSlot();
                OnInventorySlotChanged?.Invoke(slot);
            }
        }

        return true;
    }

    public bool HasItem(InventoryItemData itemToCheck, int totalAmountRequired, out int totalFound)
    {
        var slots = inventorySlots.Where(i => i.ItemData == itemToCheck).ToList();
        totalFound = slots.Sum(s => s.StackSize);
        return totalFound >= totalAmountRequired;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        return invSlot.Count > 0;
    }


    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot != null;
    }
}