using UnityEngine;
using System.Collections.Generic;

public class CraftRecipe : MonoBehaviour
{
    public InventoryItemData ResultItem;
    public int ResultQuantity;

    [System.Serializable]
    public struct RecipeRequirement
    {
        public InventoryItemData Item;
        public int Quantity;
    }

    public List<RecipeRequirement> Requirements;

    public PlayerInventoryHolder playerInventory;

    public void Craft(AudioClip craftSound, AudioClip cannotCraftSound)
    {
        foreach (var requirement in Requirements)
        {
            if (!playerInventory.HasItem(requirement.Item, requirement.Quantity, out _))
            {
                if (cannotCraftSound != null)
                {
                    AudioSource.PlayClipAtPoint(cannotCraftSound, Camera.main.transform.position);
                }
                return;
            }
        }

        foreach (var requirement in Requirements)
        {
            playerInventory.RemoveFromInventory(requirement.Item, requirement.Quantity);
        }

        playerInventory.AddToInventory(ResultItem, ResultQuantity);
        if (craftSound != null)
        {
            AudioSource.PlayClipAtPoint(craftSound, Camera.main.transform.position);
        }
    }
}