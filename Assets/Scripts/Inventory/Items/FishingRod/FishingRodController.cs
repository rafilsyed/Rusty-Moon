using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [Header("Réglages Animation")]
    public float angleDeRecul = -45f; 
    public float vitesseDeCharge = 3f; 
    public float vitesseDeLancer = 15f; 

    [Header("Réglages Pêche")]
    public float castPower = 15f; 
    public AudioClip castSound;
    public float volume = 1f;

    [Header("Lancer d'Hameçon")]
    public GameObject hookPrefab; 
    public Transform spawnPoint;  
    
    // 👇 NOUVEAU : Le composant qui dessine la ligne
    [Header("Fil de Pêche")]
    public LineRenderer filDePeche; 

    private Quaternion rotationInitiale;
    private float angleActuel = 0f;
    private bool estEnCharge = false;
    private GameObject hameconActuel; 

    void Start()
    {
        rotationInitiale = transform.localRotation;
        
        // On s'assure que le fil est caché au démarrage
        if (filDePeche != null) filDePeche.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            estEnCharge = true;
            angleActuel = Mathf.Lerp(angleActuel, angleDeRecul, Time.deltaTime * vitesseDeCharge);
        }
        else if (Input.GetMouseButtonUp(1) && estEnCharge)
        {
            estEnCharge = false;
            angleActuel = 40f; 

            LancerLaLigne();
        }
        else
        {
            angleActuel = Mathf.Lerp(angleActuel, 0f, Time.deltaTime * vitesseDeLancer);
        }

        transform.localRotation = rotationInitiale * Quaternion.Euler(angleActuel, 0f, 0f);
    }

    // 👇 NOUVELLE FONCTION : Gère le fil à chaque image
    void LateUpdate()
    {
        if (filDePeche != null)
        {
            if (hameconActuel != null)
            {
                // Si un hameçon existe, on active le fil
                filDePeche.enabled = true;
                
                // Le début du fil (Point 0) est au bout de la canne
                filDePeche.SetPosition(0, spawnPoint.position);
                
                // La fin du fil (Point 1) est sur l'hameçon
                filDePeche.SetPosition(1, hameconActuel.transform.position);
            }
            else
            {
                // Si l'hameçon n'est plus là, on cache le fil
                filDePeche.enabled = false;
            }
        }
    }

    private void LancerLaLigne()
    {
        if (castSound != null)
        {
            GameObject temp = new GameObject("FishingSoundTemp");
            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = castSound;
            source.volume = volume;
            source.Play();
            Destroy(temp, castSound.length);
        }

        if (hookPrefab != null && spawnPoint != null)
        {
            if (hameconActuel != null)
            {
                Destroy(hameconActuel);
            }

            hameconActuel = Instantiate(hookPrefab, spawnPoint.position, spawnPoint.rotation);

            Rigidbody hookRb = hameconActuel.GetComponent<Rigidbody>();
            if (hookRb != null)
            {
                Vector3 directionLancer = Camera.main.transform.forward + (Vector3.up * 0.3f);
                hookRb.AddForce(directionLancer.normalized * castPower, ForceMode.Impulse);
            }
        }
    }
}