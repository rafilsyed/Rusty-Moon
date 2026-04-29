using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Fishing Rod Item")]
public class FishingRodItemData : InventoryItemData
{
    [Header("Sound Effects")]
    public AudioClip castSound;
    public float volume = 1f;

    [Header("Fishing Settings")]
    public float castPower = 10f;
    public GameObject floatPrefab; // modèle 3D du flotteur

    public override bool UseItem()
    {
        return false; 
    }

    private void PlayCastSound()
    {
        if (castSound == null) return;

        GameObject temp = new GameObject("FishingSoundTemp");
        AudioSource source = temp.AddComponent<AudioSource>();

        source.clip = castSound;
        source.volume = volume;
        source.Play();

        Destroy(temp, castSound.length);
    }
}