using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Tools/Boomerang")]
public class BoomerangItem : InventoryItemData
{
    public override bool UseItem()
    {
        Debug.Log("Boomerang thrown!");
        return true;
    }
}