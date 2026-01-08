using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData ItemData;

    [Header("Son")]
    public AudioClip pickUpSound;

    private SphereCollider myCollider;
    private AudioSource audioSource;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
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
            if (pickUpSound != null)
            {
                audioSource.PlayOneShot(pickUpSound);
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
