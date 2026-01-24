using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private StaticInventoryDisplay hotbarDisplay;
    [SerializeField] private BoomerangSlot boomerangSlot;

    private void Update()
    {
        if (Cursor.visible) return;

        Key[] hotbarKeys = new Key[]{Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5, 
        Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9, Key.Digit0};

        for (int i = 0; i < hotbarKeys.Length; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                hotbarDisplay.SetIndexActive(i);
                boomerangSlot.SelectSlot(false);
                break;
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            hotbarDisplay.SetIndexActive(-1);
            boomerangSlot.SelectSlot(true);
        }

        float scroll = Mouse.current.scroll.y.ReadValue();

        if (scroll != 0)
        {
            int currentIndex = hotbarDisplay.GetCurrentIndex();
            int maxSlots = hotbarDisplay.GetMaxSlots();
            int newIndex = currentIndex;

            if (scroll < 0)
            {
                newIndex = (currentIndex + 1) % maxSlots;
            }
            else if (scroll > 0)
            {
                newIndex = (currentIndex - 1 + maxSlots) % maxSlots;
            }

            if (newIndex != currentIndex)
            {
                boomerangSlot.SelectSlot(false);
                hotbarDisplay.SetIndexActive(newIndex);
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            InventorySlot selectedSlot = hotbarDisplay.GetCurrentInventorySlot();

            if (selectedSlot == null || selectedSlot.ItemData == null)
            {
                return;
            }

            if (selectedSlot.ItemData.UseItem())
            {
                selectedSlot.RemoveFromStack(1);

                if (selectedSlot.StackSize <= 0)
                {
                    selectedSlot.ClearSlot();
                }

                hotbarDisplay.InventorySystem.OnInventorySlotChanged?.Invoke(selectedSlot);
            }
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            InventorySlot selectedSlot = hotbarDisplay.GetCurrentInventorySlot();

            if (selectedSlot == null || selectedSlot.ItemData == null)
            {
                return;
            }

            if (selectedSlot.ItemData.Attack())
            {
                // Attack logic handled in the ItemData's Attack method
            }
        }
    }
}