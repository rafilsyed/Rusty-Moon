using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoomerangSlot : MonoBehaviour
{
    [SerializeField] public BoomerangController boomerangController;
    [SerializeField] private Sprite boomerangIcon;
    [SerializeField] private Image slotHighlight;

    void Start()
    {
        slotHighlight.enabled = false;
    }

    public void SelectSlot(bool isSelected)
    {
        slotHighlight.enabled = isSelected;
    }

    void Update()
    {
        UpdateUI();
        
        if (slotHighlight.enabled && Mouse.current.rightButton.wasPressedThisFrame)
        {
            ThrowBoomerang();
        }
    }

    public void ThrowBoomerang()
    {
        if (boomerangController != null && !boomerangController.IsThrown)
        {
            boomerangController.ThrowBoomerang();
        }
    }

    public void UpdateUI()
    {
        Image boomerangImage = GetComponentInChildren<Image>();
        
        if (boomerangImage != null)
        {
            boomerangImage.sprite = boomerangController != null && !boomerangController.IsThrown 
                ? boomerangIcon 
                : null;
            boomerangImage.enabled = boomerangImage.sprite != null;
        }
    }
}