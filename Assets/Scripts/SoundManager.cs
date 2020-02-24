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
    public AudioSource[] BattleMusic;    public AudioSource testSong;

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

    /// <summary>
    /// Enemy Audio Cues
    /// </summary>

    /*[SerializeField]
    public AudioClip[] enemyFootsteps;
    public AudioSource enemyFootAudioSource;

    [SerializeField]
    public AudioClip[] enemyAttacks;
    public AudioSource enemyAttackAudioSource;

    [SerializeField]
    public AudioClip[] enemyHits;
    public AudioSource enemyHitAudioSource;

    [SerializeField]
    public AudioClip[] enemyDeath;
    public AudioSource enemyDeathAudioSource;*/

    // public AudioSource millionaire;

  //  private AudioSource currentAmbient;
  //  private AudioSource currentMusic;

    //float lerpSoundsTo;

   // float ambiantProgress;
   // float musicProgress;

    PauseAbility pauseAbility;
    EnemyManager enemyManager;

    int songs;
    float setVolume;
    public int random;
    public float timer = 1;
  //  float Timeholder;
   // bool randomNumber = true;
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
   //     MuteAllAudio();
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
                        MuteAllAudio();
                        BattleMusicVolumeUp();
                        //Timeholder = Time.realtimeSinceStartup;
                        timer = 5;
                        songs = 0;                        

                        for (int i = 0; i < BattleMusic.Length; i++)
                        {
                            if (BattleMusic[i].isPlaying)
                            {
                                songs++;

                            }
                        }

                        if (songs == 0)
                        {
                            BattleMusic[Random.Range(0, BattleMusic.Length)].Play();
                        }
                        
                                
                    }

                    songs = 0;

                    for (int i = 0; i < BattleMusic.Length; i++)
                    {
                        if (!BattleMusic[i].isPlaying)
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
                         MuteAllAudio();
                        timer = 5;
                        songs = 0;
                        //Timeholder = Time.realtimeSinceStartup;
                        IdleMusicVolumeUp();

                        for (int i = 0; i < IdleMusic.Length; i++)
                        {
                            if (IdleMusic[i].isPlaying)
                            {
                                songs++;

                            }
                        }

                        if (songs == 0)
                        {

                            IdleMusic[Random.Range(0, IdleMusic.Length)].Play();
                        }
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
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.2f, Time.unscaledDeltaTime / 0.2f);
        }
    }

    void IdleMusicVolumeDown()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.2f, Time.unscaledDeltaTime / 0.2f);
        }
    }

    void BattleMusicVolumeUp()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, setVolume, Time.unscaledDeltaTime / 2f); 
        }
    }

    void IdleMusicVolumeUp()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(IdleMusic[i].volume, setVolume, Time.unscaledDeltaTime / 2f); 
        }
    }

    void BattleMusicVolumeGone()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.0f, Time.unscaledDeltaTime / 1f);
        }
    }

    void IdleMusicVolumeGone()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(IdleMusic[i].volume, 0.0f, Time.unscaledDeltaTime / 1f);
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
            //randomNumber = true;
            playBattleMusic = true;   
        }

        if (state != MusicState.IDLE && state != MusicState.PAUSEDSKILL)
        {
            //randomNumber = true;
            playIdleMusic = true;  
        }
    }

    void CheckInBattle()
    {
		if (enemyManager != null)
		{
			if (enemyManager.inBattle == true)
			{
                Debug.Log("should be sounds yes yes");
                    inBattle = true;
			}

			if (enemyManager.inBattle == false)
			{
                Debug.Log("no sounds reeeeee");
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


  /* public void EnemyStepSound()
    {
       
      //  AudioClip clip = enemyFootsteps[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
        //Debug.Log(clip.name);
        //enemyFootAudioSource.PlayOneShot(clip);
        
    }

    public void EnemyAttackSound()
    {

       // AudioClip clip = enemyAttacks[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
        //Debug.Log(clip.name);
        //enemyAttackAudioSource.PlayOneShot(clip);
        
    }

    public void EnemyHitSound()
    {

     //   AudioClip clip = enemyHits[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
      //  Debug.Log(clip.name);
       // enemyHitAudioSource.PlayOneShot(clip);
        
    }

    public void EnemyDeathSound()
    {

      //  AudioClip clip = enemyDeath[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
        //Debug.Log(clip.name);
        //enemyDeathAudioSource.PlayOneShot(clip);
        
    }*/
    
}

