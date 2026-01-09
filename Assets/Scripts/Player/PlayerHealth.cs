using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;

    public float maxHealth = 100f;
    public float currentHealth;
    public bool invincible = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.J))
        {
            invincible = !invincible;

            if(invincible) Debug.Log("🦸 GOD MODE ACTIVÉ ");
            else Debug.Log("💀 GOD MODE DÉSACTIVÉ ");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (invincible) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    void UpdateUI()
    {
        healthBarImage.fillAmount = GetHealthPercentage();
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
