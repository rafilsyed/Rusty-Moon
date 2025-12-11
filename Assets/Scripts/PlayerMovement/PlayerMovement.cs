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
        if (!canMove)
            return;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * inputVertical;
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * inputHorizontal;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // --- DEBUT DE LA MODIFICATION ---
        if (animator != null)
        {
            // Calcule si on bouge (Vrai si on appuie sur Z,Q,S ou D)
            bool playerIsMoving = (inputHorizontal != 0 || inputVertical != 0);
            
            // Active l'animation si on bouge
            animator.SetBool("IsRunning", playerIsMoving);

            // (Optionnel) Garde ta logique existante si tu veux l'utiliser plus tard
            float animationSpeed = new Vector2(inputHorizontal, inputVertical).magnitude;
            animator.SetFloat("Vitesse", animationSpeed);

            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Lancer");
            }
        }
        // --- FIN DE LA MODIFICATION ---

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

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}