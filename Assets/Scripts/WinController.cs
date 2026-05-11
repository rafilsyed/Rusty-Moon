using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class WinController : MonoBehaviour
{
    public GameObject player;
    public GameObject endIsland;
    public AudioClip winSound;
    private static readonly WaitForSeconds returnDelay = new(5f);
    private bool hasWon = false;
    private float timer;

    public void Update()
    {
        timer += Time.deltaTime;

        if (hasWon) return;
        if (player != null && endIsland != null)
        {
            Collider playerCol = player.GetComponent<Collider>();
            Collider islandCol = endIsland.GetComponent<Collider>();

            if (playerCol != null && islandCol != null)
            {
                if (playerCol.bounds.Intersects(islandCol.bounds))
                {
                    OnWin();
                }
            }
            else
            {
                if (Vector3.Distance(player.transform.position, endIsland.transform.position) < 1f)
                {
                    OnWin();
                }
            }
        }
    }

    private void OnWin()
    {
        if (hasWon) return;
        if (player != null && winSound != null)
        {
            AudioSource audioSource = player.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = player.AddComponent<AudioSource>();
            }

            audioSource.clip = winSound;
            audioSource.volume = 1f;
            audioSource.spatialBlend = 1f;
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();
        }
        StartCoroutine(LoadMainMenuAfterDelay());
        hasWon = true;
    }

    private IEnumerator LoadMainMenuAfterDelay()
    {
        yield return returnDelay;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
