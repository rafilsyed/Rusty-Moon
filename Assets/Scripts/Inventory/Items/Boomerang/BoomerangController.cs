using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
[RequireComponent(typeof(Collider))]  
public class BoomerangController : MonoBehaviour
{
    [Header("Paramètres")]
    public Transform playerHand;
    public Transform playerCamera;
    public PlayerInventoryHolder playerInventory;
    
    [Tooltip("Glisse ici l'objet qui doit tourner (le Pivot ou le modèle)")]
    public Transform modelVisuel;

    [Header("Réglages Rotation")]
    public Vector3 axeDeRotation = new Vector3(0, 1, 0);
    public float rotationSpeed = 800f;

    [Header("Statistiques Vol")]
    public float speed = 15f;
    public float distance = 20f;
    public float sideArc = 5f;

    [Header("Sons")]
    public AudioClip throwSound;
    public AudioClip flightSound;
    public AudioClip catchSound;

    private AudioSource audioSource;
    private Rigidbody rb;
    private Collider col;

    private bool isThrown = false;
    private bool isReturning = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 curvePoint;
    private Vector3 returnCurvePoint;
    private float flightTime = 0f;
    

    public bool IsThrown => isThrown;

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;
        audioSource.loop = false;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; // Important pour que le boomerang suive le script et non la gravité

        col = GetComponent<Collider>();
        col.isTrigger = true; // Le boomerang doit traverser les objets, pas rebondir dessus

        // Tentative de trouver l'inventaire automatiquement si oublié dans l'inspecteur
        if (playerInventory == null && playerHand != null)
        {
            playerInventory = playerHand.GetComponentInParent<PlayerInventoryHolder>();
        }
    }

    void Update()
    {
        if (!isThrown)
        {
            transform.position = playerHand.position;
            transform.rotation = playerHand.rotation;
        }
        else
        {
            MoveBoomerang();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // On ne ramasse que si le boomerang est en vol
        if (!isThrown) return;

        // On vérifie si l'objet touché a le script ItemPickUp
        ItemPickUp item = other.GetComponent<ItemPickUp>();

        // Si c'est un item et qu'on a bien l'inventaire du joueur
        if (item != null && playerInventory != null)
        {
            // On essaie d'ajouter l'item à l'inventaire
            if (playerInventory.AddToInventory(item.ItemData, 1))
            {
                // JOUER LE SON : On joue le son de l'item via l'AudioSource du boomerang
                // (car l'item va être détruit immédiatement)
                if (item.pickUpSound != null)
                {
                    audioSource.PlayOneShot(item.pickUpSound);
                }

                // On détruit l'objet ramassé
                Destroy(other.gameObject);
            }
        }
    }

    public void ThrowBoomerang()
    {
        isThrown = true;
        isReturning = false;
        flightTime = 0f;
        startPosition = playerHand.position;
        targetPosition = playerCamera.position + (playerCamera.forward * distance);
        curvePoint = startPosition + (playerCamera.forward * (distance / 2)) + (playerCamera.right * sideArc);
        returnCurvePoint = startPosition + (playerCamera.forward * (distance / 2)) - (playerCamera.right * sideArc);

        if (throwSound != null)
            audioSource.PlayOneShot(throwSound);

        if (flightSound != null)
        {
            audioSource.clip = flightSound;
            audioSource.loop = true;
            audioSource.PlayDelayed(throwSound != null ? throwSound.length : 0f); 
        }
    }

    void MoveBoomerang()
    {
        if (modelVisuel != null)
            modelVisuel.Rotate(axeDeRotation * rotationSpeed * Time.deltaTime);

        flightTime += Time.deltaTime * speed / distance;

        if (!isReturning)
        {
            transform.position = CalculateBezierPoint(flightTime, startPosition, curvePoint, targetPosition);
            if (flightTime >= 1f)
            {
                isReturning = true;
                flightTime = 0f;
            }
        }
        else
        {
            transform.position = CalculateBezierPoint(flightTime, targetPosition, returnCurvePoint, playerHand.position);
            if (flightTime >= 1f || Vector3.Distance(transform.position, playerHand.position) < 0.5f)
                CatchBoomerang();
        }
    }

    void CatchBoomerang()
    {
        isThrown = false;
        isReturning = false;
        if (modelVisuel != null) modelVisuel.localRotation = Quaternion.identity;
        transform.rotation = playerHand.rotation;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        if (catchSound != null)
            audioSource.PlayOneShot(catchSound);
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
