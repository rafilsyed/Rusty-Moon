using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StylishButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Réglages des Objets")]
    public TextMeshProUGUI textComponent; 

    [Header("Configurations de Transition")]
    public float scaleMultiplier = 1.15f;
    public float rotationAmount = 3f; // L'angle de rotation "tarpin stylé"
    public float lerpSpeed = 12f;
    public Color hoverButtonColor = Color.cyan;
    public Color hoverTextColor = Color.white;

    [Header("Curseur Custom")]
    public Texture2D customCursor; // Glisse ton image de curseur ici

    private Vector3 initialScale;
    private Quaternion initialRotation;
    private Color originalButtonColor;
    private Color originalTextColor;
    
    private Image buttonImage;
    private Vector3 targetScale;
    private Quaternion targetRotation;
    private Color targetButtonColor;
    private Color targetTextColor;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        initialScale = transform.localScale;
        initialRotation = transform.localRotation;

        if (textComponent == null)
            textComponent = GetComponentInChildren<TextMeshProUGUI>();

        if (buttonImage != null)
            originalButtonColor = buttonImage.color;
        
        if (textComponent != null)
            originalTextColor = textComponent.color;

        // Init des cibles
        targetScale = initialScale;
        targetRotation = initialRotation;
        targetButtonColor = originalButtonColor;
        targetTextColor = originalTextColor;
    }

    void Update()
    {
        // Animation fluide (Lerp) pour tout le monde
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * lerpSpeed);
        
        if (buttonImage != null)
            buttonImage.color = Color.Lerp(buttonImage.color, targetButtonColor, Time.deltaTime * lerpSpeed);
            
        if (textComponent != null)
            textComponent.color = Color.Lerp(textComponent.color, targetTextColor, Time.deltaTime * lerpSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = initialScale * scaleMultiplier;
        targetRotation = Quaternion.Euler(0, 0, rotationAmount); // Ça tourne !
        targetButtonColor = hoverButtonColor;
        targetTextColor = hoverTextColor;
        
        // Curseur Custom
        if (customCursor != null)
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = initialScale;
        targetRotation = initialRotation;
        targetButtonColor = originalButtonColor;
        targetTextColor = originalTextColor;

        // Reset du curseur
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}