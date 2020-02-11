﻿using System.Collections;
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
    public AudioSource battleMusic1;
    public AudioSource battleMusic2;
    public AudioSource battleMusic3;

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

    public int random;
    public int randomIdle;
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
        test();

        switch (state)
        {
            case MusicState.START:
				{
					//MuteAllAudio();
					//battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 1f, Time.deltaTime / 0.5f);
					//scordbattleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.7f, Time.deltaTime / 0.3f);
					break;
				}
            case MusicState.BATTLE:
                {
                   // MuteAllAudio();
                    getRandomNumber();

                    //  battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 1f, Time.deltaTime / 0.5f);
                    //  battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.7f, Time.deltaTime / 0.3f);
                    //battleAmbient.volume = Mathf.Lerp(battleAmbient.volume, 0.1f, Time.deltaTime / 0.5f);
                    if (playBattleMusic == true)
                    {
                        //meeleeSwing.Play();
                        MuteAllAudio();

                        if (random == 0)
                        {
                             battleMusic1.Play();
                            
                          
                        }
                        else if (random == 1)
                        {
                           battleMusic2.Play();
                          
                        }
                        if (random == 2)
                        {
                           battleMusic3.Play();
                          
                        }
                    }
                   // millionaire.volume = 0.0f;
                    battleMusic1.volume = Mathf.Lerp(battleMusic1.volume, 0.8f, Time.unscaledDeltaTime / 0.1f);
                    battleMusic2.volume = Mathf.Lerp(battleMusic2.volume, 0.8f, Time.unscaledDeltaTime / 0.1f);
                    battleMusic3.volume = Mathf.Lerp(battleMusic3.volume, 0.8f, Time.unscaledDeltaTime / 0.1f);
                    playBattleMusic = false;
                    break;
                }
            case MusicState.PAUSEDSKILL:
                {
                     //MuteAllAudio();
                   // millionaire.volume = 0.8f;
                    battleMusic1.volume = Mathf.Lerp(battleMusic1.volume, 0.2f, Time.unscaledDeltaTime / 0.1f);
                    battleMusic2.volume = Mathf.Lerp(battleMusic2.volume, 0.2f, Time.unscaledDeltaTime / 0.1f);
                    battleMusic3.volume = Mathf.Lerp(battleMusic3.volume, 0.2f, Time.unscaledDeltaTime / 0.1f);
                    // battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 0.8f, Time.deltaTime / 0.05f);
                    //battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.3f, Time.deltaTime / 0.1f);
                    break;
                }
            case MusicState.IDLE:
                {
                    getRandomNumber();

                    if (playIdleMusic == true)
                    {
                        MuteAllAudio();
                        if (random == 0)
                        {
                            //idleMusic1.volume
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
                        // battleAmbient.volume = Mathf.Lerp(battleAmbient.volume, 1.7f, Time.deltaTime / 0.2f);
                        // battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 0.8f, Time.deltaTime / 0.05f);
                        //battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.3f, Time.deltaTime / 0.1f);
                    }
                    // idleMusic1.volume = 0.7f;
                    playIdleMusic = false;

                    break;
                }
            case MusicState.MAINMENU:
                {
                   // MuteAllAudio();
					//mainMenuMusic.Play();
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
            }
        }

        if (inBattle == false)
        {
            state = MusicState.IDLE;
        }

        if (state != MusicState.BATTLE && state != MusicState.PAUSEDSKILL)
        {
            randomNumber = true;
            playBattleMusic = true;
        }

        if (state != MusicState.IDLE)
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
        //MuteAllAudio();

        if (randomNumber == true)
        {
            random = Random.Range(0, 3);
        }
        randomNumber = false;
    }


    void MuteAllAudio()
    {
        //battleMusic.volume = 0.0f;
       // slowMotionMusic.volume = 0.0f;
        mainMenuMusic.volume = 0.0f;
        //millionaire.volume = 0.0f;


        battleMusic1.Stop();
        battleMusic2.Stop();
        battleMusic3.Stop();

        idleMusic1.Stop();
        idleMusic2.Stop();
        idleMusic3.Stop();

        // battleAmbient.volume = 0.0f;
    }
}
