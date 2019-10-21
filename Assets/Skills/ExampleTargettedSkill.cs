using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ExampleTargettedSkill", order = 1)]
public class ExampleTargettedSkill : SkillData
{
    Entity entityTarget1 = null;
    Vector3 teleportLocation;
    bool destination1Set = false;

    public override void TargetSkill(Transform zoneStart)
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
        // If entity target has been set but our destination point hasn't
        else if (!destination1Set && entityTarget1 != null)
        {
            // We draw a range indicator if we feel it's necessary
            DrawRangeIndicator(zoneStart, shape);

            // The overloaded version of SelectTargetRay that sets a vector position also returns a bool
            // This is so we can set a bool saying we have set a specific location, otherwise
            // there aren't other succinct ways to check if we have set a destination point
            destination1Set = SelectTargetRay(zoneStart, ref teleportLocation, true);
        }
        // If we have an entity set and we have set our destination point
        else if (destination1Set && entityTarget1 != null)
        {
            // Start casting the skill
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
            currentlyCasting = false;
            ActivateSkill();
            timeSpentOnWindUp = 0.0f;
        }
    }

    protected override void ActivateSkill()
    {
        timeBeenOnCooldown = 0.0f;
        // What happens when the skill is activated

        // Deal damage to our entity target
        entityTarget1.TakeDamage(baseDamage);   // End up passing in damage type later

        // We move our entity target to our destination point we are moving it
        entityTarget1.transform.position = teleportLocation;

        // Don't forget to nullify your targets after use
        // and reset any necessary values
        entityTarget1 = null;
        teleportLocation = Vector3.zero;
        destination1Set = false;
    }
}
