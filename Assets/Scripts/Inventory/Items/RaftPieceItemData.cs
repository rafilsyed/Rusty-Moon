using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Construction/Raft Piece")]
public class RaftPieceItemData : InventoryItemData
{
    public override bool UseItem()
    {
        PlayerBuilder builder = FindAnyObjectByType<PlayerBuilder>(); 
        
        if (builder == null)
        {
            Debug.LogError("RaftPieceItemData: Le PlayerBuilder n'est pas trouvé dans la scène.");
            return false;
        }

        return builder.RegarderEtConstruire(); 
    }
}