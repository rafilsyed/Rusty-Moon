using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Axe Item")]
public class AxeItemData : InventoryItemData
{
    [Header("Réglages de la Hache")]
    public float range = 3f; // La distance max pour toucher l'arbre
    public int degatsDeCoupe = 1; // Combien de "coups" ça enlève à l'arbre
    
    [Header("Son de la Hache (Optionnel)")]
    public AudioClip swingSound;

    public override bool UseItem()
    {
        return false;
    }

    public override bool Attack()
    {

        if (swingSound != null)
        {
            AudioSource.PlayClipAtPoint(swingSound, Camera.main.transform.position, 1f);
        }

  
        Camera cam = Camera.main;
        if (cam == null) return false;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, range))
        {

            ArbreVivant arbre = hit.collider.GetComponent<ArbreVivant>();
            
            if (arbre != null)
            {

                arbre.RecevoirCoup(degatsDeCoupe);
                return true;
            }
        }

        return false;
    }
}