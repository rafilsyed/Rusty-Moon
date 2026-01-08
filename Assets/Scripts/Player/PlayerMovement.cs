using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;

    [Header("Animation")]
    public Animator animator;

    [Header("Réglages Mouvement")]
    public float moveSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    // --- NOUVEAU : Variable pour savoir si on est dans le nuage ---
    [HideInInspector] 
    public bool isTrapped = false;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. GESTION DE LA CAMÉRA (Toujours active même dans le nuage)
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // 2. GESTION DU MOUVEMENT (Désactivée si on est dans le nuage)
        if (!isTrapped)
        {
            HandleNormalMovement();
        }
        else
        {
            // Si on est piégé, on coupe l'animation de course
            if(animator != null) animator.SetBool("IsRunning", false);
            // On s'assure que la vélocité interne ne s'accumule pas bizarrement
            moveDirection = Vector3.zero; 
        }
    }

    void HandleNormalMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputHorizontal = 0f;
        float inputVertical = 0f;
        bool isRunning = false;

        if (canMove)
        {
            inputHorizontal = Input.GetAxis("Horizontal");
            inputVertical = Input.GetAxis("Vertical");
            isRunning = Input.GetKey(KeyCode.LeftShift);

            if (animator != null)
            {
                bool playerIsMoving = (inputHorizontal != 0 || inputVertical != 0);
                animator.SetBool("IsRunning", playerIsMoving);

                if (Input.GetMouseButtonDown(0))
                {
                    animator.SetTrigger("Lancer");
                }
            }
        }

        // Gestion Crouch vs Course
        if (canMove && Input.GetKey(KeyCode.LeftShift))
        {
            characterController.height = crouchHeight;
            // On applique la vitesse directement ici sans modifier les variables globales
            float curSpeedX = crouchSpeed * inputVertical;
            float curSpeedY = crouchSpeed * inputHorizontal;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            moveDirection.y = movementDirectionY;
        }
        else
        {
            characterController.height = defaultHeight;
            float curSpeedX = (isRunning ? runSpeed : moveSpeed) * inputVertical;
            float curSpeedY = (isRunning ? runSpeed : moveSpeed) * inputHorizontal;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            moveDirection.y = movementDirectionY;
        }

        // Saut
        if (canMove && Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }

        // Gravité
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
}