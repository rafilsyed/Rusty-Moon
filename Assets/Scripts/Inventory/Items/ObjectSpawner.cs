using UnityEngine;

// 1. On crée une petite classe pour lier l'objet à sa rareté
[System.Serializable] 
public class ObjetSpawne
{
    public GameObject prefab;
    [Tooltip("Plus le chiffre est grand, plus l'objet a de chances d'apparaître")]
    public float chanceDApparition = 10f; // C'est le "poids" de l'objet
}

public class ObjectSpawner : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform joueur;          
    public float hauteurAuDessus = 10f; 

    [Header("Paramètres des objets")]
    // 2. On utilise notre nouvelle classe ici
    public ObjetSpawne[] objetsAPop;      
    public float dureeDeVie = 5f;     

    [Header("Zone d'apparition")]
    public float largeurZone = 5f;    
    public float profondeurZone = 5f; 
    public float intervalleTemps = 1f;

    private float timer;

    void Update()
    {
        SuivreLeJoueur();

        timer += Time.deltaTime;
        if (timer >= intervalleTemps)
        {
            FaireApparaitreObjet();
            timer = 0f;
        }
    }

    void SuivreLeJoueur()
    {
        if (joueur != null)
        {
            Vector3 nouvellePosition = new Vector3(joueur.position.x, joueur.position.y + hauteurAuDessus, joueur.position.z);
            transform.position = nouvellePosition;
        }
    }

    void FaireApparaitreObjet()
    {
        if (objetsAPop != null && objetsAPop.Length > 0)
        {
            Vector3 positionSpawn = transform.position;
            positionSpawn.x += Random.Range(-largeurZone, largeurZone);
            positionSpawn.z += Random.Range(-profondeurZone, profondeurZone);

            // 3. On appelle notre nouvelle fonction magique de probabilité
            GameObject objetChoisi = ChoisirObjetAuHasard();

            if (objetChoisi != null)
            {
                GameObject nouvelObjet = Instantiate(objetChoisi, positionSpawn, Quaternion.identity);
                Destroy(nouvelObjet, dureeDeVie);
            }
        }
    }


    GameObject ChoisirObjetAuHasard()
    {
        // On calcule le total de toutes les chances additionnées (ex: 50 + 30 + 20 = 100)
        float poidsTotal = 0f;
        foreach (ObjetSpawne obj in objetsAPop)
        {
            poidsTotal += obj.chanceDApparition;
        }

        // On tire un nombre au hasard entre 0 et ce total
        float tirageAuSort = Random.Range(0f, poidsTotal);
        float sommeActuelle = 0f;

        // On cherche dans quel "morceau" du camembert notre nombre est tombé
        foreach (ObjetSpawne obj in objetsAPop)
        {
            sommeActuelle += obj.chanceDApparition;
            if (tirageAuSort <= sommeActuelle)
            {
                return obj.prefab;
            }
        }

        return null; // Sécurité au cas où
    }
}