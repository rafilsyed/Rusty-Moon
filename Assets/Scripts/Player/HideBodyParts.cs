using UnityEngine;

public class HideBodyParts : MonoBehaviour
{
    [Header("Glisse ici les os à cacher (Tête, Cou, Torse)")]
    public Transform[] bonesToHide;

    void LateUpdate()
    {
        foreach (Transform bone in bonesToHide)
        {
            if (bone != null)
            {
                bone.localScale = Vector3.zero;
            }
        }
    }
}