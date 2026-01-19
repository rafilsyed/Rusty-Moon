using UnityEngine;

public class RaftPassenger : MonoBehaviour
{
    private Transform playerTransform;
    private CharacterController playerController;     private Vector3 lastRaftPosition;

    void Start()
    {
 
        lastRaftPosition = transform.position;
    }

    void LateUpdate()
    {
        Vector3 movement = transform.position - lastRaftPosition;

        if (playerTransform != null)
        {
            if (playerController != null)
            {
                playerController.Move(movement);
            }
            else
            {
                playerTransform.position += movement;
            }
        }

        lastRaftPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerController = other.GetComponent<CharacterController>(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
            playerController = null;
        }
    }
}