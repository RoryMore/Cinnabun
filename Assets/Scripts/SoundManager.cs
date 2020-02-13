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



    [Header("Battle Music")]
    public AudioSource[] BattleMusic;

    [Header("Idle Music")]

    public AudioSource[] IdleMusic;


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

    int songs;
    float setVolume;
    public int random;
    public float timer = 1;
    float Timeholder;
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
        float timeNow = Time.realtimeSinceStartup;

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
                        //MuteAllAudio();
                        BattleMusicVolumeUp();
                        Timeholder = Time.realtimeSinceStartup;
                        timer = 2;

                        BattleMusic[Random.Range(0, BattleMusic.Length)].Play();
                                
                    }

                    songs = 0;

                    for (int i = 0; i < BattleMusic.Length; i++)
                    {
                        if (!IdleMusic[i].isPlaying)
                        {
                            songs++;

                        }
                    }

                    if (songs == BattleMusic.Length)
                    {
                        BattleMusic[Random.Range(0, BattleMusic.Length)].Play();
                    }

                    IdleMusicVolumeGone();
                    MuteTimer();

                    playBattleMusic = false;

                    break;
                }
            case MusicState.PAUSEDSKILL:
                {
                    BattleMusicVolumeDown();
                    IdleMusicVolumeDown();
                    if (inBattle == true)
                    {
                        MuteIdle();
                    }

                    if (inBattle == false)
                    {
                        MuteBattle();
                    }

                    break;
                }
            case MusicState.IDLE:
                {
                   

                    if (playIdleMusic == true)
                    {
                        // MuteAllAudio();
                        timer = 2;
                        Timeholder = Time.realtimeSinceStartup;
                        IdleMusicVolumeUp();

                        IdleMusic[Random.Range(0, IdleMusic.Length)].Play();
                    }

                    songs = 0;

                    for (int i = 0; i < IdleMusic.Length; i++)
                    {
                        if (!IdleMusic[i].isPlaying)
                        {
                            songs++;

                        }
                    }

                    if (songs == IdleMusic.Length)
                    {
                        IdleMusic[Random.Range(0, IdleMusic.Length)].Play();
                    }

                    MuteTimer();

                    BattleMusicVolumeGone();
                   

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



    void BattleMusicVolumeDown()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.2f, Time.deltaTime / 0.3f);
        }
    }

    void IdleMusicVolumeDown()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.2f, Time.deltaTime / 0.3f);
        }
    }

    void BattleMusicVolumeUp()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, setVolume, Time.deltaTime / 0.8f); 
        }
    }

    void IdleMusicVolumeUp()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(IdleMusic[i].volume, setVolume, Time.deltaTime / 0.3f); 
        }
    }

    void BattleMusicVolumeGone()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.0f, Time.fixedDeltaTime / 0.3f);
        }
    }

    void IdleMusicVolumeGone()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(IdleMusic[i].volume, 0.0f, Time.fixedDeltaTime / 0.3f);
        }
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
                BattleMusicVolumeUp();
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


    void MuteTimer()
    {

        if (timer > -1)
        {
            float timeNow = Time.realtimeSinceStartup;

            timeNow -= Timeholder;
            timer -= timeNow;
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

        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].Stop();
        }

        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].Stop();
        }


    }

    void MuteBattle()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].Stop();
        }

    }

    void MuteIdle()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].Stop();
        }
    }
}
