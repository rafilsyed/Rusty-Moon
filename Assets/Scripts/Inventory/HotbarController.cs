using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    [SerializeField] private StaticInventoryDisplay hotbarDisplay;

    private void Update()
    {
        if (Cursor.visible) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(0);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(1);
        else if (Keyboard.current.digit3Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(2);
        else if (Keyboard.current.digit4Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(3);
        else if (Keyboard.current.digit5Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(4);
        else if (Keyboard.current.digit6Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(5);
        else if (Keyboard.current.digit7Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(6);
        else if (Keyboard.current.digit8Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(7);
        else if (Keyboard.current.digit9Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(8);
        else if (Keyboard.current.digit0Key.wasPressedThisFrame) hotbarDisplay.SetIndexActive(9); 
        
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
                hotbarDisplay.SetIndexActive(newIndex);
            }
        }
    }
}