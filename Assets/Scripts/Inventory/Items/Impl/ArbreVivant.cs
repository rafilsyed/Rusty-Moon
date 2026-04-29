using UnityEngine;

public class ArbreVivant : MonoBehaviour
{
    [Header("Réglages de l'Arbre")]
    public InventoryItemData boisDonne; // Glisse ici ton ScriptableObject "Plank"
    public int quantiteParCoup = 1; // Combien de planches tu gagnes à chaque coup de hache
    public int coupsAvantDestruction = 5; // Combien de fois il faut frapper pour le casser

    // Fonction appelée par la hache quand on clique dessus
    public void RecevoirCoup(int degats)
    {
        coupsAvantDestruction -= degats;

        // On cherche l'inventaire du joueur pour lui donner le bois
        PlayerInventoryHolder inventaireJoueur = FindObjectOfType<PlayerInventoryHolder>();

        if (inventaireJoueur != null && boisDonne != null)
        {
            // On ajoute le bois à l'inventaire
            inventaireJoueur.AddToInventory(boisDonne, quantiteParCoup);
        }

        // Si l'arbre n'a plus de vie, il disparaît
        if (coupsAvantDestruction <= 0)
        {
            Destroy(gameObject);
        }
    }
}