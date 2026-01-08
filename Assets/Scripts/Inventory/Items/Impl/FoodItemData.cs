using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Food Item Data")]
public class FoodItemData : InventoryItemData
{
    [Header("Utils References")]
    [SerializeField] private float foodValue;
    [Header("Sound Effects")]
    public AudioClip eatSound;
    public float volume = 1f;
 
    public override bool UseItem()
    {
        if (eatSound == null)
        {
            Debug.LogWarning("Eat sound is not assigned.");
            return true;
        }

        GameObject temp = new GameObject("EatSound");
        AudioSource source = temp.AddComponent<AudioSource>();

        source.clip = eatSound;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.Play();

        Destroy(temp, eatSound.length);

        PlayerFoodLevel playerFoodLevel = FindAnyObjectByType<PlayerFoodLevel>();
        if (playerFoodLevel != null)
        {
            playerFoodLevel.EatFood(foodValue);
        }
        
        return true;
    }
}