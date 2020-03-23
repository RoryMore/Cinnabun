using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTriggers : MonoBehaviour
{

    [SerializeField] AudioSource Water;
    public static AudioSource WaterSounds;
    public static AudioSource Wind;
    public static AudioSource Birds;

    public float volume;

    public enum SoundStates
    {
        WATER,
        WIND,
        BIRD,
        NOTHING,
    
    }

    public SoundStates state;

    // Start is called before the first frame update
    void Start()
    {
        WaterSounds = Water;
        state = SoundStates.NOTHING;
        //Water.Play();
        //Wind.Play();
        // Birds.Play();

        //  MuteSound();
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)

        {
            case SoundStates.WATER:
                {

                    Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
                    break;
                }
            case SoundStates.WIND:
                {

                  //  Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
                    break;
                }
            case SoundStates.BIRD:
                {

                  //  Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
                    break;
                }
            case SoundStates.NOTHING:
                {

                    //Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
                    break;
                }
        }
    }
    
    
    
    public static void OnTriggerEnter(Collider other)
    {
        //MuteSound();

       
        // Wind.Play();
        // Birds.Play();

        //MuteSound();

        /*   if (other.tag == "SoundWater")
           {
               Mathf.Lerp(Water.volume, volume, Time.unscaledDeltaTime / 1f);
           }

           if (other.tag == "SoundWind")
           {
               Mathf.Lerp(Wind.volume, volume, Time.unscaledDeltaTime / 1f);
           }

           if (other.tag == "SoundBird")
           {
               Mathf.Lerp(Birds.volume, volume, Time.unscaledDeltaTime / 1f);
           }*/
    }


       


    public static void OnTriggerExit(Collider other)
    {
        if (other.tag == "SoundTWater")
        {
            Mathf.Lerp(WaterSounds.volume, 0.0f, Time.unscaledDeltaTime / 1f);
        }

        if (other.tag == "SoundWind")
        {
            Mathf.Lerp(Wind.volume, 0.0f, Time.unscaledDeltaTime / 1f);
        }

        if (other.tag == "SoundBird")
        {
            Mathf.Lerp(Birds.volume, 0.0f, Time.unscaledDeltaTime / 1f);
        }

      
    }

    void volumeUpdate()
    {

        Mathf.Lerp(Water.volume, 0.0f, Time.unscaledDeltaTime / 1f);
        Mathf.Lerp(Wind.volume, 0.0f, Time.unscaledDeltaTime / 1f);
        Mathf.Lerp(Birds.volume, 0.0f, Time.unscaledDeltaTime / 1f);

    }

    void MuteSound()
    {
        Birds.volume = 0.0f;
        Water.volume = 0.0f;
        Wind.volume = 0.0f;
    }

    public static void playWaterSound()
    {
        WaterSounds.volume = 0.0f;
        WaterSounds.Play();
        Mathf.Lerp(WaterSounds.volume, 7.0f, Time.unscaledDeltaTime / 1f);
       // WaterSounds.volume = 8.0f;
    }

}
