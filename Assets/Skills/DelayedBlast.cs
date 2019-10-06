using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DelayedBlast", order = 1)]

public class DelayedBlast : SkillData
{
    // Start is called before the first frame update
    public override void CastSkill(Transform zoneStart, SkillShape shape)
    {
        DrawRangeIndicator(zoneStart, shape);

        float drawPercent = (timeSpentOnWindUp / windUp);
        rangeIndicator.DrawCastTimeIndicator(zoneStart, angleWidth, 0.0f, range, drawPercent);
        //rectangleRangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, drawPercent, range);

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
