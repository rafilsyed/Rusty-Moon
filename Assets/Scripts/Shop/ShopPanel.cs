using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour, IInteractable
{
    [Header("Configuration")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInventoryHolder playerInventory;
    [SerializeField] private PlayerMoney playerMoney;
    [SerializeField] private List<ItemPrice> itemPrices = new();

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private GameObject canvasGO;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip notEnoughMoneySound;

    [System.Serializable]
    public class ItemPrice
    {
        public InventoryItemData Item;
        public int Price;
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        if (canvasGO == null) CreateShopUI();

        canvasGO.SetActive(true);
        interactSuccessful = true;

        TogglePlayerState(false);
    }

    public void EndInteraction()
    {
        if (canvasGO != null) canvasGO.SetActive(false);
        TogglePlayerState(true);
        OnInteractionComplete?.Invoke(this);
    }

    private void TogglePlayerState(bool canMove)
    {
        playerMovement.SetCanMove(canMove);
        Cursor.lockState = canMove ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !canMove;
    }

    private void CreateShopUI()
    {
        canvasGO = new GameObject("ShopCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(canvasGO.transform);
        RectTransform mainPanelRect = mainPanel.AddComponent<RectTransform>();
        mainPanelRect.sizeDelta = new Vector2(450, 650);
        mainPanelRect.anchoredPosition = Vector2.zero;
        mainPanel.AddComponent<Image>().color = new Color(0, 0, 0, 0.9f);

        GameObject contentGO = new GameObject("ContentContainer");
        contentGO.transform.SetParent(mainPanel.transform);
        RectTransform contentRect = contentGO.AddComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(400, 500);
        contentRect.anchoredPosition = new Vector2(0, 40);

        VerticalLayoutGroup vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.spacing = 4;
        vlg.childControlHeight = false;
        vlg.childControlWidth = false;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = false;

        foreach (var itemPrice in itemPrices)
        {
            CreateItemButton(itemPrice, contentGO.transform);
        }

        CreateSellButton(mainPanel.transform);
    }

    private void CreateItemButton(ItemPrice ip, Transform parent)
    {
        GameObject btnGO = new GameObject(ip.Item.DisplayName + " Button");
        btnGO.transform.SetParent(parent);

        Button btn = btnGO.AddComponent<Button>();
        btnGO.AddComponent<Image>().color = Color.white;
        btnGO.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 50);
    
        Sprite sprite = ip.Item.Icon;
        if (sprite != null)
        {
            GameObject iconGO = new GameObject("Icon");
            iconGO.transform.SetParent(btnGO.transform);
            Image iconImg = iconGO.AddComponent<Image>();
            iconImg.sprite = sprite;
            RectTransform iconRect = iconGO.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(40, 40);
            iconRect.anchoredPosition = new Vector2(-100, 0);
        }

        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform);
        TextMeshProUGUI txt = textGO.AddComponent<TextMeshProUGUI>();
        txt.text = $"{ip.Item.DisplayName} (<color=green>${ip.Price}</color>)";
        txt.color = Color.black;
        txt.alignment = TextAlignmentOptions.Center;
        txt.fontSize = 24;

        RectTransform txtRect = textGO.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.sizeDelta = Vector2.zero;

        btn.onClick.AddListener(() => PurchaseItem(ip));
    }

    private void CreateSellButton(Transform parent)
    {
        GameObject sellGO = new GameObject("SellButton");
        sellGO.transform.SetParent(parent, false);

        Button btn = sellGO.AddComponent<Button>();
        Image img = sellGO.AddComponent<Image>();
        img.color = Color.red;

        RectTransform rt = sellGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(150, 40);
        rt.anchoredPosition = new Vector2(0, -280);

        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(sellGO.transform, false);

        TextMeshProUGUI txt = textGO.AddComponent<TextMeshProUGUI>();

        txt.text = "VENDRE TOUT";
        txt.alignment = TextAlignmentOptions.Center;
        txt.fontSize = 20;
        txt.color = Color.white;

        RectTransform txtRect = txt.rectTransform;
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        btn.onClick.AddListener(() => playerInventory.SellAllFish(playerMoney));
    }

    private void PurchaseItem(ItemPrice ip)
    {
        if (playerMoney.GetMoney() >= ip.Price)
        {
            playerMoney.RemoveMoney(ip.Price);
            playerInventory.AddToInventory(ip.Item, 1);
            if (buySound) AudioSource.PlayClipAtPoint(buySound, Camera.main.transform.position);
            Debug.Log($"Bought {ip.Item.DisplayName}");
        }
        else
        {
            if (notEnoughMoneySound) AudioSource.PlayClipAtPoint(notEnoughMoneySound, Camera.main.transform.position);
            Debug.Log("Not enough money!");
        }
    }
}