using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAbility : MonoBehaviour
{

    int actionsLeft;


    public enum GameStates
    {
        PAUSED,
        PLAY,
        TIMESTOP

    }
    public GameStates states;
    PauseMenuUI pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
       pauseMenu = FindObjectOfType<PauseMenuUI>();
       states = GameStates.PLAY;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
        if (!pauseMenu.isPaused)
        {
            TimeStop();
        }
    }

    void TimeStop()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           if (states != GameStates.TIMESTOP)
           {
               Time.timeScale = 0;
               states = GameStates.TIMESTOP;               
           }
           else if (states == GameStates.TIMESTOP)
           {
               Time.timeScale = 1;
               states = GameStates.PLAY;          
           }
        }
    }

}
