using UnityEngine;
using System.Collections;

public class disable_cloud_floater : MonoBehaviour
{
    [Header("Réglages")]
    public float tempsAvantArret = 5.0f;

    [Header("Cibles")]
    // Ici, au lieu de demander un GameObject, on demande spécifiquement le script "CloudFloater"
    public CloudFloater leScriptADesactiver; 

    void Start()
    {
        if (leScriptADesactiver != null)
        {
            StartCoroutine(CompteARebours());
        }
        else
        {
            Debug.LogError("Attention : Tu as oublié de glisser le script dans l'inspecteur !");
        }
    }

    IEnumerator CompteARebours()
    {
        // 1. On attend
        yield return new WaitForSeconds(tempsAvantArret);

        // 2. On accède au script cible et on le désactive
        // C'est comme si on décochait la case dans l'inspecteur à distance
        leScriptADesactiver.enabled = false; 
    }
}