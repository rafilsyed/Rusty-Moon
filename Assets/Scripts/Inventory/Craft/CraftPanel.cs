using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class CraftPanel : MonoBehaviour
{
    public GameObject craftButtonPrefab;
    private List<CraftRecipe> craftRecipes;
    public AudioClip craftSound;
    public AudioClip cannotCraftSound;
    private readonly Dictionary<GameObject, CraftRecipe> buttonRecipeMap = new();

    private void Start()
    {
        craftRecipes = GetComponentsInChildren<CraftRecipe>(true).ToList();

        foreach (CraftRecipe recipe in craftRecipes)
        {
            GameObject buttonGO = Instantiate(craftButtonPrefab, transform);

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

    private void Update()
    {

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            foreach (GameObject buttonGO in buttonRecipeMap.Keys)
            {
                buttonGO.SetActive(true);
            }
        }
        else if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            foreach (GameObject buttonGO in buttonRecipeMap.Keys)
            {
                buttonGO.SetActive(false);
            }
        }
    }
}