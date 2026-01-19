using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [Header("Réglages Navigation")]
    public float vitesse = 3f; // Vitesse de croisière
    public Vector3 direction = Vector3.forward; // Vers l'avant (Z)

    void Update()
    {
        // Le radeau avance en permanence dans la direction choisie
        transform.Translate(direction * vitesse * Time.deltaTime, Space.World);
    }
}