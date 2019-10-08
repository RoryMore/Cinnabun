using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Teleport", order = 1)]

public class Teleport : SkillData
{
    Entity entityTarget1 = null;
    Vector3 teleportLocation;
    bool destination1Set = false;

    public override void TargetSkill(Transform zoneStart)
    {
        //If no data has yet been specified
        if (entityTarget1 == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                Vector3 lookat = new Vector3(hit.point.x, zoneStart.position.y, hit.point.z);
                zoneStart.LookAt(lookat);


            }


            DrawRangeIndicator(zoneStart, shape);
            SelectTargetRay(zoneStart, ref entityTarget1, true);


        }
        //If we have our target but no destination
        else if (entityTarget1 != null && !destination1Set)
        {
            DrawRangeIndicator(zoneStart, shape);
            destination1Set = SelectTargetRay(zoneStart, ref teleportLocation, true);


        }
        //If we have both, proceed
        else if(entityTarget1 != null && destination1Set)
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

        timeSpentOnWindUp += Time.deltaTime;

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
        entityTarget1.transform.position = teleportLocation;
        entityTarget1.TakeDamage(baseDamage);

        entityTarget1 = null;
        teleportLocation.Set(0, 0, 0);
        destination1Set = false;
        //Target position is remade every time the skill is activated so no need to reset/null
        Debug.Log("Activated!");

    }
}


