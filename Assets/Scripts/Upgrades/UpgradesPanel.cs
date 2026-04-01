using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;

public class UpgradesPanel : MonoBehaviour, IInteractable
{
    [SerializeField] public PlayerMovement playerMovement;
    public List<Upgrade> availableUpgrades = new();
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    private GameObject panelUI;

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        ShowUpgradesUI();
        interactSuccessful = true;

        playerMovement.SetCanMove(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void ShowUpgradesUI()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(true);
            return;
        }

        GameObject canvasGO = new GameObject("UpgradeCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        panelUI = new GameObject("UpgradePanel");
        panelUI.transform.SetParent(canvasGO.transform);

        RectTransform panelRect = panelUI.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(500, 400);

        Image bg = panelUI.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.8f);

        CreateText("UPGRADES", panelUI.transform, new Vector2(0, 150), 40);

        float startY = 60;

        foreach (var upgrade in availableUpgrades)
        {
            string text = $"{upgrade.Name} - {upgrade.Price}$\n{upgrade.Description}";
            CreateText(text, panelUI.transform, new Vector2(0, startY), 24);
            startY -= 80;
        }
    }

    void CreateText(string content, Transform parent, Vector2 pos, int size)
    {
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(parent);

        TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = size;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;

        RectTransform rect = text.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(450, 80);
    }

    public void EndInteraction()
    {
        if (panelUI != null)
        {
            panelUI.SetActive(false);
        }

        playerMovement.SetCanMove(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}