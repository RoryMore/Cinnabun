using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // public AudioSource battleAmbient;
    // public AudioSource battleMusic;
    // public AudioSource slowMostionAmbient;
    // public AudioSource slowMotionMusic;

    [Header("Main Menu Music")]
  //[Space(10)]
    public AudioSource mainMenuMusic;

    public AudioSource[] BattleMusic;

   // [Header("Battle Music")]
   // public AudioSource battleMusic1;
   //// public AudioSource battleMusic2;
  //  public AudioSource battleMusic3;

    [Header("Idle Music")]
    public AudioSource idleMusic1;
    public AudioSource idleMusic2;
    public AudioSource idleMusic3;

    [Header("Sound Effects")]
    //public AudioSource meeleeSwing;
    [SerializeField]
    AudioSource meeleSwing;
    [HideInInspector] public static AudioSource meleeSwing;

    [SerializeField]
    AudioSource footStepLeft;
    public static AudioSource leftFootstep;
    [SerializeField]
    AudioSource footstepRight;
    public static AudioSource rightFootstep;

   // public AudioSource millionaire;

    private AudioSource currentAmbient;
    private AudioSource currentMusic;

    float lerpSoundsTo;

    float ambiantProgress;
    float musicProgress;

    PauseAbility pauseAbility;
    EnemyManager enemyManager;

    float setVolume;
    public int random;
    public float timer = 2;
    bool randomNumber = true;
    bool playBattleMusic = true;
    bool playIdleMusic = true;
    bool inBattle;

    public enum MusicState
    {
        BATTLE,
        PAUSEDSKILL,
        MAINMENU,
        START,
        PAUSED,
        IDLE
    }

    public MusicState state;

    // Start is called before the first frame update
    void Start()
    {
        pauseAbility = FindObjectOfType<PauseAbility>();
        enemyManager = FindObjectOfType<EnemyManager>();
        setVolume = 0.7f;


    }

    private void Awake()
    {
        // Setting static sound effect variables to inspector set data
        meleeSwing = meeleSwing;

        leftFootstep = footStepLeft;
        rightFootstep = footstepRight;
    }

    // Update is called once per frame
    void Update()
    {

        CheckInBattle();
        checkState();

        switch (state)
        {
            case MusicState.START:
				{
					break;
				}
            case MusicState.BATTLE:
                {
               

                    if (playBattleMusic == true)
                    {
        
                        for (int i = 0; i < BattleMusic.Length; i++)
                        {
                            BattleMusic[i].volume = setVolume;
                        }

                        BattleMusic[Random.Range(0, BattleMusic.Length)].Play();
                                
                    }
         
                    playBattleMusic = false;

                    break;
                }
            case MusicState.PAUSEDSKILL:
                {
                    //BattleMusicVolumeDown();

                    IdleMusicVolumeDown();

                    break;
                }
            case MusicState.IDLE:
                {
                   

                    if (playIdleMusic == true)
                    {
                        getRandomNumber();
                        timer = 1f;
                        MuteIdle();
                        if (random == 0)
                        {
                            idleMusic1.Play();
                        }
                        else if (random == 1)
                        {
                            idleMusic2.Play();
                        }
                        if (random == 2)
                        {
                            idleMusic3.Play();
                        }



                        
                    }

                    //BattleMusicGone();
                    MuteTimer();
                    IdleMusicVolumeUp();

              

                    playIdleMusic = false;

                    break;
                }
            case MusicState.MAINMENU:
                {
                    mainMenuMusic.volume = 0.7f;
                    break;
                }
                case MusicState.PAUSED:
                {
                        MuteAllAudio();
                    break;
                }
        }

    
    }


   /* void BattleMusicVolumeUp()
    {
        battleMusic1.volume = Mathf.Lerp(battleMusic1.volume, setVolume, Time.unscaledDeltaTime / 1f);
        battleMusic2.volume = Mathf.Lerp(battleMusic2.volume, setVolume, Time.unscaledDeltaTime / 1f);
        battleMusic3.volume = Mathf.Lerp(battleMusic3.volume, setVolume, Time.unscaledDeltaTime / 1f);
    }

    void BattleMusicVolumeDown()
    {
        battleMusic1.volume = Mathf.Lerp(battleMusic1.volume, 0.2f, Time.unscaledDeltaTime / 0.3f);
        battleMusic2.volume = Mathf.Lerp(battleMusic2.volume, 0.2f, Time.unscaledDeltaTime / 0.3f);
        battleMusic3.volume = Mathf.Lerp(battleMusic3.volume, 0.2f, Time.unscaledDeltaTime / 0.3f);
        //MuteIdle();
    }*/

    void IdleMusicVolumeDown()
    {
       idleMusic1.volume = Mathf.Lerp(idleMusic1.volume, 0.2f, Time.unscaledDeltaTime / 0.3f);
       idleMusic2.volume = Mathf.Lerp(idleMusic2.volume, 0.2f, Time.unscaledDeltaTime / 0.3f);
       idleMusic3.volume = Mathf.Lerp(idleMusic3.volume, 0.2f, Time.unscaledDeltaTime / 0.3f);
       // MuteBattle();
    }

    void IdleMusicVolumeUp()
    {
       idleMusic1.volume = Mathf.Lerp(idleMusic1.volume, setVolume, Time.unscaledDeltaTime / 1f);
       idleMusic2.volume = Mathf.Lerp(idleMusic2.volume, setVolume, Time.unscaledDeltaTime / 1f);
       idleMusic3.volume = Mathf.Lerp(idleMusic3.volume, setVolume, Time.unscaledDeltaTime / 1f);
    }

   /* void BattleMusicGone()
    {
        battleMusic1.volume = Mathf.Lerp(battleMusic1.volume, 0.0f, Time.unscaledDeltaTime / 0.3f);
        battleMusic2.volume = Mathf.Lerp(battleMusic2.volume, 0.0f, Time.unscaledDeltaTime / 0.3f);
        battleMusic3.volume = Mathf.Lerp(battleMusic3.volume, 0.0f, Time.unscaledDeltaTime / 0.3f);       
        
    }*/
    void IdleMusicGone()
    {
        idleMusic1.volume = Mathf.Lerp(idleMusic1.volume, 0.0f, Time.unscaledDeltaTime / 0.3f);
        idleMusic2.volume = Mathf.Lerp(idleMusic2.volume, 0.0f, Time.unscaledDeltaTime / 0.3f);
        idleMusic3.volume = Mathf.Lerp(idleMusic3.volume, 0.0f, Time.unscaledDeltaTime / 0.3f);
    }

    void checkMusicPLaying()
    {
        
    }

    void checkState()
    {
        if (pauseAbility.states == PauseAbility.GameStates.TIMESTOP)
        {
            state = MusicState.PAUSEDSKILL; 
        }

        if (inBattle == true)
        {
            if (pauseAbility.states != PauseAbility.GameStates.TIMESTOP)
            {

                state = MusicState.BATTLE;
               // BattleMusicVolumeUp();
            }
        }

        if (inBattle == false)
        {
            if (pauseAbility.states != PauseAbility.GameStates.TIMESTOP)
            {
                state = MusicState.IDLE;
                IdleMusicVolumeUp();
            }

            
        }

        if (state != MusicState.BATTLE && state != MusicState.PAUSEDSKILL)
        {
            randomNumber = true;
            playBattleMusic = true;
          
            
            
        }

        if (state != MusicState.IDLE && state != MusicState.PAUSEDSKILL)
        {
            randomNumber = true;
            playIdleMusic = true;
            
        }
    }

    void CheckInBattle()
    {
		if (enemyManager != null)
		{
			if (enemyManager.inBattle == true)
			{ 
                    inBattle = true;
			}

			if (enemyManager.inBattle == false)
			{
				inBattle = false;
			}
		}
		else
		{
			inBattle = false;
		}
    }

    void getRandomNumber()
    {

        if (randomNumber == true)
        {
            random = Random.Range(0, 3);
        }
        randomNumber = false;
    }

    void MuteTimer()
    {
        // float timer = 2;

        if (timer > -1)
        {
            
            timer -= Time.unscaledDeltaTime;
            if (timer < 0)
            {
                if (state == MusicState.IDLE)
                {
                    MuteBattle();
                }

                if (state == MusicState.BATTLE)
                {
                    MuteIdle();
                }
            }
        }
    }


    void MuteAllAudio()
    {
        mainMenuMusic.volume = 0.0f;

       // battleMusic1.Stop();
       // battleMusic2.Stop();
      //  battleMusic3.Stop();

        idleMusic1.Stop();
        idleMusic2.Stop();
        idleMusic3.Stop();
    }

    void MuteBattle()
    {
      //  battleMusic1.Stop();
      //  battleMusic2.Stop();
      //  battleMusic3.Stop();
    }

    void MuteIdle()
    {
        idleMusic1.Stop();
        idleMusic2.Stop();
        idleMusic3.Stop();
    }
}
