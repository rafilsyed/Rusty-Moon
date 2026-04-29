using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    public AudioClip deathMusic;

    private void Start()
    {
        Debug.Log("1. Script lancé");
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("2. ERREUR : Il manque un composant AudioSource sur cet objet !");
            return;
        }

        if (deathMusic == null)
        {
            Debug.LogError("2. ERREUR : Tu as oublié de glisser le son dans la case Death Music !");
            return;
        }

        audioSource.clip = deathMusic;
        audioSource.Play();
        Debug.Log("3. Le son devrait jouer maintenant : " + deathMusic.name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}