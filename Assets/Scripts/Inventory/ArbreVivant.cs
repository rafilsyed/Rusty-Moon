using UnityEngine;

public class ArbreVivant : MonoBehaviour
{
    [Header("Réglages de l'Arbre")]
    public InventoryItemData boisDonne;
    public int quantiteParCoup = 1;
    public int coupsAvantDestruction = 5;
    public void RecevoirCoup(int degats)
    {
        coupsAvantDestruction -= degats;

        PlayerInventoryHolder inventaireJoueur = FindFirstObjectByType<PlayerInventoryHolder>();
        if (inventaireJoueur != null && boisDonne != null)
        {
            inventaireJoueur.AddToInventory(boisDonne, quantiteParCoup);
        }

        if (coupsAvantDestruction <= 0)
        {
            Destroy(gameObject);
        }
    }
}