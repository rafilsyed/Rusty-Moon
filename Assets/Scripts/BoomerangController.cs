using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    [Header("Paramètres")]
    public Transform playerHand;
    public Transform playerCamera;
    
    [Tooltip("Glisse ici l'objet qui doit tourner (le Pivot ou le modèle)")]
    public Transform modelVisuel;

    [Header("Réglages Rotation")]
    [Tooltip("Autour de quel axe ça tourne ? (1,0,0)=X, (0,1,0)=Y, (0,0,1)=Z")]
    public Vector3 axeDeRotation = new Vector3(0, 1, 0); // Par défaut Y
    public float rotationSpeed = 800f;

    [Header("Statistiques Vol")]
    public float speed = 15f;
    public float distance = 20f;
    public float sideArc = 5f;

    // États internes
    private bool isThrown = false;
    private bool isReturning = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 curvePoint;
    private Vector3 returnCurvePoint;
    private float flightTime = 0f;

    void Update()
    {
        if (!isThrown)
        {
            transform.position = playerHand.position;
            transform.rotation = playerHand.rotation;

            if (Input.GetButtonDown("Fire1")) ThrowBoomerang();
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
        targetPosition = playerCamera.position + (playerCamera.forward * distance);
        curvePoint = startPosition + (playerCamera.forward * (distance / 2)) + (playerCamera.right * sideArc);
        returnCurvePoint = startPosition + (playerCamera.forward * (distance / 2)) - (playerCamera.right * sideArc);
    }

    void MoveBoomerang()
    {
        // --- C'EST ICI QUE LA MAGIE OPÈRE ---
        // On utilise ta variable 'axeDeRotation' pour définir le sens
        if (modelVisuel != null)
        {
            modelVisuel.Rotate(axeDeRotation * rotationSpeed * Time.deltaTime);
        }
        // ------------------------------------

        flightTime += Time.deltaTime * speed / distance;
        
        if (!isReturning)
        {
            transform.position = CalculateBezierPoint(flightTime, startPosition, curvePoint, targetPosition);
            if (flightTime >= 1f) { isReturning = true; flightTime = 0f; }
        }
        else
        {
            transform.position = CalculateBezierPoint(flightTime, targetPosition, returnCurvePoint, playerHand.position);
            if (flightTime >= 1f || Vector3.Distance(transform.position, playerHand.position) < 0.5f) CatchBoomerang();
        }
    }

    void CatchBoomerang()
    {
        isThrown = false;
        isReturning = false;
        if (modelVisuel != null) modelVisuel.localRotation = Quaternion.identity;
        transform.rotation = playerHand.rotation;
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        t = Mathf.Clamp01(t);
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}