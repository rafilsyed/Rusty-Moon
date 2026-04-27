using UnityEngine;
using UnityEngine.Events;

public class RessourceNode : MonoBehaviour, IInteractable
{
    [Header("Réglages des ressource")]
    public InventoryItemData itemARamasser;
    public int quantiteParRecolte = 2;

    [Header("Durabilité")]
    public int nombreDeRecoltesRestantes = 3; 

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        PlayerInventoryHolder inventaireJoueur = interactor.GetComponent<PlayerInventoryHolder>();

        if (inventaireJoueur != null && itemARamasser != null)
        {

            bool aEteAjoute = inventaireJoueur.AddToInventory(itemARamasser, quantiteParRecolte);

            if (aEteAjoute)
            {
                interactSuccessful = true;
                nombreDeRecoltesRestantes--;


                if (nombreDeRecoltesRestantes <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                interactSuccessful = false;
            }
        }
        else
        {
            interactSuccessful = false;
        }
    }

    public void EndInteraction()
    {
    }
}