using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    //varables

    // start up skill
    void awake()
    {
        //spawn target area
        //start animation
        //trigger any hitboxs Requiered
    }

    // 
    void update()
    {
        //any other features of sed skill
    }


    //cancel to resettime can probaly be made into a single funation but keep sperat for now 
    void cancel()
    {

        //EndSkill()
        //set coodown to make 1? 
    }
    void EndSkill()
    {
        //destory all targeted areas
        //end Animation
        //turn of all hitboxs Which were used
        //set coodown to make coodown
    }
    void Interupaded()
    {
        //EndSkill()
        //stun enemy
    }
    void ResetTime()
    {
        //cancel()
        //reset time to ??
    }


}
