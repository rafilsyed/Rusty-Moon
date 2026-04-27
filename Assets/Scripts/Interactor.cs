using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public Transform interactPoint;
    public LayerMask interactableLayer;
    public float interactPointRadius = 1f;
    public static bool IsInteracting { get; set; }

    private GameObject interactPopup;
    private TextMeshProUGUI popupText;
    private bool forceEndInteraction = false;

    void Start()
    {
        CreatePopup();
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(interactPoint.position, interactPointRadius, interactableLayer);
        bool hasInteractable = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<IInteractable>() != null)
            {
                hasInteractable = true;
                break;
            }
        }

        interactPopup.SetActive(hasInteractable && !IsInteracting);

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) StartInteraction(interactable);
            }
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame && IsInteracting || forceEndInteraction)
        {
            EndInteraction();

            if (colliders.Length > 0)
            {
                var interactable = colliders[0].GetComponent<IInteractable>();
                interactable.EndInteraction();
            }

            forceEndInteraction = false;
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    void EndInteraction()
    {
        IsInteracting = false;
    }

    public void ForceEndInteraction()
    {
        forceEndInteraction = true;
    }

    void CreatePopup()
    {
        GameObject canvasGO = new GameObject("InteractCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        interactPopup = new GameObject("PopupContainer");
        interactPopup.transform.SetParent(canvasGO.transform);

        RectTransform containerRect = interactPopup.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.anchoredPosition = Vector2.zero;
        containerRect.sizeDelta = new Vector2(300, 80);

        UnityEngine.UI.Image bg = interactPopup.AddComponent<UnityEngine.UI.Image>();
        bg.color = new Color(0, 0, 0, 0.6f);

        GameObject textGO = new GameObject("PopupText");
        textGO.transform.SetParent(interactPopup.transform);

        popupText = textGO.AddComponent<TextMeshProUGUI>();
        popupText.text = "<b>[ F ]</b>   Interagir";
        popupText.fontSize = 36;
        popupText.alignment = TextAlignmentOptions.Center;
        popupText.color = Color.white;

        RectTransform textRect = popupText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        popupText.outlineWidth = 0.2f;

        interactPopup.SetActive(false);
    }
}