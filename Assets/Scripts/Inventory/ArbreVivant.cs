using System.Collections;
using UnityEngine;

public class ArbreVivant : MonoBehaviour
{
    [Header("Réglages de l'Arbre")]
    public InventoryItemData boisDonne; 
    public int quantiteParCoup = 1; 
    public int coupsAvantDestruction = 5; 

    [Header("Sons en séquence (À chaque coup)")]
    public AudioClip premierSonImpact;  
    public AudioClip deuxiemeSonImpact;
    public float delaiEntreLesSons = 0.3f; 

    [Header("Son final (Optionnel)")]
    public AudioClip sonChuteArbre; 

    public void RecevoirCoup(int degats)
    {
        coupsAvantDestruction -= degats;


        StartCoroutine(JouerSonsEnSequence());


        PlayerInventoryHolder inventaireJoueur = FindAnyObjectByType<PlayerInventoryHolder>();
        if (inventaireJoueur != null && boisDonne != null)
        {
            inventaireJoueur.AddToInventory(boisDonne, quantiteParCoup);
        }


        if (coupsAvantDestruction <= 0)
        {

            if (sonChuteArbre != null)
            {
                AudioSource.PlayClipAtPoint(sonChuteArbre, transform.position, 1f);
            }

            
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;
            if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
            
            Destroy(gameObject, 2f); 
        }
    }


    IEnumerator JouerSonsEnSequence()
    {

        if (premierSonImpact != null)
        {
            AudioSource.PlayClipAtPoint(premierSonImpact, transform.position, 1f);
        }


        yield return new WaitForSeconds(delaiEntreLesSons);

        // On joue le deuxième son
        if (deuxiemeSonImpact != null && coupsAvantDestruction > 0) 
        {
            AudioSource.PlayClipAtPoint(deuxiemeSonImpact, transform.position, 1f);
        }
    }
}