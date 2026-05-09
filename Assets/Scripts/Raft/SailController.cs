using UnityEngine;
using UnityEngine.InputSystem; 

public class SailController : MonoBehaviour
{
    [Header("Réglages de la Voile")]
    public float vitesseRotation = 45f; 

    private bool joueurProche = false;
    private RaftController raftController; // NOUVEAU : Référence au radeau

    void Start()
    {
        // Au démarrage, on cherche le script du radeau qui est sur l'objet Parent
        raftController = GetComponentInParent<RaftController>();
    }

    void Update()
    {
        // On ne vérifie les touches que si le joueur est dans le Trigger
        if (joueurProche)
        {
            if (Keyboard.current.fKey.isPressed)
            {
                transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime);
            }
            
            if (Keyboard.current.qKey.isPressed)
            {
                transform.Rotate(Vector3.down * vitesseRotation * Time.deltaTime);
            }

            // 👇 NOUVEAU : On gère l'ouverture/fermeture de la voile ICI
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                if (raftController != null)
                {
                    // On donne l'ordre au radeau de changer l'état de la voile
                    raftController.ToggleVoile();
                }
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