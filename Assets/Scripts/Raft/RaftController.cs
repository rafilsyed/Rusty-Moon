using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RaftController : MonoBehaviour
{
    [Header("Références")]
    [Tooltip("Glisse ici l'objet Pivot_Voile")]
    public Transform voile; 
    public GameObject visuelVoile; 

    [Header("Réglages de Navigation")]
    public float forceDuVent = 10f; 
    public bool voileActive = false; 

    private Rigidbody rbRaft;

    void Start()
    {
        rbRaft = GetComponent<Rigidbody>();

        if (visuelVoile != null)
        {
            visuelVoile.SetActive(voileActive);
        }
    }

    
    public void ToggleVoile()
    {
        voileActive = !voileActive;

        if (visuelVoile != null)
        {
            visuelVoile.SetActive(voileActive);
        }
    }

    void FixedUpdate() 
    {
        if (voile != null && voileActive)
        {
            Vector3 directionPoussee = voile.forward;
            directionPoussee.y = 0f;
            directionPoussee.Normalize();

            rbRaft.AddForce(directionPoussee * forceDuVent, ForceMode.Acceleration);
        }
    }
}