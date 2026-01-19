using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform interactPoint;
    public LayerMask interactableLayer;
    public float interactPointRadius = 1f;
    public static bool IsInteracting { get; set; }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            var colliders = Physics.OverlapSphere(interactPoint.position, interactPointRadius, interactableLayer);
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    var interactable = colliders[i].GetComponent<IInteractable>();

                    if (interactable != null) StartInteraction(interactable);
                }
            }
        }

        if(Keyboard.current.tabKey.wasPressedThisFrame && IsInteracting)
        {
            EndInteraction();
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    void EndInteraction()
    {
        IsInteracting = false;
    }
}
