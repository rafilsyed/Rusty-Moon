using UnityEngine;
using UnityEngine.InputSystem; // Requis pour lire les touches A et E

public class SailController : MonoBehaviour
{
    [Header("Réglages de la Voile")]
    public float vitesseRotation = 45f; // Vitesse à laquelle la voile tourne

    private bool joueurProche = false;

    void Update()
    {
        // On ne vérifie les touches que si le joueur est à côté de la voile
        if (joueurProche)
        {
            // isPressed permet de tourner en continu tant que la touche est maintenue
            if (Keyboard.current.fKey.isPressed)
            {
                // Tourne dans le sens horaire (vers la droite)
                transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);
            }
            
            if (Keyboard.current.qKey.isPressed)
            {
                // Tourne dans le sens anti-horaire (vers la gauche)
                transform.Rotate(Vector3.down * vitesseRotation * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurProche = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            joueurProche = false;
        }
    }
}