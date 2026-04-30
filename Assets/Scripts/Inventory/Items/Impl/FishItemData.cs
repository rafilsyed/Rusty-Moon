using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Fish Item")]
public class FishItemData : InventoryItemData
{
    [SerializeField] public int soldPrice;
    
    public override bool UseItem()
    {
        return true;
    }
}
