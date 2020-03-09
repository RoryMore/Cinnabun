using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{

    GameObject[] pauseObjects;
    GameObject[] hidePauseObjects;
    GameObject[] playerUI;
    PauseAbility pauseAbility;
    TextSystem textSystem;
    public GameObject visualNovel;
    public GameObject VHSimage;
    public GameObject InventoryUI;
    bool inventory;
    bool player;
    bool pause;
   public bool novel;
 public bool skipText = false;

    public bool isPaused;

    void Start()
    {
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePauseObjects = GameObject.FindGameObjectsWithTag("HideOnPause");
        playerUI = GameObject.FindGameObjectsWithTag("PlayerUI"); 
        textSystem = FindObjectOfType<TextSystem>();
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

        if (InventoryUI.activeSelf == true)
        {
            foreach (GameObject g in playerUI)
            {
                g.SetActive(false);
            }
            player = true;
        }

        if (InventoryUI.activeSelf == false && player == true)
        {
            foreach (GameObject g in playerUI)
            {
                g.SetActive(true);
            }
            player = false;
        }
        if (isPaused == true)
        {
            foreach (GameObject g in playerUI)
            {
                g.SetActive(false);
            }
        }

    }

    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }

        foreach (GameObject g in playerUI)
        {
            g.SetActive(false);
        }

        if (textSystem.hideNovel == false)
        {
            if (visualNovel.activeSelf == true)
            {
                visualNovel.SetActive(false);
                novel = true;
                skipText = true;
            }
        }
        else
        {
            novel = false;
        }

        if (InventoryUI.activeSelf == true)
        {
            InventoryUI.SetActive(false);
            inventory = true;
        }
    }

    public void hidePaused()
    {
        if (visualNovel != null)
        {
           
        }
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }

        foreach (GameObject g in playerUI)
        {
            g.SetActive(true);
        }

     
        if (novel == true)
        {
           visualNovel.SetActive(true);
        }

        if (inventory == true)
        {
            InventoryUI.SetActive(true);
            inventory = false;
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

    

}
