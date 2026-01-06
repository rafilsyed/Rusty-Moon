using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    [Header("Debug")]
    [SerializeField] private InventoryItemData debugItemToSet;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackPackDisplayRequested;

    protected override void Awake()
    {
        base.Awake();
        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            OnPlayerBackPackDisplayRequested?.Invoke(secondaryInventorySystem);
        }

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            // Vérifie si l'objet de debug est bien assigné dans l'Inspecteur
            if (debugItemToSet != null)
            {
                const int SLOT_INDEX = 0; // Le premier slot de la Hotbar
                const int QUANTITY = 64;

                // Accède au slot cible
                InventorySlot targetSlot = primaryInventorySystem.InventorySlots[SLOT_INDEX];

                // Met à jour les données du slot : VOTRE LIGNE SIMPLE
                targetSlot.UpdateInventorySlot(debugItemToSet, QUANTITY);

                // Déclenche l'événement pour mettre à jour l'UI (pour que la Hotbar affiche l'objet)
                primaryInventorySystem.OnInventorySlotChanged?.Invoke(targetSlot);

                Debug.Log($"[CHEAT] 64x {debugItemToSet.DisplayName} forcé dans le Slot 1.");
            }
            else
            {
                Debug.LogError("Assignez un ItemData au champ 'Debug Item To Set' dans l'Inspecteur !");
            }
        }
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        else if (secondaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
