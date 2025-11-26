using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    [Header("Paramètres")]
    public Transform playerHand; 
    public Transform playerCamera;

    [Header("Statistiques")]
    public float speed = 15f;
    public float rotationSpeed = 800f;
    public float distance = 20f;
    public float sideArc = 5f; // Écart sur le côté

    // États internes
    private bool isThrown = false;
    private bool isReturning = false;
    
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 curvePoint;      // Point de courbure pour l'ALLER
    private Vector3 returnCurvePoint; // Point de courbure pour le RETOUR
    private float flightTime = 0f;

    void Update()
    {
        if (!isThrown)
        {
            transform.position = playerHand.position;
            transform.rotation = playerHand.rotation;

            if (Input.GetButtonDown("Fire1"))
            {
                ThrowBoomerang();
            }
        }
        else
        {
            MoveBoomerang();
        }
    }

    void ThrowBoomerang()
    {
        isThrown = true;
        isReturning = false;
        flightTime = 0f;

        startPosition = playerHand.position;
        
        // Point cible (bout de la course)
        targetPosition = playerCamera.position + (playerCamera.forward * distance);

        // Point de courbure ALLER (à droite par exemple)
        curvePoint = startPosition + (playerCamera.forward * (distance / 2)) + (playerCamera.right * sideArc);
        
        // Point de courbure RETOUR 
        // On le calcule dès maintenant pour définir la forme globale de la boucle
        returnCurvePoint = startPosition + (playerCamera.forward * (distance / 2)) - (playerCamera.right * sideArc);
    }

    void MoveBoomerang()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // On augmente le temps de vol
        flightTime += Time.deltaTime * speed / distance;

        if (!isReturning)
        {
            // --- PHASE ALLER ---
            // Courbe de Bézier : Départ / curvePoint / Cible
            transform.position = CalculateBezierPoint(flightTime, startPosition, curvePoint, targetPosition);

            if (flightTime >= 1f)
            {
                isReturning = true;
                flightTime = 0f; // On reset le temps pour le retour
            }
        }
        else
        {
   
            
            transform.position = CalculateBezierPoint(flightTime, targetPosition, returnCurvePoint, playerHand.position);

            // Condition de fin (proche de la main ou temps écoulé)
            if (flightTime >= 1f || Vector3.Distance(transform.position, playerHand.position) < 0.5f)
            {
                CatchBoomerang();
            }
        }
    }

    void CatchBoomerang()
    {
        isThrown = false;
        isReturning = false;
        transform.rotation = playerHand.rotation;
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
  
        
        // On limite t entre 0 et 1 pour éviter les bugs
        t = Mathf.Clamp01(t);

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
}