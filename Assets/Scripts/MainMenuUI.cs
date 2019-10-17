﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    public GameObject menuButtons;
    public GameObject credits;


    void OnClickPlay()
    {
        // This is often changed with each feedback build. Keep an eye
        SceneManager.LoadScene(1);
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
    }
}
