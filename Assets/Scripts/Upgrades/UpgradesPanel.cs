using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class UpgradesPanel : MonoBehaviour, IInteractable
{
    [Header("Configuration")]
    [SerializeField] public PlayerMovement playerMovement; 
    [SerializeField] private PlayerMoney playerMoney;
    public List<Upgrade> availableUpgrades = new();
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private GameObject canvasGO;
    private GameObject mainPanel;
    private Transform contentContainer;
    [SerializeField] private AudioClip upgradeSound;
    [SerializeField] private AudioClip notEnoughMoneySound;

    public void Start()
    {
        foreach (var upg in availableUpgrades)
        {
            upg.reset();
        }
    } 

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        ShowUpgradesUI();
        interactSuccessful = true;
        playerMovement.SetCanMove(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowUpgradesUI()
    {
        if (canvasGO != null)
        {
            canvasGO.SetActive(true);
            RefreshUpgrades();
            return;
        }

        // 1. Canvas & Setup
        canvasGO = new GameObject("UpgradeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1.2f; // Taille globale divisée par 2

        canvasGO.AddComponent<GraphicRaycaster>();

        // 2. Overlay sombre
        GameObject bg = CreateUIObject("Background", canvasGO.transform);
        SetRect(bg, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        bg.AddComponent<Image>().color = new Color(0, 0, 0, 0.7f);

        // 3. Fenêtre Principale
        mainPanel = CreateUIObject("MainPanel", bg.transform);
        SetRect(mainPanel, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(700, 550));
        var panelImg = mainPanel.AddComponent<Image>();
        panelImg.color = new Color(0.12f, 0.12f, 0.14f, 1f);

        // 4. Titre Principal
        GameObject title = CreateUIObject("Title", mainPanel.transform);
        SetRect(title, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0, -50), new Vector2(600, 60));
        var titleTxt = title.AddComponent<TextMeshProUGUI>();
        titleTxt.text = "AMÉLIORATIONS";
        titleTxt.fontSize = 38;
        titleTxt.alignment = TextAlignmentOptions.Center;
        titleTxt.color = new Color(0.3f, 0.75f, 1f);
        titleTxt.fontStyle = FontStyles.Bold;

        // 5. Scroll View (Indispensable pour la propreté)
        GameObject scrollView = CreateUIObject("ScrollView", mainPanel.transform);
        SetRect(scrollView, Vector2.zero, Vector2.one, new Vector2(0, -50), new Vector2(-40, -120));
        
        GameObject viewport = CreateUIObject("Viewport", scrollView.transform);
        SetRect(viewport, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        viewport.AddComponent<RectMask2D>();

        GameObject content = CreateUIObject("Content", viewport.transform);
        SetRect(content, new Vector2(0, 1), new Vector2(1, 1), Vector2.zero, new Vector2(0, 400));
        content.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
        
        var layout = content.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 15;
        layout.padding = new RectOffset(15, 15, 15, 15);
        layout.childControlHeight = false;
        layout.childForceExpandHeight = false;
        content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        contentContainer = content.transform;
        RefreshUpgrades();
    }

    void RefreshUpgrades()
    {
        foreach (Transform child in contentContainer) Destroy(child.gameObject);
        foreach (var upg in availableUpgrades) CreateUpgradeCard(upg);
    }

    void CreateUpgradeCard(Upgrade upg)
    {
        // La Carte
        GameObject card = CreateUIObject($"Card_{upg.Name}", contentContainer);
        SetRect(card, Vector2.zero, Vector2.zero, Vector2.zero, new Vector2(640, 110));
        card.AddComponent<Image>().color = new Color(0.18f, 0.19f, 0.22f);

        // Icône (bien à gauche)
        GameObject iconGo = CreateUIObject("Icon", card.transform);
        SetRect(iconGo, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(55, 0), new Vector2(80, 80));
        var img = iconGo.AddComponent<Image>();
        img.sprite = upg.Icon;
        img.preserveAspect = true;

        // Zone de texte (au milieu) - espace très réduit avec l'icône
        GameObject textZone = CreateUIObject("TextZone", card.transform);
        SetRect(textZone, new Vector2(0, 0), new Vector2(1, 1), new Vector2(8, 0), new Vector2(-280, -10));
        
        var nameTxt = CreateUIObject("Name", textZone.transform).AddComponent<TextMeshProUGUI>();
        nameTxt.text = upg.Name;
        nameTxt.fontSize = 22;
        nameTxt.fontStyle = FontStyles.Bold;
        SetRect(nameTxt.gameObject, new Vector2(0, 1), new Vector2(1, 1), new Vector2(6, -25), new Vector2(0, 30));
        nameTxt.alignment = TextAlignmentOptions.Left;

        var descTxt = CreateUIObject("Desc", textZone.transform).AddComponent<TextMeshProUGUI>();
        descTxt.text = upg.Description;
        descTxt.fontSize = 14;
        descTxt.color = new Color(0.8f, 0.8f, 0.8f);
        SetRect(descTxt.gameObject, new Vector2(0, 0), new Vector2(1, 0), new Vector2(6, 30), new Vector2(0, 45));
        descTxt.alignment = TextAlignmentOptions.TopLeft;
        descTxt.textWrappingMode = TextWrappingModes.Normal;

        // Bouton d'achat (bien à droite)
        GameObject btnGo = CreateUIObject("BuyBtn", card.transform);
        SetRect(btnGo, new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(-80, 0), new Vector2(130, 80));
        btnGo.AddComponent<Image>().color = new Color(0.2f, 0.45f, 0.25f);
        var btn = btnGo.AddComponent<Button>();
        
        var priceTxt = CreateUIObject("Price", btnGo.transform).AddComponent<TextMeshProUGUI>();
        priceTxt.text = $"<b>{upg.Price}$</b>\n<size=14>NV. {upg.Level}</size>";
        priceTxt.fontSize = 22;
        priceTxt.alignment = TextAlignmentOptions.Center;
        SetRect(priceTxt.gameObject, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

        btn.onClick.AddListener(() =>
        {
            if(playerMoney.GetMoney() < upg.Price)
            {
                Debug.Log("Pas assez d'argent pour acheter cette amélioration !");
                if (notEnoughMoneySound != null)
                {
                    AudioSource.PlayClipAtPoint(notEnoughMoneySound, Camera.main.transform.position);
                }
                return;
            }

            playerMoney.RemoveMoney(upg.Price);
            upg.OnUpgrade();
            
            if (upgradeSound != null)
            {
                AudioSource.PlayClipAtPoint(upgradeSound, Camera.main.transform.position);
            }

            RefreshUpgrades();
        });
    }

    GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    void SetRect(GameObject go, Vector2 anchorMin, Vector2 anchorMax, Vector2 pos, Vector2 size)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
    }

    public void EndInteraction()
    {
        if (canvasGO != null) canvasGO.SetActive(false);
        playerMovement.SetCanMove(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}