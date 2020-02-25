using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeSinceStartUp : MonoBehaviour
{

    PauseAbility pause;
    private Text textNumbers;
    float timeNumbers;

    int hours = 0;
    int minutes = 0;
    [SerializeField]
    int seconds = 0;

    float localLoadTime = 0;

    Color textColor;
    // Start is called before the first frame update
    void Start()
    {
        textNumbers = transform.GetComponent<Text>();
        pause = FindObjectOfType<PauseAbility>();
       // textColor = textNumbers;
    }

    // Update is called once per frame
    void Update()
    {
        //seconds = (int)Time.timeSinceLevelLoad;

        float levelLoadTime;
        levelLoadTime = Time.timeSinceLevelLoad - localLoadTime;
        seconds = (int)levelLoadTime;

        if (seconds >= 60)
        {
            localLoadTime = Time.timeSinceLevelLoad;
            minutes++;
        }
        if (minutes >= 60)
        {
            minutes = 0;
            hours++;
        }

        string doubleDigitHours, doubleDigitMinutes, doubleDigitSeconds;
        if (hours < 10)
        {
            doubleDigitHours = "0" + hours.ToString();
        }
        else
        {
            doubleDigitHours = hours.ToString();
        }
        if (minutes < 10)
        {
            doubleDigitMinutes = "0" + minutes.ToString();
        }
        else
        {
            doubleDigitMinutes = minutes.ToString();
        }
        if (seconds < 10)
        {
            doubleDigitSeconds = "0" + seconds.ToString();
        }
        else
        {
            doubleDigitSeconds = seconds.ToString();
        }
      
        textNumbers.text = doubleDigitHours.ToString()+ ":" + doubleDigitMinutes.ToString() + ":"+ doubleDigitSeconds.ToString();

   
    }
}
