﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BasicAttack", order = 1)]

public class BasicAttack : SkillData
{
    //Indicator

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeBeenOnCooldown += cooldown;
    }

    public void DrawRangeIndicator(Transform zoneStart)
    {
        radialRangeIndicator.DrawIndicator(zoneStart, angle, 0.0f, range);
    }

    public override void CastSkill(Transform zoneStart)
    {
        currentlyCasting = true;

        DrawRangeIndicator(zoneStart);
        float drawPercent = (timeSpentOnWindUp / windUp) * range;
        radialRangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, drawPercent);
        // Increment the time spent winding up the skill
        timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= windUp)
        {
            ActivateSkill();
            timeSpentOnWindUp = 0.0f;
            currentlyCasting = false;
        }
    }

    void ActivateSkill()
    {
        Debug.Log("KOBE!");
    }
}
