using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   // public AudioSource battleAmbient;
   // public AudioSource battleMusic;
   // public AudioSource slowMostionAmbient;
   // public AudioSource slowMotionMusic;
    public AudioSource mainMenuMusic;

    private AudioSource currentAmbient;
    private AudioSource currentMusic;

    float lerpSoundsTo;

    float ambiantProgress;
    float musicProgress;

    public enum MusicState
    {
        BATTLE,
        SLOWMOTION,
        MAINMENU,
        START,
        PAUSED
    }

    public MusicState state;

    // Start is called before the first frame update
    void Start()
    {
        //state = MusicState.BATTLE;
        MuteAllAudio();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MusicState.START:
				{
					MuteAllAudio();
					//battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 1f, Time.deltaTime / 0.5f);
					//battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.7f, Time.deltaTime / 0.3f);
					break;
				}
            case MusicState.BATTLE:
                {
                    MuteAllAudio();
                  //  battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 1f, Time.deltaTime / 0.5f);
                  //  battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.7f, Time.deltaTime / 0.3f);
                    //battleAmbient.volume = Mathf.Lerp(battleAmbient.volume, 0.1f, Time.deltaTime / 0.5f);
                    break;
                }
            case MusicState.SLOWMOTION:
                {
                    MuteAllAudio();
                   // battleAmbient.volume = Mathf.Lerp(battleAmbient.volume, 1.7f, Time.deltaTime / 0.2f);
                   // battleMusic.pitch = Mathf.Lerp(battleMusic.pitch, 0.8f, Time.deltaTime / 0.05f);
                    //battleMusic.volume = Mathf.Lerp(battleMusic.volume, 0.3f, Time.deltaTime / 0.1f);
                    break;
                }
            case MusicState.MAINMENU:
                {
                    MuteAllAudio();
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

    void MuteAllAudio()
    {
        //battleMusic.volume = 0.0f;
       // slowMotionMusic.volume = 0.0f;
        mainMenuMusic.volume = 0.0f;
       // battleAmbient.volume = 0.0f;
    }
}
