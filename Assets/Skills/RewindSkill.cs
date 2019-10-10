using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RewindSkill", order = 1)]


public class RewindSkill : SkillData
{

    Entity entity = null;
    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter

    public override void TargetSkill(Transform zoneStart)
    {
        if (entity == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                Vector3 lookAt = new Vector3(hit.point.x, zoneStart.position.y, hit.point.z);
                zoneStart.LookAt(lookAt);
            }

            DrawRangeIndicator(zoneStart, shape);
            SelectTargetRay(zoneStart, ref entity);
            
        }
        else
        {
            CastSkill(zoneStart);
        }
    }

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
            ActivateSkill();
            timeSpentOnWindUp = 0.0f;
            currentlyCasting = false;
        }
    }

    protected override void ActivateSkill()
    {
        timeBeenOnCooldown = 0.0f;
        entity.RewindBack();

        entity = null;
    }
}
