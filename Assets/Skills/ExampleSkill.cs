using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExampleSkill", order = 1)]
public class ExampleSkill : SkillData
{
    RectangleRangeIndicator rectRangeIndicator;
    RadialRangeIndicator coneRangeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        rectRangeIndicator.Init();
        coneRangeIndicator.Init(angle);
    }

    // Update is called once per frame
    void Update()
    {
        timeBeenOnCooldown += cooldown;
    }

    // This function can be split into two functions for cast time indicators and maxRange indicators
    // Called by the entity casting the skill
    public void DrawRangeIndicator(Transform zoneStart)
    {
        // How to draw the basic range indicators.
        // Parameters:  Transform zoneStart = where the skill indicator will start drawing from with a given rotation
        //              float width/angle = how wide a line indicator is, or what angle a radial indicator will use to draw
        //              float minRange = where the indicator will start drawing from. Essentially an offset if we want space between the caster and skill area
        //                              using the normal point direction for the skill
        //              float maxRange = the maximum range the skill indicator will extend
        rectRangeIndicator.DrawIndicator(zoneStart, width, 0.0f, range);
        coneRangeIndicator.DrawIndicator(zoneStart, angle, 0.0f, range);

        float drawPercent = timeSpentOnWindUp / windUp;
        rectRangeIndicator.DrawCastTimeIndicator(zoneStart, width, 0.0f, drawPercent, range);
        coneRangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, drawPercent);
    }

    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter
    public void CastSkill(Transform zoneStart)
    {
        DrawRangeIndicator(zoneStart);
        // Increment the time spent winding up the skill
        timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= windUp)
        {
            ActivateSkill();
            timeSpentOnWindUp = 0.0f;
        }
    }

    void ActivateSkill()
    {
        // What happens when the skill is activated
    }
}
