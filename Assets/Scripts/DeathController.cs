using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    public AudioClip deathMusic;

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && deathMusic != null)
        {
            audioSource.clip = deathMusic;
            audioSource.Play();
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}