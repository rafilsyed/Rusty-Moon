using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;

public class CraftPanel : MonoBehaviour, IInteractable
{
    public GameObject craftPanelUI;
    public GameObject craftButtonPrefab;
    public PlayerMovement playerMovement;
    private List<CraftRecipe> craftRecipes;
    public AudioClip craftSound;
    public AudioClip cannotCraftSound;
    private readonly Dictionary<GameObject, CraftRecipe> buttonRecipeMap = new();
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Start()
    {
        craftRecipes = GetComponentsInChildren<CraftRecipe>(true).ToList();

        foreach (CraftRecipe recipe in craftRecipes)
        {
            GameObject buttonGO = Instantiate(craftButtonPrefab, craftPanelUI.transform, false);

            Image image = buttonGO.GetComponentInChildren<Image>();
            Button button = buttonGO.GetComponentInChildren<Button>();

            if (image != null && recipe.ResultItem != null)
            {
                image.sprite = recipe.ResultItem.Icon;
            }

            if (button != null)
            {
                button.onClick.AddListener(() => recipe.Craft(craftSound, cannotCraftSound));
            }

            buttonGO.SetActive(false);
            buttonRecipeMap[buttonGO] = recipe;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        foreach (GameObject buttonGO in buttonRecipeMap.Keys)
        {
            buttonGO.SetActive(true);
        }

		playerMovement.SetCanMove(false);
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;

        interactSuccessful = true;
    }

    public void EndInteraction()
    {
        foreach (GameObject buttonGO in buttonRecipeMap.Keys)
        {
            buttonGO.SetActive(false);
        }

        playerMovement.SetCanMove(true);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
}