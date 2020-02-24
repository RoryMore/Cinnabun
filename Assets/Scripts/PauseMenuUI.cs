using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{

    GameObject[] pauseObjects;
    PauseAbility pauseAbility;
    public GameObject VHSimage;

    public bool isPaused;

    void Start()
    {
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        pauseAbility = FindObjectOfType<PauseAbility>();
        hidePaused();
    }

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

    public void OnClickToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
       // VHS();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            onPaused();
        }    
    }

    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void onPaused()
    {       
        if (isPaused == false)
        {
            isPaused = true;
            showPaused();
        }
        else if (isPaused == true)
        {
            isPaused = false;
            hidePaused();
        }
    }

  /*  public void VHS()
    {
        if (isPaused == true)
        {
            VHSimage.SetActive(true);
        }
        else if (isPaused == false)
        {
            VHSimage.SetActive(false);
        }
    }*/

}
