using UnityEngine;

public class AirMachine : MonoBehaviour
{
    [Header("Réglages Machine")]
    public float rayonSecurite = 15f; // La taille de la bulle d'air
    public float carburantMax = 100f;
    public float consommationParSeconde = 2f; // Combien ça brûle par seconde
    
    [Header("Réglages Danger")]
    public float degatsToxiques = 5f; // Combien de PV on perd par seconde dehors

    [Header("État (Ne pas toucher)")]
    public float carburantActuel;
    public bool machineActive = true;

    [Header("Lien avec le Joueur")]
    public Transform joueur; // Glisse ton objet "Player" ici

    void Start()
    {
        // On commence avec le réservoir plein
        carburantActuel = carburantMax;
    }

    void Update()
    {
        // --- 1. GESTION DU CARBURANT ---
        if (carburantActuel > 0)
        {
            carburantActuel -= consommationParSeconde * Time.deltaTime;
            machineActive = true;
        }
        else
        {
            carburantActuel = 0;
            machineActive = false;
            // (Ici on pourra plus tard couper le son du moteur)
        }

        // --- 2. GESTION DE LA SURVIE (Dégâts) ---
        
        // On calcule la distance entre la machine et le joueur
        float distanceJoueur = Vector3.Distance(transform.position, joueur.position);

        // Si la machine est éteinte OU si on est trop loin
        if (!machineActive || distanceJoueur > rayonSecurite)
        {
            // On cherche le script de vie sur le joueur
            PlayerHealth vieJoueur = joueur.GetComponent<PlayerHealth>();
            
            if (vieJoueur != null)
            {
                // On applique les dégâts par seconde (ex: 5 PV * temps écoulé)
                vieJoueur.TakeDamage(degatsToxiques * Time.deltaTime);
            }
        }
    }

    // Fonction pour rajouter du fuel (quand on mettra du bois dedans)
    public void AjouterCarburant(float quantite)
    {
        carburantActuel += quantite;
        if (carburantActuel > carburantMax) carburantActuel = carburantMax;
        
        Debug.Log("Carburant ajouté ! Niveau : " + carburantActuel);
    }

    // Dessine la zone de sécurité dans l'éditeur (Cercle Vert/Rouge)
    void OnDrawGizmos()
    {
        if (machineActive) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, rayonSecurite);
    }
}