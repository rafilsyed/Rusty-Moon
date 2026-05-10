using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SailController : MonoBehaviour
{
    [Header("Réglages de la Voile")]
    public float vitesseRotation = 45f; 

    private bool joueurProche = false;
    private RaftController raftController;
    private GameObject interactPopup;
     private TextMeshProUGUI popupText;

    void Start()
    {
        CreatePopup();
        raftController = GetComponentInParent<RaftController>();
    }

    void Update()
    {
        if (joueurProche)
        {
            if (Keyboard.current.fKey.isPressed)
            {
                transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);
            }
            
            if (Keyboard.current.qKey.isPressed)
            {
                transform.Rotate(Vector3.down * vitesseRotation * Time.deltaTime);
            }

            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                if (raftController != null)
                {
                    raftController.ToggleVoile();
                }
            }

            if (raftController.voileActive)
            {
                ShowPopup("Tourner la voile avec A ou F");
            }
            else
            {
                ShowPopup("T pour lever la voile");
            }
        }else
        {
            if (interactPopup != null)
            {
                interactPopup.SetActive(false);
            }
        }
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

        Image bg = interactPopup.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.6f);

        GameObject textGO = new GameObject("PopupText");
        textGO.transform.SetParent(interactPopup.transform);

        popupText = textGO.AddComponent<TextMeshProUGUI>();
        popupText.text = "none";
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

    public void ShowPopup(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
            interactPopup.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurProche = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurProche = false;
        }
    }
}