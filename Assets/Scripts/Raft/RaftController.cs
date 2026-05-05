using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Force Unity à ajouter un Rigidbody s'il n'y en a pas
public class RaftController : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Glisse ici l'objet Voile (l'enfant du radeau)")]
    public Transform voile; 

    [Header("Réglages de Navigation")]
    public float forceDuVent = 10f; // La puissance qui pousse le radeau

    private Rigidbody rbRaft;

    void Start()
    {
        rbRaft = GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        // On utilise FixedUpdate car on manipule la physique (Rigidbody)
        if (voile != null)
        {
            // 1. On récupère la direction vers laquelle la voile pointe
            Vector3 directionPoussee = voile.forward;

            // 2. SÉCURITÉ : On met le Y à zéro pour que le vent ne fasse pas s'envoler ou couler le radeau !
            directionPoussee.y = 0f;
            
            // On normalise pour que la force soit toujours égale, peu importe l'inclinaison de la voile
            directionPoussee.Normalize();

            // 3. On pousse le radeau !
            // On utilise ForceMode.Acceleration pour que le poids du radeau n'impacte pas trop la vitesse
            rbRaft.AddForce(directionPoussee * forceDuVent, ForceMode.Acceleration);
        }
    }
}