using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm.isPaused)
            gm.Unpause();
        SceneManager.LoadScene("Main Menu");
    }
}
