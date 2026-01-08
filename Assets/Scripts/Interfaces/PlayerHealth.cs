using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    [Header("Réglages Santé")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider; 

    [Header("Mode Triche")]
    public bool isInvincible = false; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    void Update()
    {
        //  GOD MODE (Touche J)
        if (Input.GetKeyDown(KeyCode.J))
        {
            isInvincible = !isInvincible;
            
            if(isInvincible) Debug.Log("🦸 GOD MODE ACTIVÉ ");
            else Debug.Log("💀 GOD MODE DÉSACTIVÉ ");
        }
    }

    // Cette fonction sera activé par la Machine à Air ou les ennemis
    public void TakeDamage(float damageAmount)
    {
        // si invincible, ne pas prendre de dégâts
        if (isInvincible) return;

        currentHealth -= damageAmount;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("GAME OVER");
        GetComponent<PlayerMovement>().enabled = false;
    }
}