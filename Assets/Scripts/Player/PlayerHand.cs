using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private StaticInventoryDisplay staticInventoryDisplay;
    private bool eating = false;

    public bool IsEating
    {
        get { return eating; }
        set { eating = value; }
    }
    private GameObject currentItem;

    private InventoryItemData currentItemDataHeld;

    void Start()
    {
        InvokeRepeating(nameof(UpdateItemDisplay), 0f, 0.1f);
    }

    private void UpdateItemDisplay()
    {
        if (staticInventoryDisplay != null)
        {
            InventoryItemData currentItemData = staticInventoryDisplay.GetCurrentItem();
            
            // NOUVEAU : On vérifie si l'objet sélectionné dans l'inventaire a changé
            if (currentItemData != currentItemDataHeld)
            {
                // Si c'est un nouvel objet, on met à jour la mémoire et on le fait apparaître
                currentItemDataHeld = currentItemData;
                ShowItemInHand(currentItemData);
            }
        }
    }

    public void ClearHand()
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }
    }

    public void ShowItemInHand(InventoryItemData itemData)
    {
        ClearHand();

        if (itemData != null && itemData.itemPrefab != null)
        {
            currentItem = Instantiate(itemData.itemPrefab, transform);
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.Euler(itemData.rotationOffsetDansLaMain);
            currentItem.transform.localScale = Vector3.one * itemData.handDisplayScale;

            Collider[] colliders = currentItem.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }

            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
    }

    public void StartEating(float duration, FoodItemData foodItem)
    {
        if (!eating)
        {
            eating = true;
            Invoke(nameof(StopEating), duration);
        }
    }

    private void StopEating()
    {
        eating = false;
    }
}