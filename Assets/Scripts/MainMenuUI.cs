using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;

    void OnClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
