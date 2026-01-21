using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{   
    [SerializeField] private PlayerInventoryHolder playerInventory;
    [SerializeField] private AudioClip chestOpenSound;

    private GameObject chestOpen;
    private GameObject chestClosed;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        // Assumes:
        // Child 0 = closed chest
        // Child 1 = open chest
        chestClosed = transform.GetChild(0).gameObject;
        chestOpen = transform.GetChild(1).gameObject;

        CloseChestUI();
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem);
        PlayerInventoryHolder.OnPlayerBackPackDisplayRequested
            ?.Invoke(playerInventory.SecondaryInventorySystem);

        interactSuccessful = true;
        OpenChestUI();
    }

    public void OpenChestUI()
    {
        AudioSource.PlayClipAtPoint(chestOpenSound, transform.position);
        chestClosed.SetActive(false);
        chestOpen.SetActive(true);
    }

    public void CloseChestUI()
    {
        chestClosed.SetActive(true);
        chestOpen.SetActive(false);
    }

    public void EndInteraction()
    {
        // Assuming 'this' refers to the current instance of ChestInventory
        this.CloseChestUI();
    }
}
