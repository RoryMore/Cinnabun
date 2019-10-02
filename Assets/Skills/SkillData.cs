using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public enum DamageType
    {
        PHYSICAL,
        MAGICAL
    }

    public enum SkillShape
    {
        RADIAL,
        LINE
    }

    public enum SkillList  // Go through different Skills with team - discuss. Only small amounts of discussion on skills have been had. Think about Skill tree as well. How many skills we'll have, what "category" they are, if they're all unlocked through the skill tree, or if they're unlocked by doing something in the world or killing something specific
    {
        TELEPORT
    }

    [Tooltip("The maximum distance the skill can be used at from the casters position")]
    public float range;

    [Tooltip("The time (in seconds) that needs to pass before the player is able to cast this skill again")]
    public float cooldown;
    [HideInInspector]
    public float timeBeenOnCooldown = 10.0f;

    [Tooltip("The time (in seconds) it will take to cast the skill before it does any effect")]
    public float windUp;
    [HideInInspector]
    public float timeSpentOnWindUp = 0;

    [Tooltip("The 'shape' that this skill will be.\n Radial: Uses Angle and Range values to determine the area from a point it will affect.\n Line: Uses Width and Range values to determine the area from a point it will affect.")]
    public SkillShape shape;

    [Tooltip("Skills that require an angle are skills that use Radial shape.")]
    public float angle;

    [Tooltip("Skills that require a width are skills that use Line shape.")]
    public float width;

    [Tooltip("Which skill this actually is")]
    public SkillList skill;

    [Header("Damage Variables")]
    [Tooltip("The base amount before any additional calculations for how much damage this skill may deal")]
    public int baseDamage;

    [Tooltip("Whether this skill deals Physical or Magical damage")]
    public DamageType damageType;

    [HideInInspector]
    public bool currentlyCasting = false;

    public bool CheckLineSkillHit(Vector3 hitCheckPosition, Mesh lineIndicatorMesh)
    {
        // This checks if the x & z values are within the bounds of the rectangular mesh -> Returns true if hit, false if not
        // Can add additional checks for y component of position if necessary to add to make height checks relevant
        Vector3 meshYPosition = new Vector3(hitCheckPosition.x, lineIndicatorMesh.bounds.center.y, hitCheckPosition.z);
        if (lineIndicatorMesh.bounds.Contains(meshYPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckRadialSkillHit(Vector3 hitCheckPosition, Transform zoneStart)
    {
        // Based on zoneStart (where the radial skill area is starting from with a given rotation) and the position we are checking
        // Returns whether hitCheckPosition is within the arc area of the radial skill
        // Note: zoneStart is equivelent to the zoneStart parameter used for the radial skill indicator
        float forwardAngle = 90 - Mathf.Rad2Deg * Mathf.Atan2(zoneStart.forward.z, zoneStart.forward.x);

        float positionAngle = Vector3.Angle(hitCheckPosition - zoneStart.position, zoneStart.forward);
        float distance = Vector3.Distance(hitCheckPosition, zoneStart.position);

        if (positionAngle <= angle)
        {
            if (distance <= range)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
