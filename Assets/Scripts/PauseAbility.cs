using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAbility : MonoBehaviour
{

    public int actionsLeft = 2;
    [SerializeField] int maxActions = 2;
    public float timeStopCoolDown;
    //public float abilityCastTime = 0;
    public bool inBattle;
    bool isTimeStopped;
    public bool activatedAbility;
    public bool unPaused;

    Player player;
    public GameObject[] entity;

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
       //entity = FindObjectOfType<Entity>();
       states = GameStates.PLAY;
    }

    private void Awake()
    {
        // Find reference to the Player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        entity = GameObject.FindGameObjectsWithTag("Enemy");
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
                else if (actionsLeft <= 0)
                {
                    states = GameStates.PLAY;
                    calculateTimeStop();
                }
                break;
        }

        if (!pauseMenu.isPaused)
        {
            if (timeStopCoolDown <= 0)
            {
                TimeStop();
            }
            checkTime();
        }
        else if (pauseMenu.isPaused)
        {
            Time.timeScale = 0;
        }

        checkAbilityCastTime();
        checkTimeStopOnCoolDown();
        test();
    }

    void test()
    {
        if (states == GameStates.TIMESTOP)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                actionsLeft -= 1;
                //abilityCastTime = 1;
                //states = GameStates.PLAY;
                //activatedAbility = true;

            }
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
                unPaused = true;
                states = GameStates.PLAY;
                //unPaused = false;
                calculateTimeStop();
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

    void checkTimeStopOnCoolDown()
    {
        if (states == GameStates.PLAY)
        {
            if (timeStopCoolDown >= 0)
            {
                timeStopCoolDown -= Time.deltaTime;
            }
        }
    }

    void calculateTimeStop()
    {
        timeStopCoolDown = maxActions;
        timeStopCoolDown -= actionsLeft;
        timeStopCoolDown += 2;
        actionsLeft = maxActions;
    }

    void checkAbilityCastTime()
    {
        
        if (actionsLeft > 0)
        {
            if (player.selectedSkill != null)
            {
                // If the player is casting
                if (player.selectedSkill.currentlyCasting)
                {
                    states = GameStates.PLAY;
                }
                // Else stop again
                else
                {
                    //rewind state time stamp
                   //entity.RecordRewind();
                    states = GameStates.TIMESTOP;

                    //activatedAbility = false;
                }
            }
        }

        if (actionsLeft == 0 && player.selectedSkill == null)
        {
            calculateTimeStop();
            unPaused = true;
            actionsLeft = maxActions;
            states = GameStates.PLAY;
        }


    }

}
