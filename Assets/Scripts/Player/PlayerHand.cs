using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private StaticInventoryDisplay staticInventoryDisplay;
    private GameObject currentItem;

    void Start()
    {
        InvokeRepeating(nameof(UpdateItemDisplay), 0f, 0.1f);
    }

    private void UpdateItemDisplay()
    {
        if (staticInventoryDisplay != null)
        {
            InventoryItemData currentItemData = staticInventoryDisplay.GetCurrentItem();
            ShowItemInHand(currentItemData);
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

        Debug.Log("show item in hand");

        if (itemData != null && itemData.itemPrefab != null)
        {
            currentItem = Instantiate(itemData.itemPrefab, transform);
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;
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
}