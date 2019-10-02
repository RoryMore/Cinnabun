using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAbility : MonoBehaviour
{

    int actionsLeft = 2;
    public bool inBattle;
    bool isTimeStopped;


    public enum GameStates
    {
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
        switch (states)
        {
            case GameStates.PLAY:

                isTimeStopped = false;

                break;

            case GameStates.TIMESTOP:
                //this is where the check for how meny actions the player has left and to carry out the unpausing 
                // and repauseing after an ability is done
                //also need to add the auto unpause

                if (actionsLeft >= 0)
                {
                    isTimeStopped = true;
                }
                if (actionsLeft <= 0)
                {
                    states = GameStates.PLAY;
                }
                break;
        }

        if (!pauseMenu.isPaused)
        {
            TimeStop();
            checkTime();
        }
        else if (pauseMenu.isPaused)
        {
            Time.timeScale = 0;
        }

    }

    void TimeStop()
    {
        //Add in to check if player is casting an ability, cannot Starttime again until this is complete
        if (Input.GetKeyDown(KeyCode.Space))
        {    
          if (states == GameStates.PLAY)
          {    
                states = GameStates.TIMESTOP;  
          }
          else if (states == GameStates.TIMESTOP)
          {
                states = GameStates.PLAY; 
          }
        }
    }

    void checkTime()
    {
        if (isTimeStopped == true)
        {
            Time.timeScale = 0;
        }
        else if (isTimeStopped == false)
        {
            Time.timeScale = 1;
        }
    }

}
