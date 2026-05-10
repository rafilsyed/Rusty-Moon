using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Axe Item")]
public class AxeItemData : InventoryItemData
{
    [Header("Réglages de la Hache")]
    public float range = 3f; 
    public int degatsDeCoupe = 1; 
    
    [Header("Son dans le vide")]
    public AudioClip swingSound;

    [Header("Temps de recharge (Cooldown)")]
    public float tempsEntreChaqueCoup = 1.2f; 
    private float tempsDuProchainCoup = 0f;  

    public override bool UseItem()
    {
        return false;
    }

    public override bool Attack()
    {
    
        if (Time.time < tempsDuProchainCoup)
        {
            return false; 
        }


        tempsDuProchainCoup = Time.time + tempsEntreChaqueCoup;



        Camera cam = Camera.main;
        if (cam != null)
        {
            Animator itemAnimator = cam.GetComponentInChildren<Animator>();
            if (itemAnimator != null)
            {
                itemAnimator.SetTrigger("Attack");
            }
        }

        if (swingSound != null && cam != null)
        {
            AudioSource.PlayClipAtPoint(swingSound, cam.transform.position, 0.5f);
        }


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