using UnityEngine;

public class CloudFloater : MonoBehaviour
{
    [Header("Réglages Nuages")]
    public float niveauDesNuages = 0f;
    public float forceDeFlottaison = 15f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogWarning($"CloudFloater sur {name} n’a pas de Rigidbody, script désactivé.");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        if (this == null) return;

        if (transform.position.y < niveauDesNuages)
        {
            float forceUp = (niveauDesNuages - transform.position.y) * forceDeFlottaison;
            rb.AddForce(Vector3.up * forceUp, ForceMode.Acceleration);
        }
    }
}
