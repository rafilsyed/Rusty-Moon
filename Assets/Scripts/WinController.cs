using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinController : MonoBehaviour
{
    public GameObject player;
    public GameObject endIsland;
    public AudioClip winSound;
    private static readonly WaitForSeconds returnDelay = new WaitForSeconds(5f);
    private bool hasWon = false;

    public void Update()
    {
        if(hasWon) return;
        if (player != null && endIsland != null)
        {
            Collider playerCol = player.GetComponent<Collider>();
            Collider islandCol = endIsland.GetComponent<Collider>();

            if (playerCol != null && islandCol != null)
            {
                if (playerCol.bounds.Intersects(islandCol.bounds))
                {
                    AudioSource.PlayClipAtPoint(winSound, player.transform.position, 5f);
                    hasWon = true;
                    StartCoroutine(LoadMainMenuAfterDelay());
                }
            }
            else
            {
                if (Vector3.Distance(player.transform.position, endIsland.transform.position) < 1f)
                {
                    AudioSource.PlayClipAtPoint(winSound, player.transform.position, 5f);
                    hasWon = true;
                    StartCoroutine(LoadMainMenuAfterDelay());
                }
            }
        }
    }

    private IEnumerator LoadMainMenuAfterDelay()
    {
        yield return returnDelay;
        SceneManager.LoadScene("MainMenu");
    }
}
