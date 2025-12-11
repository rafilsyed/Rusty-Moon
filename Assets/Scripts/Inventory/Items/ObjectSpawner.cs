using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform joueur;          // Glisse ton personnage ici !
    public float hauteurAuDessus = 10f; // A quelle hauteur au-dessus de la tête il se place

    [Header("Paramètres de l'objet")]
    public GameObject objetAPop;      
    public float dureeDeVie = 5f;     

    [Header("Zone d'apparition")]
    public float largeurZone = 5f;    // Axe X
    public float profondeurZone = 5f; // Axe Z
    public float intervalleTemps = 1f;

    private float timer;

    void Update()
    {
        // 1. D'abord, on déplace le Spawner au-dessus du joueur
        SuivreLeJoueur();

        // 2. Ensuite, on gère le timer pour faire apparaître les objets
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
            // On se place à la position du joueur + la hauteur souhaitée
            // On garde X et Z du joueur, mais on change Y
            Vector3 nouvellePosition = new Vector3(joueur.position.x, joueur.position.y + hauteurAuDessus, joueur.position.z);
            
            // On applique la position au Spawner
            transform.position = nouvellePosition;
        }
    }

    void FaireApparaitreObjet()
    {
        // On part de la position actuelle du Spawner (qui est déjà au-dessus du joueur)
        Vector3 positionSpawn = transform.position;

        // On ajoute un décalage aléatoire en X et Z
        positionSpawn.x += Random.Range(-largeurZone, largeurZone);
        positionSpawn.z += Random.Range(-profondeurZone, profondeurZone);

        // Création de l'objet
        if (objetAPop != null)
        {
            GameObject nouvelObjet = Instantiate(objetAPop, positionSpawn, Quaternion.identity);
            Destroy(nouvelObjet, dureeDeVie);
        }
    }
}