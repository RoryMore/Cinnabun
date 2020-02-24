using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAbility : MonoBehaviour
{

    public int actionsLeft = 2;
    [SerializeField] int maxActions = 2;
    public float timeStopCoolDown;
    public float abilityCastTime = 0;
    public bool inBattle;
    bool isTimeStopped;
    public bool takeingTurn;

    public List<Entity> entity;

    Player player;

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

    private void Awake()
    {
        // Find reference to the Player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        entity.AddRange(GameObject.FindObjectsOfType<Entity>());

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
            Time.timeScale = 0.0f;
        }

        checkAbilityCastTime();
        checkTimeStopOnCoolDown();
    }


    void TimeStop()
    {
        //Add in to check if player is casting an ability, cannot Starttime again until this is complete
        if (Input.GetKeyDown(KeyCode.Space))
        {    
            if (player.playerState != Player.PlayerState.DOINGSKILL)
            {
                if (states == GameStates.PLAY)
                {
                    states = GameStates.TIMESTOP;
                }
                else if (states == GameStates.TIMESTOP)
                {
                    states = GameStates.PLAY;
                    clearAllList();
                    calculateTimeStop();
                    takeingTurn = false;
                }
            }
          
        }
    }
    public void ButtonPaused()
    {
        if (states == GameStates.PLAY)
        {
            states = GameStates.TIMESTOP;
        }
    }

   public void ButtonPlay()
    {
        if (player.playerState != Player.PlayerState.DOINGSKILL)
        {

            states = GameStates.PLAY;
            clearAllList();
            calculateTimeStop();
            takeingTurn = false;
        }
    }

    void checkTime()
    {
        if (isTimeStopped == true)
        {
            Time.timeScale = 0.0f;
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
        //if (abilityCastTime >= 0)
        //{
        //    abilityCastTime -= Time.deltaTime;
        //}

        
        if (actionsLeft > 0)
        {
            if (player.selectedSkill != null)
              {
                  if (player.selectedSkill.currentlyCasting)
                   {
                       states = GameStates.PLAY;
                       takeingTurn = true;
                   }
               }
               else if (takeingTurn == true)
               {
                states = GameStates.TIMESTOP;
               }
        }
        

          //  if (takeingTurn == true)
          // {
         //       states = GameStates.TIMESTOP;
        //    }


        if (actionsLeft == 0 && player.selectedSkill == null)
        {
            calculateTimeStop();
            clearAllList();
            actionsLeft = maxActions;
            states = GameStates.PLAY;
            takeingTurn = false;
        }

    }

    void clearAllList()
    {
        //foreach (Entity checkedEntity in entity)
        //{
        //    checkedEntity.ClearList();
        //}

        // For our current encounter (other entities are irrelevant)
        // Clear the rewind points
        if (Entity.currentEncounter != null)
        {
            foreach (Entity checkedEntity in Entity.currentEncounter.initiativeList)
            {
                checkedEntity.ClearList();
            }
        }
        // Player isn't held in encounter
        // Clear player rewindpoints
        player.ClearList();
    }
}
