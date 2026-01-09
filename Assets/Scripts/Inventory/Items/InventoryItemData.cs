using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize; 
    public GameObject itemPrefab;
    public float handDisplayScale = 1f;

    public virtual bool UseItem()
    {
        return true; 
    }

    public virtual bool Attack()
    {
        return true;
    }
}
