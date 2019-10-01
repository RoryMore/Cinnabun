using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;

    bool isPaused;

    public void OnClickResume()
    {
        onPaused();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPaused();
        }    
    }

    public void showPaused()
    {

    }

    public void hidePaused()
    {

    }

    public void onPaused()
    {       
        if (isPaused == false)
        {
            Time.timeScale = 0;
            isPaused = true;
            Debug.Log("IsPaused");
            //showPaused();
        }
        else if (isPaused == true)
        {
            Time.timeScale = 1;
            Debug.Log("IsResumed");
            isPaused = false;
            //hidePaused();
        }
    }
}
