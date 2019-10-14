using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DelayedBlast", order = 1)]

public class DelayedBlast : SkillData
{
    Entity entityTarget1 = null;
    int numOfDelayedBlasts = 0;

    public float explosionRadius;
    

    public override void TargetSkill(Transform zoneStart, List<Entity> entityList)
    {
        // Entity is not set; therefore we need to wait until the user has set the entity
        if (entityTarget1 == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                Vector3 lookAt = new Vector3(hit.point.x, zoneStart.position.y, hit.point.z);
                zoneStart.LookAt(lookAt);
            }

            // We are drawing the range indicator here so the player knows if what they are clicking is in range
            // If being in range is relevant
            DrawRangeIndicator(zoneStart, shape);

            // Select our entity target
            SelectTargetRay(zoneStart, ref entityTarget1, true);
            // The true value in the SelectTargetRay function is specifying that we want to also make a check
            // to see if the target is in range
            // We can leave that extra field blank, or false, if we don't want to make that check
        }
        else if (entityTarget1 != null)
        {
            // Start casting the skill
            CastSkill(zoneStart, entityList);
        }

    }

    protected override void CastSkill(Transform zoneStart, List<Entity> entityList)
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
            ActivateSkill(entityList);
            timeSpentOnWindUp = 0.0f;
        }
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        timeBeenOnCooldown = 0.0f;
        numOfDelayedBlasts = 0;

        //Assuming the list is empty, give it a delayed blast condition
        if (entityTarget1.ReturnConditions().Count == 0)
        {
            Entity.Condition delayedBlast = new Entity.Condition();
            delayedBlast.conditionType = Entity.ConditionType.DELAYEDBLAST;
            delayedBlast.duration = 9.0f;
            entityTarget1.currentConditions.Add(delayedBlast);
            Debug.Log("Tick tick tick...");
            entityTarget1 = null;

        }
        else
        {

            for (int i = 0; i < entityTarget1.currentConditions.Count; i++)
            {
                if (entityTarget1.currentConditions[i].conditionType == Entity.ConditionType.DELAYEDBLAST)
                {
                    entityTarget1.currentConditions.Remove(entityTarget1.currentConditions[i]);
                    //entityTarget1.TakeDamage(baseDamage);

                    if (entityList != null)
                    {
 
                        //Deal splash damage to enemies
                        rangeIndicator.DrawIndicator(entityTarget1.transform, 360, 0, explosionRadius);
                        foreach (Entity enemy in entityList)
                        {
                            //Make sure we don't deal double damage
                            //if (enemy == entityTarget1)
                            //{
                                //break;
                            //}
                            if (Vector3.Distance(enemy.transform.position, entityTarget1.transform.position) < explosionRadius)
                            {
                                enemy.TakeDamage(baseDamage);
                            }
                        }

                    }


                    numOfDelayedBlasts++;
                    Debug.Log("BOOM!");
                    entityTarget1 = null;
                    break;
                }
            }

            if (numOfDelayedBlasts == 0)
            {
            Entity.Condition delayedBlast = new Entity.Condition();
            delayedBlast.conditionType = Entity.ConditionType.DELAYEDBLAST;
            delayedBlast.duration = 9.0f;
            entityTarget1.currentConditions.Add(delayedBlast);
            Debug.Log("Tick tick tick...");
            entityTarget1 = null;
            }
        }
    }
}
