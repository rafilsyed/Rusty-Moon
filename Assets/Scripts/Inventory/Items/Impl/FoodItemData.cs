using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Food Item")]
public class FoodItemData : InventoryItemData
{
    [SerializeField] private float foodValue;
    [SerializeField] private float eatDuration = 2f;
    public AudioClip eatSound;
    public float volume = 1f;

    public override bool UseItem()
    {
        PlayerHand playerHand = FindAnyObjectByType<PlayerHand>();
        if (playerHand.IsEating)
        {
            return false;
        }


        playerHand.StartEating(eatDuration, this);
        if (eatSound != null)
        {
            GameObject temp = new GameObject("EatSound");
            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = eatSound;
            source.volume = volume;
            source.Play();
            Destroy(temp, eatSound.length);
        }

        PlayerFoodLevel playerFoodLevel = FindAnyObjectByType<PlayerFoodLevel>();
        if (playerFoodLevel != null)
        {
            playerFoodLevel.EatFood(foodValue);
        }

        return true;
    }
}
