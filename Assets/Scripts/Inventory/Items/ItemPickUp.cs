using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData ItemData;

    [Header("Son")]
    public AudioClip pickUpSound;  // Son à jouer lors du ramassage

    private SphereCollider myCollider;
    private AudioSource audioSource;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;

        // Ajouter ou récupérer un AudioSource
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f; // 3D
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 10f;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        if (!inventory) return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            // Jouer le son avant de détruire
            if (pickUpSound != null)
            {
                audioSource.PlayOneShot(pickUpSound);
                // Détacher l'objet du parent pour qu'il continue à jouer le son même après Destroy
                transform.parent = null;
                Destroy(gameObject, pickUpSound.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
