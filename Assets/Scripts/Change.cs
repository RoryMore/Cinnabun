using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change : MonoBehaviour
{
    public DayNight.CurrentTime EffectedTime;
    bool isEffect = false;
    // Start is called before the first frame update
   
    public void change(DayNight.CurrentTime CurrentTime) {
        if (CurrentTime == EffectedTime)
        {
            isEffect = true;
        }
        else
        {
            isEffect = false;
        }
    }
    bool GetIsEffected()
    {
        return isEffect;
    }
}
