using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExampleTargettedSkill", order = 1)]
public class ExampleTargettedSkill : SkillData
{
    Entity entityTarget1 = null;
    Vector3 teleportLocation;

    public override void TargetSkill(Transform zoneStart)
    {
        if (entityTarget1 == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                Vector3 lookAt = new Vector3(hit.point.x, zoneStart.position.y, hit.point.z);
                zoneStart.LookAt(lookAt);
            }

            DrawRangeIndicator(zoneStart, shape);
            SelectTargetRay(zoneStart, ref entityTarget1);
        }
        else
        {
            CastSkill(zoneStart);
        }
        
    }

    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter
    protected override void CastSkill(Transform zoneStart)
    {
        currentlyCasting = true;

        DrawRangeIndicator(zoneStart, shape);

        float drawPercent = (timeSpentOnWindUp / windUp);
        rangeIndicator.DrawCastTimeIndicator(zoneStart, angleWidth, 0.0f, range, drawPercent);

        // Increment the time spent winding up the skill
        timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= windUp)
        {
            currentlyCasting = false;
            ActivateSkill();
            timeSpentOnWindUp = 0.0f;
        }
    }

    protected override void ActivateSkill()
    {
        

        timeBeenOnCooldown = 0.0f;
        // What happens when the skill is activated

        // Very basic dealt damage
        entityTarget1.TakeDamage(baseDamage);   // End up passing in damage type later

        // Don't forget to nullify your targets after use
        entityTarget1 = null;
    }
}
