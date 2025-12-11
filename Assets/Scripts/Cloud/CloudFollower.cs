using UnityEngine;

public class CloudFollower : MonoBehaviour
{
    public Transform player; // Glisse ton Player ici
    public float hauteurNuages = -5f; // La hauteur Y de ta mer de nuages

    void Update()
    {
        if (player != null)
        {
            // On se place à la position X et Z du joueur, mais on garde notre hauteur Y fixe
            transform.position = new Vector3(player.position.x, hauteurNuages, player.position.z);
        }
    }
}