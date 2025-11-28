using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot)
    {
        if (itemData == invSlot.ItemData) AddToStack(invSlot.StackSize);
        else
        {
            itemData = invSlot.ItemData;
            stackSize = 0;
            AddToStack(invSlot.StackSize);
        }
    }

    public void UpdateInventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.MaxStackSize - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if(itemData == null || itemData != null && stackSize + amountToAdd <= itemData.MaxStackSize) return true;
        else return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {
        if (stackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(itemData, halfStack);
        return true;
    }
}
