using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Teleport", order = 1)]

public class Teleport : SkillData
{
    GameObject target = null;
    Vector3 intendedPosition;
    

    //public override void CastSkill(Transform zoneStart, SkillShape shape)
    //{
    //    Debug.Log("Entered Skill");
    //    //Draw Circle for player to use
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    //Vector3 intendedPosition = new Vector3();

    //    DrawRangeIndicator(zoneStart, shape);

    //    //Check if there are any enemies in range
    //    //initiativeList.AddRange(GameObject.FindGameObjectsWithTag("EnemyTeleSlowInitswappable"));
    //    //foreach (Enemy enemy in initiativeList)
    //    //CheckInRange(zoneStart, enemy.transform.position)

    //    //Inquire as to the target of their choice


    //    //STEP 1: Pick your target
    //    if (target == null)
    //    {
    //        if (Physics.Raycast(ray, out RaycastHit hit, 400))
    //        {
    //            //If the target inherets from entity and is able to be teleported
    //            if (hit.collider.gameObject.GetComponent<Entity>().cannotBeTeleported != true)
    //            {
    //                //Visual for selecton here


    //                if (Input.GetMouseButtonDown(0))
    //                {

    //                    // Is target in range
    //                    if (CheckInRange(zoneStart.position, hit.point))
    //                    {
    //                        target = hit.collider.gameObject;
    //                        Debug.Log("First Target selected");
    //                    }

    //                    //float distanceFromPlayer = Vector3.Distance(hit.collider.gameObject.transform.position, zoneStart.position);
    //                    //if (distanceFromPlayer <= range)
    //                    //{
    //                    //    // Set First target
    //                    //    target = hit.collider.gameObject;
    //                    //    Debug.Log("First Target selected");

    //                    //}
    //                }
    //                else
    //                {
    //                    Debug.Log("Invalid Target: Tagged as immune to teleport");
    //                }
    //            }

    //        }
    //    }

    //    //Step 2: Pick your Destination
    //    else if (target != null)
    //    {
    //        if (Physics.Raycast(ray, out RaycastHit hit2, 400))
    //        {

    //            //Visual for selecton here
    //            if (Input.GetMouseButtonDown(0))
    //            {

    //                //Is target in range
    //                float distanceFromPlayer = Vector3.Distance(hit2.point, zoneStart.position);
    //                if (distanceFromPlayer <= range)
    //                {
    //                    //Set Destination
    //                    intendedPosition = hit2.point;
    //                    Debug.Log("Destination Selected");

    //                }
    //                else
    //                {
    //                    Debug.Log("Not in range");
    //                }
    //            }


    //        }
    //    }




    //    //Check if target is valid (Is a gameObject and has the Teleportable bool from entity)

    //    //If target is valid, prompt player to pick a random point within range

    //    //If both targets are valid, move target to position and deal a small amount of damage

    //    //

    //    if (target != null)
    //    {
    //        if (intendedPosition != null)
    //        {
    //            float drawPercent = (timeSpentOnWindUp / windUp);
    //            rangeIndicator.DrawCastTimeIndicator(zoneStart, angleWidth, 0.0f, range, drawPercent);
    //            //rectangleRangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, drawPercent, range);

    //            // Increment the time spent winding up the skill
    //            timeSpentOnWindUp += Time.deltaTime;

    //            // When the skill can be activated
    //            if (timeSpentOnWindUp >= windUp)
    //            {
    //                ActivateSkill(target,intendedPosition);
    //                timeSpentOnWindUp = 0.0f;
    //                currentlyCasting = false;
    //            }
    //        }
    //    }



    //}

    void ActivateSkill(GameObject target, Vector3 targetPosition)
    {
        timeBeenOnCooldown = 0.0f;
        // What happens when the skill is activated

        target.transform.position = targetPosition;
        target = null;
        //Target position is remade every time the skill is activated so no need to reset/null
        Debug.Log("Activated!");

    }
}


