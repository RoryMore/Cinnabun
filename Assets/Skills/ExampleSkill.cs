using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skills/ExampleSkill", order = 1)]
public class ExampleSkill : SkillData
{

    public override void TargetSkill(Transform zoneStart)
    {
        // What things need to be done here while targetting right before casting.
        // Generally used for setting targetted entities or points that the skill utilises
    }

    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter
    protected override void CastSkill(Transform zoneStart)
    {
        currentlyCasting = true;

        //DrawRangeIndicator(zoneStart, shape);

        float drawPercent = (timeSpentOnWindUp / windUp);
        rangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, maxRange, drawPercent);

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

    protected override void ActivateSkill()
    {
        timeBeenOnCooldown = 0.0f;
        // What happens when the skill is activated
        
    }
}
