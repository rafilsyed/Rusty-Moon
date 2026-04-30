using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CraftPanel : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    [Header("Settings")]
    public PlayerMovement playerMovement;
    public AudioClip craftSound;
    public AudioClip notEnoughMaterialsSound;
    public string menuTitle = "Établi";

    private GameObject canva;
    private List<CraftRecipe> craftRecipes;

    // Palette de couleurs
    private readonly Color colorBackground = new Color(0.1f, 0.1f, 0.1f, 0.95f);
    private readonly Color colorPanel = new Color(0.15f, 0.15f, 0.15f, 1f);
    private readonly Color colorAccent = new Color(0.12f, 0.6f, 1f, 1f); // Bleu moderne
    private readonly Color colorText = new Color(0.9f, 0.9f, 0.9f, 1f);

    void Start()
    {
        craftRecipes = new List<CraftRecipe>(GetComponentsInChildren<CraftRecipe>());
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        interactSuccessful = true;

        if (canva != null)
        {
            canva.SetActive(true);
            TogglePlayerState(false);
            return;
        }

        // --- Création de la Racine ---
        canva = new GameObject("CraftCanvas");
        Canvas canvas = canva.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canva.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canva.AddComponent<GraphicRaycaster>();

        // --- Fond Flou / Sombre ---
        GameObject bg = CreateUIObject("BlurBackground", canva.transform);
        SetRect(bg, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        bg.AddComponent<Image>().color = new Color(0, 0, 0, 0.6f);
        // Ajout d'un bouton invisible pour fermer en cliquant à côté
        Button bgBtn = bg.AddComponent<Button>();
        bgBtn.onClick.AddListener(EndInteraction);

        // --- Fenêtre Principale ---
        GameObject mainPanel = CreateUIObject("MainPanel", canva.transform);
        SetRect(mainPanel, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(650, 450));
        var panelImg = mainPanel.AddComponent<Image>();
        panelImg.color = colorPanel;
        panelImg.type = Image.Type.Sliced; // Idéal si tu as un sprite de bordure arrondie
        
        // Ajout d'une ombre/contour
        var outline = mainPanel.AddComponent<Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.5f);
        outline.effectDistance = new Vector2(2, -2);

        // --- En-tête (Header) ---
        GameObject header = CreateUIObject("Header", mainPanel.transform);
        SetRect(header, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -25), new Vector2(0, 50));
        header.AddComponent<Image>().color = colorAccent;

        var title = CreateText(menuTitle, header.transform);
        title.fontSize = 20;
        title.fontStyle = FontStyles.Bold;
        title.color = Color.white;
        title.alignment = TextAlignmentOptions.Center;

        // Bouton Fermer (X)
        GameObject closeBtnGO = CreateUIObject("CloseBtn", header.transform);
        SetRect(closeBtnGO, new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(-25, 0), new Vector2(30, 30));
        closeBtnGO.AddComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        var cb = closeBtnGO.AddComponent<Button>();
        cb.onClick.AddListener(EndInteraction);
        CreateText("X", closeBtnGO.transform).color = Color.white;

        // --- Zone de Grille (Contenu) ---
        GameObject scrollArea = CreateUIObject("ScrollArea", mainPanel.transform);
        SetRect(scrollArea, Vector2.zero, Vector2.one, new Vector2(0, -25), new Vector2(-20, -70));
        
        var grid = scrollArea.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(140, 180);
        grid.spacing = new Vector2(15, 15);
        grid.padding = new RectOffset(15, 15, 15, 15);
        grid.childAlignment = TextAnchor.UpperCenter;

        // --- Génération des Recettes ---
        foreach (var recipe in craftRecipes)
        {
            CreateRecipeSlot(recipe, scrollArea.transform);
        }

        TogglePlayerState(false);
    }

    private void CreateRecipeSlot(CraftRecipe recipe, Transform parent)
    {
        GameObject slot = CreateUIObject("Slot_" + recipe.ResultItem.DisplayName, parent);
        var slotImg = slot.AddComponent<Image>();
        slotImg.color = new Color(1, 1, 1, 0.05f);

        // Icône
        GameObject iconGO = CreateUIObject("Icon", slot.transform);
        SetRect(iconGO, new Vector2(0.5f, 0.7f), new Vector2(0.5f, 0.7f), Vector2.zero, new Vector2(80, 80));
        var icon = iconGO.AddComponent<Image>();
        icon.sprite = recipe.ResultItem.Icon;
        icon.preserveAspect = true;

        // Nom
        var nameTxt = CreateText(recipe.ResultItem.DisplayName, slot.transform);
        SetRect(nameTxt.gameObject, new Vector2(0, 0.35f), new Vector2(1, 0.45f), Vector2.zero, Vector2.zero);
        nameTxt.fontSize = 14;
        nameTxt.color = colorText;

        // Bouton Craft
        GameObject btnGO = CreateUIObject("CraftButton", slot.transform);
        SetRect(btnGO, new Vector2(0.1f, 0.05f), new Vector2(0.9f, 0.25f), Vector2.zero, Vector2.zero);
        var btnImg = btnGO.AddComponent<Image>();
        btnImg.color = colorAccent;
        
        var btn = btnGO.AddComponent<Button>();
        btn.onClick.AddListener(() => recipe.Craft(craftSound, notEnoughMaterialsSound));
        
        // Effet de survol simple
        ColorBlock cb = btn.colors;
        cb.highlightedColor = colorAccent * 1.2f;
        cb.pressedColor = colorAccent * 0.8f;
        btn.colors = cb;

        var btnTxt = CreateText("CRAFTER", btnGO.transform);
        btnTxt.fontSize = 12;
        btnTxt.fontStyle = FontStyles.Bold;
        btnTxt.color = Color.white;
    }

    private void TogglePlayerState(bool canMove)
    {
        if(playerMovement) playerMovement.SetCanMove(canMove);
        Cursor.lockState = canMove ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !canMove;
    }

    public void EndInteraction()
    {
        if (canva != null) canva.SetActive(false);
        TogglePlayerState(true);
        OnInteractionComplete?.Invoke(this);
    }

    // --- Helpers ---
    private GameObject CreateUIObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    private void SetRect(GameObject go, Vector2 min, Vector2 max, Vector2 pos, Vector2 size)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
    }

    private TextMeshProUGUI CreateText(string content, Transform parent)
    {
        GameObject go = CreateUIObject("Text", parent);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = 18;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = colorText;
        
        SetRect(go, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        return tmp;
    }
}