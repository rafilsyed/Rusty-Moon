using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Hammer Item")]
public class HammerItemData : InventoryItemData
{
    public override bool UseItem()
    {
        Debug.Log("Hammer used!");
        // Implementation for using the hammer would go here.
        return true;
    }
}