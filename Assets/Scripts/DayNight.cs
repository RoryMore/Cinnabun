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
    //types of day when the sun is up. the moon uses the same time slot as
    public enum CurrentTime
    {
        Night,
        Dawn,
        Morning,
        MidDay,
        Afternoon,
        Dusk,
       
    }

    //tells when the time of day changes in degrees
    [Tooltip("Required the same amount of slots as CurrentTime, from larges to smallest")] 
    public List<float> ToNextPartOfTheDay;
    //list of times the sun/moon should take between each CurrentTime zones
    public List<float> SunMoonTimes;
    //time it takes to do a full day and night in minutes
    [Tooltip("In minutes")] 
    public float timer;
    //CurrentTime for sun and moon
    CurrentTime TimeOfDay;
     CurrentTime TimeOfNight;

    //what type of way the sun and moon detains how fast they are going
    public MethodOfMoving MovingMent;

    //speed
    public float MoonSpeed;
    public float SunSpeed;

    [Tooltip("List of gameobjects which get effected by the different CurrentTime, the sun is in")]
    public List<GameObject> SunEffectList;
    [Tooltip("List of gameobjects which get effected by the different CurrentTime, the moon is in")]
    public List<GameObject> MoonEffectList;
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
       //set speed
       transform.GetChild(0).LookAt(transform.GetChild(2).transform);
        transform.GetChild(1).LookAt(transform.GetChild(2).transform);

        if (MovingMent == MethodOfMoving.Timer)
        {
            MoonSpeed = 360 * (timer / 60);
            SunSpeed = 360 * (timer / 60);
        }
        if (MovingMent == MethodOfMoving.MultTime)
        {
            MoonSpeed = 360 * (SunMoonTimes[(int)TimeOfNight] / 60);
            SunSpeed = 360 * (SunMoonTimes[(int)TimeOfDay] / 60);
        }


        //change rotation
        UpdateRotation();

        //set rotation 
        currentMoonRotation = NewPostion(Moon, pastMoonRotation, currentMoonRotation);
        currentDayRotation = NewPostion(Sun, pastDayRotation, currentDayRotation);

        //check to see if the sun or moon need to change their time of day
        TimeOfNight = changeTimeOfDay(TimeOfNight, currentMoonRotation);
        TimeOfDay = changeTimeOfDay(TimeOfDay, currentDayRotation);
    }
    //update the current rotation and get all the numbers for the sun and moon NewPostion
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

    //figing out the current rotation. 0-360 degrees
    float NewPostion(Light CurrentLight, float pastRotation, float currentRotation)
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

    //see if the time of day has changed
    CurrentTime changeTimeOfDay(CurrentTime TimeOfLight, float currentRotation)
    {
        if (ToNextPartOfTheDay.Count - 1 <= (int)TimeOfLight)
        {
            //in range of new CurrentTime
            if ((ToNextPartOfTheDay[(int)TimeOfLight] >= currentRotation) &&
                    (ToNextPartOfTheDay[0] >= currentRotation))
            {
                //change CurrentTime of moon or sun
                if (TimeOfLight == TimeOfNight)
                {
                    ChangeTimeOfDay(Moon, TimeOfNight);
                    ChangeTime(false);
                }
                else
                {
                    ChangeTimeOfDay(Sun, TimeOfDay);
                    ChangeTime(true);
                }
                TimeOfLight = 0;
               
            }
        }
        else
        {
            //in range of new CurrentTime
            if ((ToNextPartOfTheDay[(int)TimeOfLight] >= currentRotation) &&
                (ToNextPartOfTheDay[(int)TimeOfLight + 1] <= currentRotation))
            {
                //change CurrentTime of moon or sun
                if (TimeOfLight == TimeOfNight)
                {
                    ChangeTimeOfDay(Moon, TimeOfNight);
                    ChangeTime(false);
                }
                else
                {
                    ChangeTimeOfDay(Sun, TimeOfDay);
                    ChangeTime(true);
                }
                TimeOfLight++;
               
            }
        }
        return TimeOfLight;
    }
    
    //tells the sun or moon to move onto th new lighting and update interal CurrentTime
    CurrentTime ChangeTimeOfDay(Light light, CurrentTime TimeOf)
    {
        if (TimeOf == CurrentTime.Night)
        {
            TimeOf = 0;
        }
        else
        {
            TimeOf++;
        }
        light.GetComponent<LightingIntensity>().SetCurrentLight();

        return TimeOf;
    }
    //tell all gameobects that the sun or moon is in a new time zone
    public void ChangeTime(bool sun)
    {
        if (sun)
        {
            foreach (GameObject item in SunEffectList)
            {
                item.GetComponent<Change>().change(TimeOfDay);
            }
        }
        else
        {
            foreach (GameObject item in MoonEffectList)
            {
                item.GetComponent<Change>().change(TimeOfNight);
            }
        }
        
    }
}
