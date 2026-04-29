using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [Header("Réglages Animation")]
    public float angleDeRecul = -45f; // Jusqu'où la canne se penche en arrière
    public float vitesseDeCharge = 3f; // Vitesse d'armement
    public float vitesseDeLancer = 15f; // Vitesse du coup de fouet (très rapide)

    [Header("Réglages Pêche")]
    public float castPower = 10f;
    public AudioClip castSound;
    public float volume = 1f;

    private Quaternion rotationInitiale;
    private float angleActuel = 0f;
    private bool estEnCharge = false;

    void Start()
    {
        // On mémorise la rotation de base (incluant le fameux offset de 180 degrés)
        rotationInitiale = transform.localRotation;
    }

    void Update()
    {
        // 1 = Clic droit de la souris
        if (Input.GetMouseButton(1))
        {
            estEnCharge = true;
            // La canne recule doucement jusqu'à l'angle de recul
            angleActuel = Mathf.Lerp(angleActuel, angleDeRecul, Time.deltaTime * vitesseDeCharge);
        }
        // Au moment précis où on lâche le clic droit
        else if (Input.GetMouseButtonUp(1) && estEnCharge)
        {
            estEnCharge = false;
            // Coup de fouet vers l'avant ! (On dépasse un peu le zéro pour l'effet)
            angleActuel = 40f; 

            // --- On déclenche la vraie action de pêche ---
            LancerLaLigne();
        }
        else
        {
            // Si on ne fait rien, la canne revient doucement à sa position de repos (0)
            angleActuel = Mathf.Lerp(angleActuel, 0f, Time.deltaTime * vitesseDeLancer);
        }

        // On applique la rotation finale.
        // NOTE: Selon l'orientation de ton modèle 3D, si la canne penche sur le côté au lieu d'en arrière,
        // remplace (angleActuel, 0f, 0f) par (0f, angleActuel, 0f) ou (0f, 0f, angleActuel).
        transform.localRotation = rotationInitiale * Quaternion.Euler(angleActuel, 0f, 0f);
    }

    private void LancerLaLigne()
    {
        Debug.Log("Lancement de la ligne avec une puissance de " + castPower);

        if (castSound != null)
        {
            GameObject temp = new GameObject("FishingSoundTemp");
            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = castSound;
            source.volume = volume;
            source.Play();
            Destroy(temp, castSound.length);
        }
    }
}