using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Fibre Item")]
public class FiberItemData : InventoryItemData
{


    public override bool UseItem()
    {
        return false;
    }


}
