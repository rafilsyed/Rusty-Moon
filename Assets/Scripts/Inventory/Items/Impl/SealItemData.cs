using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Seal Item")]
public class SealItemData : InventoryItemData
{
    public override bool UseItem()
    {
        return true;
    }
}
