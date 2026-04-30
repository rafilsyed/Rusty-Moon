using UnityEngine;

// 👇 NOUVEAU : On crée une structure pour lier un modèle 3D à un objet d'inventaire
[System.Serializable]
public struct PoissonPeche
{
    public GameObject prefab3D;       // Le modèle 3D (ex: le poisson rouge, le poisson bleu...)
    public FishItemData itemDonne;    // L'objet d'inventaire associé (ex: "Petit Poisson")
}

public class FishingRodController : MonoBehaviour
{
    [Header("Réglages Animation")]
    public float angleDeRecul = -45f; 
    public float vitesseDeCharge = 3f; 
    public float vitesseDeLancer = 15f; 

    [Header("Réglages Pêche")]
    public float castPower = 15f; 
    public AudioClip castSound;    
    public AudioClip reelSound;    
    public float volume = 1f;

    [Header("Lancer d'Hameçon")]
    public GameObject hookPrefab; 
    public Transform spawnPoint;  
    
    [Header("Fil de Pêche")]
    public LineRenderer filDePeche; 

    [Header("Mécanique de Capture")]
    public AudioClip splashSound; 
    public float longueurFilPoisson = 1.5f; 
    public float tempsMinAvantTouche = 2f;
    public float tempsMaxAvantTouche = 6f;
    public float tempsPourReagir = 2f; 
    
    [Header("Table de Butin")]
    public PoissonPeche[] poissonsPossibles;

    private Quaternion rotationInitiale;
    private float angleActuel = 0f;
    private bool estEnCharge = false;
    private GameObject hameconActuel; 
    private GameObject poissonAccroche; 
    
    // On mémorise les infos du poisson qu'on vient de tirer au sort
    private PoissonPeche poissonEnCours; 

    private bool attendTouche = false;
    private bool toucheActive = false;
    private float chronoTouche = 0f;
    private float cibleChrono = 0f;
    private float chronoReaction = 0f;

    // Variables pour la physique du fil
    private Vector3 velocitePoisson;
    private Vector3 positionPoissonSimulee;
    private PlayerInventoryHolder inventaireJoueur;
    void Start()
    {
        rotationInitiale = transform.localRotation;
        if (filDePeche != null) filDePeche.enabled = false;

        // On cherche le composant d'inventaire sur le parent (le Joueur qui tient la canne)
        inventaireJoueur = GetComponentInParent<PlayerInventoryHolder>();
        
        // Sécurité : si la canne n'est pas enfant du joueur, on le cherche dans la scène
        if (inventaireJoueur == null)
        {
            inventaireJoueur = FindAnyObjectByType<PlayerInventoryHolder>();
        }
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

        // --- 👇 MODIFIÉ : RAMENER OU STOCKER (Clic Gauche) 👇 ---
        if (Input.GetMouseButtonDown(0))
        {
            if (poissonAccroche != null)
            {
                // Si un poisson pend au bout de la canne, on le range !
                StockerLePoisson();
            }
            else
            {
                // Sinon, c'est qu'on ramène l'hameçon normalement
                RamenerLaLigne();
            }
        }

        if (attendTouche)
        {
            chronoTouche += Time.deltaTime;
            if (chronoTouche >= cibleChrono) DeclencherTouche();
        }
        else if (toucheActive)
        {
            chronoReaction -= Time.deltaTime;
            if (chronoReaction <= 0f)
            {
                toucheActive = false;
                Debug.Log("Trop lent ! Le poisson s'est échappé.");
            }
        }
    }

    void LateUpdate()
    {
        if (filDePeche != null)
        {
            if (hameconActuel != null)
            {
                filDePeche.enabled = true;
                filDePeche.SetPosition(0, spawnPoint.position);
                filDePeche.SetPosition(1, hameconActuel.transform.position);
            }
            else if (poissonAccroche != null)
            {
                filDePeche.enabled = true;
                filDePeche.SetPosition(0, spawnPoint.position);
                
                velocitePoisson += Vector3.down * 15f * Time.deltaTime;
                velocitePoisson = Vector3.Lerp(velocitePoisson, Vector3.zero, Time.deltaTime * 3f);
                positionPoissonSimulee += velocitePoisson * Time.deltaTime;
                
                Vector3 directionFil = positionPoissonSimulee - spawnPoint.position;
                positionPoissonSimulee = spawnPoint.position + (directionFil.normalized * longueurFilPoisson);
                
                poissonAccroche.transform.position = positionPoissonSimulee;
                poissonAccroche.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
                filDePeche.SetPosition(1, poissonAccroche.transform.position);
            }
            else
            {
                filDePeche.enabled = false;
            }
        }
    }

    private void LancerLaLigne()
    {
        if (poissonAccroche != null) Destroy(poissonAccroche);

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
            BaitBehavior[] tousLesHamecons = Object.FindObjectsByType<BaitBehavior>(FindObjectsSortMode.None);
            foreach (BaitBehavior ancienHamecon in tousLesHamecons)
            {
                if (ancienHamecon != null) Destroy(ancienHamecon.gameObject);
            }

            hameconActuel = Instantiate(hookPrefab, spawnPoint.position, spawnPoint.rotation);

            Rigidbody hookRb = hameconActuel.GetComponent<Rigidbody>();
            if (hookRb != null)
            {
                Vector3 directionLancer = Camera.main.transform.forward + (Vector3.up * 0.3f);
                hookRb.AddForce(directionLancer.normalized * castPower, ForceMode.Impulse);
            }

            attendTouche = true;
            toucheActive = false;
            chronoTouche = 0f;
            cibleChrono = Random.Range(tempsMinAvantTouche, tempsMaxAvantTouche);
        }
    }

    private void DeclencherTouche()
    {
        attendTouche = false;
        toucheActive = true;
        chronoReaction = tempsPourReagir; 

        if (splashSound != null)
        {
            GameObject temp = new GameObject("SplashSoundTemp");
            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = splashSound;
            source.volume = volume;
            source.Play();
            Destroy(temp, splashSound.length);
        }

        if (hameconActuel != null)
        {
            Rigidbody hookRb = hameconActuel.GetComponent<Rigidbody>();
            if (hookRb != null)
            {
                hookRb.isKinematic = false;
                hookRb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }
        }
    }

    private void RamenerLaLigne()
    {
        attendTouche = false;

        if (hameconActuel != null)
        {
            Destroy(hameconActuel);
            hameconActuel = null; 

            // 👇 MODIFIÉ : On tire un poisson au sort ! 👇
            if (toucheActive && poissonsPossibles.Length > 0)
            {
                // 1. On choisit un poisson au hasard dans notre liste
                int indexAleatoire = Random.Range(0, poissonsPossibles.Length);
                poissonEnCours = poissonsPossibles[indexAleatoire];
                
                // 2. On instancie son modèle 3D
                if (poissonEnCours.prefab3D != null && spawnPoint != null)
                {
                    poissonAccroche = Instantiate(poissonEnCours.prefab3D, spawnPoint.position, Quaternion.identity);
                    
                    Rigidbody rbPoisson = poissonAccroche.GetComponent<Rigidbody>();
                    if (rbPoisson != null) rbPoisson.isKinematic = true;
                    
                    Collider[] colliders = poissonAccroche.GetComponentsInChildren<Collider>();
                    foreach(Collider col in colliders) col.enabled = false;
                    
                    positionPoissonSimulee = spawnPoint.position + (Vector3.down * longueurFilPoisson);
                    velocitePoisson = Vector3.zero;
                }
            }

            toucheActive = false;

            if (reelSound != null)
            {
                GameObject temp = new GameObject("ReelSoundTemp");
                AudioSource source = temp.AddComponent<AudioSource>();
                source.clip = reelSound;
                source.volume = volume;
                source.Play();
                Destroy(temp, reelSound.length);
            }
        }
    }

    // 👇 NOUVELLE FONCTION : Ajouter à l'inventaire 👇
    private void StockerLePoisson()
    {
        if (poissonAccroche != null)
        {
            // On s'assure qu'on a bien trouvé l'inventaire du joueur
            if (inventaireJoueur != null)
            {
                // On tente d'ajouter 1 poisson de la catégorie tirée au sort (itemDonne)
                bool aEteAjoute = inventaireJoueur.AddToInventory(poissonEnCours.itemDonne, 1);

                if (aEteAjoute)
                {
                    Debug.Log("Bravo ! Ajout de " + poissonEnCours.itemDonne.name + " à l'inventaire.");
                    
                    // L'ajout a réussi, on fait disparaître le modèle 3D du bout de la canne
                    Destroy(poissonAccroche);
                    poissonAccroche = null;
                }
                else
                {
                    // L'inventaire est plein (la fonction AddToInventory a renvoyé false)
                    Debug.LogWarning("Ton inventaire est plein ! Le poisson reste au bout du fil.");
                    // Le joueur devra jeter un objet pour faire de la place avant de recliquer.
                }
            }
            else
            {
                Debug.LogError("PlayerInventoryHolder introuvable dans la scène !");
            }
        }
    }
}