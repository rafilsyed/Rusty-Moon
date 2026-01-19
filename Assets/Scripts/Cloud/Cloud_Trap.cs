using UnityEngine;

public class Cloud_Trap : MonoBehaviour
{
    [Header("Réglages Nuage")]
    public float sinkingSpeed = 2f;      // Vitesse d'enfoncement constant
    public float jumpEscapeForce = 5f;   // Force d'un saut
    public float cloudGravity = 3f;      // Gravité interne (douce)
    public float moveSpeedInCloud = 2f;  // Vitesse de déplacement lente

    [Header("Réglages Dégâts")]
    public float Damageincloud = 1f;     // Quantité de dégâts
    public float damageInterval = 1.0f;  // Dégâts toutes les X secondes

    private PlayerMovement playerScript;
    
    private PlayerHealth PVjoueur; 
    private CharacterController cc;
    private float verticalVelocity = 0f;
    private bool isInCloud = false;

    // Timer pour gérer la fréquence des dégâts
    private float damageTimer = 0f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<PlayerMovement>();
            cc = other.GetComponent<CharacterController>();

            if (playerScript != null)
            {
                isInCloud = true;
                playerScript.isTrapped = true; // On prend le contrôle !
                verticalVelocity = 0f;
                PVjoueur = other.GetComponent<PlayerHealth>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerScript != null)
        {
            isInCloud = false;
            playerScript.isTrapped = false; // On rend le contrôle
            playerScript = null;
            cc = null;
        }
    }

    private void Update()
    {
        if (isInCloud && cc != null)
        {
            HandleCloudPhysics();
            HandleCloudDamage();
        }
    }

    private void HandleCloudDamage()
    {
        // On augmente le timer avec le temps qui passe
        damageTimer += Time.deltaTime;

        // Si le timer dépasse l'intervalle défini 
        if (damageTimer >= damageInterval)
        {
            ApplyDamageToPlayer();
            damageTimer = 0f; 
        }
    }

    private void ApplyDamageToPlayer()
    {        
        PVjoueur.TakeDamage(Damageincloud);
    }
        

    private void HandleCloudPhysics()
    {
        // 1. Calcul des entrées mouvement (mais ralenti)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        // Note: transform ici fait référence au nuage, il faut utiliser la direction du joueur
        // Correction : Utiliser la direction relative au joueur
        Vector3 playerForward = playerScript.transform.forward;
        Vector3 playerRight = playerScript.transform.right;
        
        Vector3 moveDir = (playerForward * z + playerRight * x).normalized;

        // 2. Gestion Verticale (Lutte)
        
        // On coule tout le temps
        verticalVelocity -= sinkingSpeed * Time.deltaTime;

        // On spamme Espace pour remonter
        if (Input.GetButtonDown("Jump"))
        {
            verticalVelocity += jumpEscapeForce;
        }

        // On applique une petite gravité pour ne pas s'envoler à l'infini
        // On simule une résistance de l'air (Drag)
        verticalVelocity = Mathf.Lerp(verticalVelocity, -sinkingSpeed, Time.deltaTime * cloudGravity);

        // 3. Application du mouvement final
        Vector3 finalVelocity = moveDir * moveSpeedInCloud;
        finalVelocity.y = verticalVelocity;

        cc.Move(finalVelocity * Time.deltaTime);
    }
}