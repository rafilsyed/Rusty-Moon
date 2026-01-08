using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Construction/Raft Piece")]
public class RaftPieceItemData : InventoryItemData
{
    [Header("Sound Effects")]
    public AudioClip sound;
    public float volume = 1f;

    public override bool UseItem()
    {
        PlayerBuilder builder = FindAnyObjectByType<PlayerBuilder>();

        if (builder == null)
        {
            return false;
        }

        if (builder.RegarderEtConstruire())
        {
            if (sound == null)
            {
                Debug.LogWarning("Sound is not assigned.");
                return true;
            }

            GameObject temp = new GameObject("Sound");
            AudioSource source = temp.AddComponent<AudioSource>();

            source.clip = sound;
            source.volume = volume;
            source.spatialBlend = 0f;
            source.Play();

            Object.Destroy(temp, sound.length);
            return true;
        }

        return false;
    }
}