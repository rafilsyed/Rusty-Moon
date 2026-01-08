using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Food Item")]
public class FoodItem : InventoryItemData
{
    [Header("Informations")]
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

        Object.Destroy(temp, eatSound.length);
        return true;
    }
}