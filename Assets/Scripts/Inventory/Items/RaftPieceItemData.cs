using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Construction/Raft Piece")]
public class RaftPieceItemData : InventoryItemData
{
    public override bool UseItem()
    {
        PlayerBuilder builder = FindAnyObjectByType<PlayerBuilder>(); 
        
        if (builder == null)
        {
            return false;
        }

        return builder.RegarderEtConstruire(); 
    }
}