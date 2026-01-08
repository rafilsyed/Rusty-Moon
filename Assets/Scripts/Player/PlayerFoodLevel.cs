using UnityEngine;
using UnityEngine.UI;

public class PlayerFoodLevel : MonoBehaviour
{
    [SerializeField] private Image foodBarImage;

    public float maxFoodLevel = 100f;
    public float currentFoodLevel;
    public float foodDepletionRate = 10f;

    void Start()
    {
        currentFoodLevel = maxFoodLevel;
        UpdateUI();
    }

    void Update()
    {
        DepleteFoodLevel();
        UpdateUI();
    }

    void DepleteFoodLevel()
    {
        currentFoodLevel -= foodDepletionRate * Time.deltaTime;
        currentFoodLevel = Mathf.Clamp(currentFoodLevel, 0, maxFoodLevel);
    }

    public void EatFood(float foodAmount)
    {
        currentFoodLevel += foodAmount;
        currentFoodLevel = Mathf.Clamp(currentFoodLevel, 0, maxFoodLevel);
        UpdateUI();
    }

    void UpdateUI()
    {
        foodBarImage.fillAmount = GetFoodLevelPercentage();
    }

    public float GetFoodLevelPercentage()
    {
        return currentFoodLevel / maxFoodLevel;
    }
}
