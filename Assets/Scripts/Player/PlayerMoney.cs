using TMPro;
using UnityEngine;

public partial class PlayerMoney : MonoBehaviour
{
    [SerializeField] private int money = 0;
    public TextMeshProUGUI moneyText; 
    public TextMeshProUGUI moneyShadowText;

    public void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Argent: " + money + " $";
        }
        if (moneyShadowText != null)
        {
            moneyShadowText.text = "Argent: " + money + " $";
        }
    }

    public int GetMoney() => money;
}