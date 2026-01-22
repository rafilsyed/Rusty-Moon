using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{
    [SerializeField] private PlayerInventoryHolder playerInventory;
    [SerializeField] private AudioClip chestOpenSound;
    [SerializeField] private AudioClip chestClosedSound;

    private static GameObject chestOpen;
    private static GameObject chestClosed;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
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
        AudioSource.PlayClipAtPoint(chestOpenSound, transform.position, 2f);
        chestClosed.SetActive(false);
        chestOpen.SetActive(true);
    }

    public void CloseChestUI()
    {
        AudioSource.PlayClipAtPoint(chestClosedSound, transform.position, 2f);
        chestClosed.SetActive(true);
        chestOpen.SetActive(false);
    }

    public void EndInteraction()
    {
        CloseChestUI();
    }
}
