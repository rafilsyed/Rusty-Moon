using UnityEngine;

public class AirMachine : MonoBehaviour
{
    [Header("Réglages Machine")]
    public float rayonSecurite = 10f; //taille  bulle d'air
    public float carburantMax = 100f;
    public float consommationParSeconde = 2f; // degats par seconde
    
    [Header("État (Ne pas toucher)")]
    public float carburantActuel;
    public bool machineActive = true;

    [Header("Lien avec le Joueur")]
    public Transform joueur; 

    void Start()
    {
        carburantActuel = carburantMax;
    }

    void Update()
    {
        if (carburantActuel > 0)
        {
            carburantActuel -= consommationParSeconde * Time.deltaTime;
            machineActive = true;
        }
        else
        {
            carburantActuel = 0;
            machineActive = false;
            // Ici, plus tard, on arrêtera les particules et le bruit du moteur
        }

        float distanceJoueur = Vector3.Distance(transform.position, joueur.position);

        if (machineActive && distanceJoueur <= rayonSecurite)
        {
        }
        else
        {
            Debug.LogWarning("ATTENTION : ZONE TOXIQUE ! VOUS ÉTOUFFEZ !");
        }
    }

    public void AjouterCarburant(float quantite)
    {
        carburantActuel += quantite;
        if (carburantActuel > carburantMax) carburantActuel = carburantMax;
        
        Debug.Log("Carburant ajouté ! Niveau : " + carburantActuel);
    }
    void OnDrawGizmos()
    {
        if (machineActive) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, rayonSecurite);
    }
}