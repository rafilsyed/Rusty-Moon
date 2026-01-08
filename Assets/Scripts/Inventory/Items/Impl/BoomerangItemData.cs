using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Boomerang Item")]
public class BoomerangItemData : InventoryItemData
{
    public override bool UseItem()
    {
        Debug.Log("Boomerang thrown!");
        // Implementation for throwing the boomerang would go here.
        return true;
    }
}