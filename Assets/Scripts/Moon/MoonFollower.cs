using UnityEngine;

public class MoonFollower : MonoBehaviour
{
    
    [Header("Cible à suivre")]
    public Transform player;
    public float hauteur_lune = 100f; 
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, hauteur_lune, player.position.z);
        }
    }

}