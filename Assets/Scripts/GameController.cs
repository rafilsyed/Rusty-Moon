using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void JoinGame()
    {
        SceneManager.LoadScene("Scene2");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
