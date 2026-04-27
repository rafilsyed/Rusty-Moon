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
       // On récupère le joueur (on suppose qu'il a le tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        // Jouer le son de lancer
        PlayCastSound();

        // Logique de lancer (exemple simplifié)
        Debug.Log("Lancement de la ligne avec une puissance de " + castPower);
        
        // Ici, tu pourrais instancier ton flotteur devant le joueur

        return true;
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