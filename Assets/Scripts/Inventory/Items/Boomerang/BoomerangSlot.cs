using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoomerangSlot : MonoBehaviour
{
    [SerializeField] public BoomerangController boomerangController;
    [SerializeField] private Sprite boomerangIcon;

    void Update()
    {
        UpdateUI();

        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            ThrowBoomerang();
        }
    }

    public void ThrowBoomerang()
    {
        if(boomerangController != null && !boomerangController.IsThrown)
        {
            boomerangController.ThrowBoomerang();
        }
    }
    
    public void UpdateUI(){
        Image boomerangImage = GetComponent<Image>();
        if (boomerangImage != null)
        {
            boomerangImage.sprite = boomerangController != null && !boomerangController.IsThrown ? boomerangIcon : null;
            boomerangImage.enabled = boomerangImage.sprite != null;
        }
    }
}