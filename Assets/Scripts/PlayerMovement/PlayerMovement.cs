using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    
    [Header("Animation")] 
    public Animator animator; // Glisse ton objet "Zaun_Character" ici

    [Header("Réglages Mouvement")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    // Toggle du curseur
    private bool cursorVisible = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ----- Toggle curseur avec C -----
        if (Input.GetKeyDown(KeyCode.C))
        {
            cursorVisible = !cursorVisible;

            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;

            canMove = !cursorVisible;
            
            if (!canMove && animator != null) animator.SetFloat("Vitesse", 0f);
        }

        // Empêche le mouvement si curseur visible
        if (!canMove)
            return;

        // ----- Mouvements -----
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputHorizontal = Input.GetAxis("Horizontal"); 
        float inputVertical = Input.GetAxis("Vertical");     

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * inputVertical;
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * inputHorizontal;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (animator != null)
        {
           
            float animationSpeed = new Vector2(inputHorizontal, inputVertical).magnitude;
            animator.SetFloat("Vitesse", animationSpeed);

           
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Lancer");
            }
        }
        // ---------------------------------------------------------

        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // ----- Accroupissement -----
        if (Input.GetKey(KeyCode.R))
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // ----- Rotation caméra + joueur -----
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}