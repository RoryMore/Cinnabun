﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExampleSkill", order = 1)]
public class ExampleSkill : SkillData
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function can be split into two functions for cast time indicators and maxRange indicators
    // Called by the entity casting the skill
    

    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter
    public override void CastSkill(Transform zoneStart, SkillShape shape)
    {
        DrawRangeIndicator(zoneStart, shape);

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
        timeBeenOnCooldown = 0.0f;
        // What happens when the skill is activated
    }
}
