using UnityEngine;
using UnityEngine.UI;
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
        if (gm != null && gm.isPaused)
            gm.Unpause();
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadEndScreen()
    {
        SceneManager.LoadScene("End Screen");
    }
}
