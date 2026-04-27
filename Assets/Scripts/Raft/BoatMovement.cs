using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [Header("Réglages Navigation")]
    public float vitesse = 3f; // Vitesse de croisière
    public Vector3 direction = Vector3.forward; // Vers l'avant (Z)
    private Rigidbody rb;

    void Start()
    {
        // 2. Aller chercher le composant Rigidbody attaché au bateau
        rb = GetComponent<Rigidbody>();

        // Sécurité au cas où tu aurais oublié d'ajouter un Rigidbody sur l'objet
        if (rb == null)
        {
            Debug.LogError("Il manque un Rigidbody sur le bateau !");
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Maintenant 'rb' est connu et le mouvement fonctionnera physiquement
        rb.MovePosition(rb.position + direction * vitesse * Time.fixedDeltaTime);
    }
}