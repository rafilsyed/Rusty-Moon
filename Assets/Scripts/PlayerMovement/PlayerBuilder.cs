using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    [Header("Réglages")]
    public GameObject raftPrefab;
    public float distanceConstruction = 15f;
    public KeyCode toucheConstruction = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(toucheConstruction))
        {
            RegarderEtConstruire();
        }
    }

    void RegarderEtConstruire()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

       
            if (Physics.Raycast(ray, out hit, distanceConstruction, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.name.Contains("SnapPoint"))
            {
            
                Vector3 futurePosition = hit.collider.transform.position;

                if (Physics.CheckSphere(futurePosition, 0.4f, -1, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log("Impossible : Il y a déjà un radeau ici !");
                
                    Destroy(hit.collider.gameObject); 
                    return; 

                }
                Rigidbody mainRaft = hit.collider.GetComponentInParent<Rigidbody>();

                if (mainRaft != null)
                {
                    GameObject nouveauMorceau = Instantiate(raftPrefab, hit.collider.transform.position, hit.collider.transform.rotation);
                    
                    nouveauMorceau.transform.SetParent(mainRaft.transform);
                    Destroy(nouveauMorceau.GetComponent<Rigidbody>());
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}