using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;
    public void JoinGame()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        SceneManager.LoadScene("Scene2");
    }
    
    public void ExitGame()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        Application.Quit();
    }
}
