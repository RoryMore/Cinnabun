using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BasicAttack", order = 1)]

public class BasicAttack : SkillData
{

    Entity target = null;
    
    public int damage;

    public override void TargetSkill(Transform zoneStart, List<Entity> entityList)
    {
        //This.. may not work!
        target = GameObject.FindWithTag("Player").GetComponent<Entity>();

        //Init
        //rangeIndicator.Init(SkillShape.RADIAL, 90.0f);

        //Face target
        zoneStart.LookAt(target.transform);
        //DrawRangeIndicator(zoneStart, shape, range, 90.0f);

        //Select?
        //SelectTargetRay(zoneStart, ref target, true);


        CastSkill(zoneStart, entityList); 

    }

    protected override void CastSkill(Transform zoneStart, List<Entity> entityList)
    {
        currentlyCasting = true;

        float drawPercent = (timeSpentOnWindUp / windUp);



        DrawRangeIndicator(zoneStart, SkillShape.RADIAL, range, angleWidth);

        rangeIndicator.DrawCastTimeIndicator(zoneStart, angleWidth, 0.0f, range, drawPercent);



        timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= windUp)
        {
            currentlyCasting = false;
            ActivateSkill(zoneStart, entityList);
            timeSpentOnWindUp = 0.0f;
        }
    }

    protected override void ActivateSkill(Transform zoneStart, List<Entity> entityList)
    {
        timeBeenOnCooldown = 0.0f;

        foreach (Entity testedEntity in entityList)
        {
            if (CheckRadialSkillHit(testedEntity.transform.position, zoneStart))
            {
                if (testedEntity != caster)
                {
                    testedEntity.TakeDamage(damage);
                }
                
            }
        }

        target = null;
        

    }

}
