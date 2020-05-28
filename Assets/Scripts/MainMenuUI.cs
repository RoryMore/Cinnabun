using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject menuButtons;
    public GameObject credits;
    public GameObject OptionsUI;

    [Header("Buttons")]
    public GameObject newGameButton;
    public GameObject continueButton;

    bool initialised = false;

    private void Update()
    {
        if (!initialised)
        {
            // Money file was loaded so they have definitely played before
            if (SaveManager.GetObjectsLoaded().moneyLoaded)
            {
                newGameButton.SetActive(false);
                continueButton.SetActive(true);
            }
            else // No money file so they're still a freshy
            {
                newGameButton.SetActive(true);
                continueButton.SetActive(false);
            }
            initialised = true;
        }
    }

    public void OnClickPlay()
    {
        // This is often changed with each feedback build. Keep an eye
        SceneManager.LoadScene(SaveManager.gameScene);
        Time.timeScale = 1;
    }

    void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickCredits()
    {
        menuButtons.SetActive(false);
        credits.SetActive(true);
    }

    public void OnClickReturn()
    {
        menuButtons.SetActive(true);
        credits.SetActive(false);
        OptionsUI.SetActive(false);
    }

    public void Options()
    {
        OptionsUI.SetActive(true);
        menuButtons.SetActive(false);
       // SaveManager.GetSettings().musicVolume = 3f;

     //   SaveManager.SaveSettings();
    }

	public void Tutorial()
	{
		SceneManager.LoadScene(1);
		Time.timeScale = 1;
	}
}
