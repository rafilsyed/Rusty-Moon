using UnityEngine;

public class moon_follower : MonoBehaviour
{   //La lune suit les positions X et Z du joueur (la hauteur ne change pas)
    
    [Header("Cible à suivre")]
    public Transform player;          // input du personnage !
    public float hauteurlune = 100f; // hauteur de la lune
    void Update()
    {
        if (player != null)
        {
            // On se place à la position X et Z du joueur, mais on garde notre hauteur Y fixe
            transform.position = new Vector3(player.position.x, hauteurlune, player.position.z);
        }
    }

}