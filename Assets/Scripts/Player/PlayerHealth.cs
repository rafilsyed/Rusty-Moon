using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f;
    public float currentHealth;
    public bool invincible = false;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image damageEffectImage;
    [SerializeField] private float damageEffectMaxAlpha = 0.8f;
    [SerializeField] private float damageEffectFadeDuration = 0.5f;
    private Coroutine damageEffectCoroutine;
    [SerializeField] private AudioClip damageSfx;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float damageSfxVolume = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.J))
        {
            invincible = !invincible;

            if (invincible) Debug.Log("🦸 GOD MODE ACTIVÉ ");
            else Debug.Log("💀 GOD MODE DÉSACTIVÉ ");
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (invincible) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        if (damageSfx != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(damageSfx, damageSfxVolume);
            }
            else
            {
                Camera cam = Camera.main;
                Vector3 pos = (cam != null) ? cam.transform.position : transform.position;
                AudioSource.PlayClipAtPoint(damageSfx, pos, damageSfxVolume);
            }
        }

        if (damageEffectImage != null)
        {
            if (damageEffectCoroutine != null) StopCoroutine(damageEffectCoroutine);
            SetDamageEffectAlpha(damageEffectMaxAlpha);
            damageEffectCoroutine = StartCoroutine(FadeDamageEffect());
        }

        if (currentHealth <= 0f)
        {
            Debug.Log("💀 Le joueur est mort !");
        }
    }


    void UpdateUI()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

    private void SetDamageEffectAlpha(float a)
    {
        if (damageEffectImage == null) return;
        Color c = damageEffectImage.color;
        c.a = Mathf.Clamp01(a);
        damageEffectImage.color = c;
    }

    private IEnumerator FadeDamageEffect()
    {
        if (damageEffectImage == null) yield break;
        float startAlpha = damageEffectImage.color.a;
        float t = 0f;

        while (t < damageEffectFadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, 0f, t / damageEffectFadeDuration);
            SetDamageEffectAlpha(a);
            yield return null;
        }

        SetDamageEffectAlpha(0f);
        damageEffectCoroutine = null;
    }
}
