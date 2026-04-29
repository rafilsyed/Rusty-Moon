using UnityEngine;

public class BaitBehavior : MonoBehaviour
{
    [Header("Réglages du Freinage")]
    public float forceDeFreinageHorizontal = 2f; // Freine fort sur X et Z
    public float forceDeFreinageVertical = 0.5f; // Freine un peu sur Y (effet parachute/flotteur)
    public float delaiAvantFreinage = 0.5f; 

    private Rigidbody rb;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() 
    {
        if (rb == null || rb.isKinematic) return;

        timer += Time.fixedDeltaTime;

        if (timer > delaiAvantFreinage)
        {
            Vector3 velociteActuelle = rb.linearVelocity;

            // 1. On calcule le freinage horizontal (qui vise 0 très vite)
            float targetX = Mathf.Lerp(velociteActuelle.x, 0f, Time.fixedDeltaTime * forceDeFreinageHorizontal);
            float targetZ = Mathf.Lerp(velociteActuelle.z, 0f, Time.fixedDeltaTime * forceDeFreinageHorizontal);

            // 2. On calcule le freinage vertical (qui vise 0 doucement)
            // Comme la gravité d'Unity continue de pousser vers le bas en même temps, 
            // ça ne l'arrêtera pas, ça va juste créer une "vitesse de chute maximale" très douce.
            float targetY = Mathf.Lerp(velociteActuelle.y, 0f, Time.fixedDeltaTime * forceDeFreinageVertical);

            // 3. On applique la nouvelle vélocité
            rb.linearVelocity = new Vector3(targetX, targetY, targetZ);

            // Optionnel : On s'assure qu'il s'arrête net horizontalement à la fin
            if (Mathf.Abs(rb.linearVelocity.x) < 0.05f && Mathf.Abs(rb.linearVelocity.z) < 0.05f)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }
    }
}