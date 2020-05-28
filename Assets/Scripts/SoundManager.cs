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
	public AudioSource testSong;


	[Header("Idle Music")]

	public AudioSource[] IdleMusic;

	[SerializeField]
	AudioSource blast;
	[HideInInspector] public static AudioSource blastsound;
	[Header("Sound Effects")]
    //public AudioSource meeleeSwing;
    [SerializeField]
    AudioSource meeleSwing;
	[HideInInspector] public static AudioSource meleeSwing;

	[SerializeField]
	//AudioSource blast;
	//[HideInInspector] public static AudioSource blast;

    AudioSource footStepLeft;
	[HideInInspector] public static AudioSource leftFootstep;

    [SerializeField]
    AudioSource footstepRight;
	[HideInInspector] public static AudioSource rightFootstep;

    [Header(" EnemyFoot")]
    [SerializeField] AudioSource EnemyFoot1;
    [SerializeField] AudioSource EnemyFoot2;
    [SerializeField] AudioSource EnemyFoot3;
    public static List<AudioSource> EnemyFoot = new List<AudioSource>();

    [Header(" EnemyAttack")]
    [SerializeField] AudioSource EnemyAttack1;
    [SerializeField] AudioSource EnemyAttack2;
    [SerializeField] AudioSource EnemyAttack3;
    public static List<AudioSource> EnemyAttack = new List<AudioSource>();

    [Header(" EnemyHit")]
    [SerializeField] AudioSource EnemyHit1;
    [SerializeField] AudioSource EnemyHit2;
    [SerializeField] AudioSource EnemyHit3;
    public static List<AudioSource> EnemyHit = new List<AudioSource>();

    [Header(" Enemydeath")]
    [SerializeField] AudioSource Enemydeath1;
    [SerializeField] AudioSource Enemydeath2;
    [SerializeField] AudioSource Enemydeath3;
    public static List<AudioSource> Enemydeath = new List<AudioSource>();


    PauseAbility pauseAbility;
    EnemyManager enemyManager;
    public GameObject playerUI;
    public GameObject visualNovel;
	Player player;
    int songs;
    float setVolume;
    [Header("Other")] public int random;
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
		player = FindObjectOfType<Player>();
		setVolume = 0.7f;

        float timeNow = Time.realtimeSinceStartup;

    }

    private void Awake()
    {
        // Setting static sound effect variables to inspector set data
        meleeSwing = meeleSwing;
		blastsound = blast;

        leftFootstep = footStepLeft;
        rightFootstep = footstepRight;


        //add sound to the array or list

        EnemyFoot.Add(EnemyFoot1);
        EnemyFoot.Add(EnemyFoot2);
        EnemyFoot.Add(EnemyFoot3);

        EnemyAttack.Add(EnemyAttack1);
        EnemyAttack.Add(EnemyAttack2);
        EnemyAttack.Add(EnemyAttack3);

        EnemyHit.Add(EnemyHit1);
        EnemyHit.Add(EnemyHit2);
        EnemyHit.Add(EnemyHit3);

        Enemydeath.Add(Enemydeath1);
        Enemydeath.Add(Enemydeath2);
        Enemydeath.Add(Enemydeath3);


    }

    // Update is called once per frame
    void Update()
    {
   //     MuteAllAudio();
        CheckInBattle();
        checkState();
	
		//Blast();

        switch (state)
        {
            case MusicState.START:
				{
                    if (playerUI.activeSelf == true)
                    {
                        playerUI.SetActive(false);
                    }
                  
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
                    if (!mainMenuMusic.isPlaying)
                    {
                        mainMenuMusic.Play();
                    }
                    mainMenuMusic.volume = 0.7f * SaveManager.GetSettings().musicVolume;
                    break;
                }
                case MusicState.PAUSED:
                {
                        MuteAllAudio();
                    break;
                }
        }

        UpdateSFXVolumes();
    }


    void BattleMusicVolumeDown()
    {

        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.2f * SaveManager.GetSettings().musicVolume, Time.unscaledDeltaTime / 0.2f);
        }
    }



    void IdleMusicVolumeDown()
    {
       for (int i = 0; i < IdleMusic.Length; i++)
        {
            IdleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, 0.2f * SaveManager.GetSettings().musicVolume, Time.unscaledDeltaTime / 0.2f);
        }
    }



    void BattleMusicVolumeUp()
    {
        for (int i = 0; i < BattleMusic.Length; i++)
        {
            BattleMusic[i].volume = Mathf.Lerp(BattleMusic[i].volume, setVolume * SaveManager.GetSettings().musicVolume, Time.unscaledDeltaTime / 2f); 
        }
    }



    void IdleMusicVolumeUp()
    {
        for (int i = 0; i < IdleMusic.Length; i++)
        {
           IdleMusic[i].volume = Mathf.Lerp(IdleMusic[i].volume, setVolume * SaveManager.GetSettings().musicVolume, Time.unscaledDeltaTime / 2f); 
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
        if (pauseAbility != null)
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

            if (inBattle == false && visualNovel.activeSelf == false)
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
    }

    void CheckInBattle()
    {
		if (enemyManager != null)
		{
            if (visualNovel.activeSelf == false)
            {
                if (enemyManager.inBattle == true)
                {
                   // Debug.Log("should be sounds yes yes");
                    inBattle = true;
                }

                if (enemyManager.inBattle == false)
                {
                   // Debug.Log("no sounds reeeeee");
                    inBattle = false;
                }
            }
         //   else
         //   {
           //     state = MusicState.START;
				//inBattle = true;
           // }
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

    void UpdateSFXVolumes()
    {
        if (SaveManager.GetSettings() != null)
        {
            blastsound.volume = setVolume * SaveManager.GetSettings().sfxVolume;

            meleeSwing.volume = setVolume * SaveManager.GetSettings().sfxVolume;

            leftFootstep.volume = setVolume * SaveManager.GetSettings().sfxVolume;

            rightFootstep.volume = setVolume * SaveManager.GetSettings().sfxVolume;

            foreach (AudioSource source in EnemyHit)
            {
                if (source != null)
                {
                    source.volume = setVolume * SaveManager.GetSettings().sfxVolume;
                }
            }
            foreach (AudioSource source in Enemydeath)
            {
                if (source != null)
                {
                    source.volume = setVolume * SaveManager.GetSettings().sfxVolume;
                }
            }
            foreach (AudioSource source in EnemyAttack)
            {
                if (source != null)
                {
                    source.volume = setVolume * SaveManager.GetSettings().sfxVolume;
                }
            }
            foreach (AudioSource source in EnemyFoot)
            {
                if (source != null)
                {
                    source.volume = setVolume * SaveManager.GetSettings().sfxVolume;
                }
            }
        }
    }


	/*public void Blast()
	{

		blast[Random.Range(0, blast.Length)].Play();
	}*/

    
}

