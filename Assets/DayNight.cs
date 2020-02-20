using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public enum MethodOfMoving
    {
        Timer,
        Speed,
        MultTime
    }
    public enum CurrentTime
    {
        Dawn,
        Morning,
        MidDay,
        Afternoon,
        Dusk,
        Night
    }

    //current time of day list
    public List<float> ToNextPArtOfTheDay;
    public List<float> SunMoonTimes;
    public int TimeOfDaySun;
    public int TimeOfDayMoon;
    CurrentTime TimeOfDay;

    //what type of way to tell how quickly the sun moves
    public MethodOfMoving MovingMent;
    public float MoonSpeed;
    public float SunSpeed;
    public float timer;

    //rotation
    
     float pastMoonRotation;
     float currentMoonRotation;
     float pastDayRotation;
     float currentDayRotation;

    private Light Sun;
    private Light Moon;
    // Start is called before the first frame update
    void Start()
    {
        Sun = transform.GetChild(0).GetComponent<Light>();
        Moon = transform.GetChild(1).GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).LookAt(transform.GetChild(2).transform);
        transform.GetChild(1).LookAt(transform.GetChild(2).transform);

        if (MovingMent == MethodOfMoving.Timer)
        {
            MoonSpeed = 360 * (timer / 60);
            SunSpeed = 360 * (timer / 60);
        }
        if (MovingMent == MethodOfMoving.MultTime)
        {
            MoonSpeed = 360 * (SunMoonTimes[TimeOfDayMoon] / 60);
            SunSpeed = 360 * (SunMoonTimes[TimeOfDaySun] / 60);
        }
        //change rotation
        UpdateRotation();

        //
        currentMoonRotation = NewLight(Moon, pastMoonRotation, currentMoonRotation);
        currentDayRotation = NewLight(Sun, pastDayRotation, currentDayRotation);

        //
        TimeOfDayMoon= changeDay(TimeOfDayMoon, currentMoonRotation);
        TimeOfDaySun=changeDay(TimeOfDaySun, currentDayRotation);
    }
    void UpdateRotation()
    {
        //Sun rotation update
        pastMoonRotation = transform.GetChild(0).rotation.eulerAngles.x;
        transform.GetChild(0).RotateAround(transform.GetChild(2).transform.position, Vector3.right, SunSpeed * Time.deltaTime);
        currentMoonRotation = transform.GetChild(0).rotation.eulerAngles.x;

        //Moon rotation update
        pastDayRotation = transform.GetChild(1).rotation.eulerAngles.x;
        transform.GetChild(1).RotateAround(transform.GetChild(2).transform.position, Vector3.right, MoonSpeed * Time.deltaTime);
        currentDayRotation = transform.GetChild(1).rotation.eulerAngles.x;
    }

    float NewLight(Light CurrentLight, float pastRotation, float currentRotation)
    {
        
        if (currentRotation < 180)
        {
            if (currentRotation > pastRotation)
            {
                currentRotation -= 90;
                currentRotation = currentRotation * -1;
                currentRotation += 90;
            }
        }
        else
        {
            if (currentRotation > pastRotation)
            {
                currentRotation -= 270;
                currentRotation = currentRotation * -1;
                currentRotation += 270;
            }
        }
        return currentRotation;
    }

    int changeDay(int TimeOfLight, float currentRotation)
    {
        if (ToNextPArtOfTheDay.Count - 1 <= TimeOfLight)
        {
            if ((ToNextPArtOfTheDay[TimeOfLight] >= currentRotation) &&
                    (ToNextPArtOfTheDay[0] >= currentRotation))
            {
                if (TimeOfLight == TimeOfDayMoon)
                {
                    ChangeTimeOfDay(Moon);
                }
                else
                {
                    ChangeTimeOfDay(Sun);
                }
                TimeOfLight = 0;
               
            }
        }
        else
        {

            if ((ToNextPArtOfTheDay[TimeOfLight] >= currentRotation) &&
                (ToNextPArtOfTheDay[TimeOfLight + 1] <= currentRotation))
            {
                
                if (TimeOfLight == TimeOfDayMoon)
                {
                    ChangeTimeOfDay(Moon);
                }
                else
                {
                    ChangeTimeOfDay(Sun);
                }
                TimeOfLight++;
               
            }
        }
        return TimeOfLight;
    }

    void ChangeTimeOfDay(Light light)
    {
        if (TimeOfDay == CurrentTime.Dusk)
        {
            TimeOfDay = 0;
        }
        else
        {
            TimeOfDay++;
        }
        light.GetComponent<IntensityOfCycle>().SetCurrentLight();
    }
}
