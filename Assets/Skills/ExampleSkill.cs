using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExampleSkill", order = 1)]
public class ExampleSkill : SkillData
{
    public RadialRangeIndicator coneRangeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        coneRangeIndicator.Init(angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeenOnCooldown < cooldown)
        {
            timeBeenOnCooldown += cooldown;
        }
    }

    // This function can be split into two functions for cast time indicators and maxRange indicators
    // Called by the entity casting the skill
    public void DrawRangeIndicator(Transform zoneStart)
    {
        coneRangeIndicator.DrawIndicator(zoneStart, angle, 0.0f, range);
    }

    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter
    public override void CastSkill(Transform zoneStart)
    {
        //currentlyCasting = true;

        DrawRangeIndicator(zoneStart);
        float drawPercent = (timeSpentOnWindUp / windUp) * range;
        coneRangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, drawPercent);
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
        // What happens when the skill is activated
    }
}
