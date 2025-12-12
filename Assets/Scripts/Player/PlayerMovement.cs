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

        float curSpeedX = (isRunning ? runSpeed : walkSpeed) * inputVertical;
        float curSpeedY = (isRunning ? runSpeed : walkSpeed) * inputHorizontal;
        float movementDirectionY = moveDirection.y;

        moveDirection = canMove ?
            (forward * curSpeedX) + (right * curSpeedY) :
            new Vector3(0, movementDirectionY, 0);

        if (canMove && Input.GetButton("Jump") && characterController.isGrounded)
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

        if (canMove && Input.GetKey(KeyCode.R))
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

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
