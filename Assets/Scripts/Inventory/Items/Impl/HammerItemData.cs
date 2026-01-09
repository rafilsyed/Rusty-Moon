using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Hammer Item")]
public class HammerItemData : InventoryItemData
{
    [Header("Sound Effects")]
    public AudioClip soundEffect;
    public float volume = 1f;

    [Header("Hit Settings")]
    public float range = 2f;

    public override bool UseItem()
    {
        return false;
    }

    public override bool Attack()
    {
        if (soundEffect == null) return false;

        GameObject temp = new GameObject("SoundEffectTemp");
        AudioSource source = temp.AddComponent<AudioSource>();

        source.clip = soundEffect;
        source.volume = volume;
        source.spatialBlend = 0f;
        source.Play();

        Destroy(temp, soundEffect.length);
        return true;
    }
}
