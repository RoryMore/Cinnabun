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
        TELEPORT,
        DELAYEDBLAST,
        REWIND,
        NotAppliccable
    }

    [Tooltip("The maximum distance the skill can be used at from the casters position")]
    public float range;

    [Tooltip("How high above the casting transform the skill will check if it will hit a target on the Y axis. A value of zero will mean the targets position Y value has to be equivelant to the Y value of the Indicator/caster Transform to be damaged")]
    public float positiveRangeHeight;
    [Tooltip("How low below the casting transform the skill will check if it will hit a target on the Y axis. A value of zero will mean the targets position Y value has to be equivelant to the Y value of the Indicator/caster Transform to be damaged")]
    public float negativeRangeHeight;

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

    [Tooltip("The Width a line Skill will have, or the Angle a Radial skill will use")]
    public float angleWidth;

    [Tooltip("Which skill this actually is")]
    public SkillList skill;

    [Header("Damage Variables")]
    [Tooltip("The base amount before any additional calculations for how much damage this skill may deal")]
    public int baseDamage;

    [Tooltip("Whether this skill deals Physical or Magical damage")]
    public DamageType damageType;

    [Header("Indicator")]
    public RangeIndicator rangeIndicator = null;
    public Material indicatorMaterial;

    [HideInInspector]
    public bool currentlyCasting = false;

    // The entity that casts this skill
    [HideInInspector]
    public Entity caster = null;

    public virtual void Initialise()
    {
        rangeIndicator = new RangeIndicator();
        rangeIndicator.Init(shape, angleWidth);
        rangeIndicator.indicatorMaterial = indicatorMaterial;

        timeBeenOnCooldown = cooldown;
        timeSpentOnWindUp = 0.0f;
    }

    // This virtual method is only used by the players WeaponAttack skill to make sure they can't punch themselves
    public virtual void Initialise(Entity ownCaster)
    {
        rangeIndicator = new RangeIndicator();
        rangeIndicator.Init(shape, angleWidth);
        rangeIndicator.indicatorMaterial = indicatorMaterial;

        caster = ownCaster;

        timeBeenOnCooldown = cooldown;
        timeSpentOnWindUp = 0.0f;
    }

    public void ProgressCooldown()
    {
        if (timeBeenOnCooldown < cooldown)
        {
            timeBeenOnCooldown += Time.deltaTime;
        }
    }

    public bool CheckLineSkillHit(Vector3 hitCheckPosition)
    {
        // Check if our target is within the height limits of our skill range
        if (hitCheckPosition.y <= rangeIndicator.mesh.bounds.center.y + positiveRangeHeight)
        {
            if (hitCheckPosition.y >= rangeIndicator.mesh.bounds.center.y - negativeRangeHeight)
            {
                // This checks if the x & z values are within the bounds of the rectangular mesh -> Returns true if hit, false if not
                // Can add additional checks for y component of position if necessary to add to make height checks relevant
                Vector3 meshYPosition = new Vector3(hitCheckPosition.x, rangeIndicator.mesh.bounds.center.y, hitCheckPosition.z);
                if (rangeIndicator.mesh.bounds.Contains(meshYPosition))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    public bool CheckRadialSkillHit(Vector3 hitCheckPosition, Transform zoneStart)
    {
        // Check if our target is within the height limits of our skill range
        if (hitCheckPosition.y <= zoneStart.position.y + positiveRangeHeight)
        {
            if (hitCheckPosition.y >= zoneStart.position.y - negativeRangeHeight)
            {
                // Based on zoneStart (where the radial skill area is starting from with a given rotation) and the position we are checking
                // Returns whether hitCheckPosition is within the arc area of the radial skill
                // Note: zoneStart is equivelent to the zoneStart parameter used for the radial skill indicator
                float forwardAngle = 90 - Mathf.Rad2Deg * Mathf.Atan2(zoneStart.forward.z, zoneStart.forward.x);

                float positionAngle = Vector3.Angle(hitCheckPosition - zoneStart.position, zoneStart.forward);
                float distance = Vector3.Distance(hitCheckPosition, zoneStart.position);

                if (positionAngle <= angleWidth)
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
        return false;
    }

    public void DrawRangeIndicator(Transform zoneStart, SkillShape shape)
    {
        rangeIndicator.DrawIndicator(zoneStart, angleWidth, 0.0f, range);
        //switch (shape)
        //{
        //    case SkillShape.LINE:
        //        rectangleRangeIndicator.DrawIndicator(zoneStart, angleWidth, 0.0f, range);
        //        break;
        //
        //    case SkillShape.RADIAL:
        //        radialRangeIndicator.DrawIndicator(zoneStart, angleWidth, 0.0f, range);
        //        break;
        //    default:
        //        break;
        //}
    }

    // Used to draw an outline circle, instead of a filled circle
    protected void DrawRangeIndicator(Transform zoneStart, SkillShape shape, float maxRange, float angle)
    {
        rangeIndicator.DrawIndicator(zoneStart, angle, 0.0f, maxRange);
    }

    public bool CheckInRange(Vector3 castPosition, Vector3 targetPosition)
    {
        // If the targets position is within the range of the skill,
        // Return true
        if (Vector3.Distance(castPosition, targetPosition) <= range)
        {
            return true;
        }
        // Targets position from caster position is out of skill range
        return false;
    }

    protected virtual void SelectTargetRay(Transform zoneStart, ref Entity entityToSet, bool checkInRange = false)
    {
        if (entityToSet == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
                {
                    Debug.Log("Skill is raycasting");
                    if (checkInRange)
                    {
                        if (CheckInRange(zoneStart.position, hit.point))
                        {
                            Debug.Log("Entity reference set for skill");
                            entityToSet = hit.collider.gameObject.GetComponent<Entity>();
                        }
                    }
                    else
                    {
                        entityToSet = hit.collider.gameObject.GetComponent<Entity>();
                    }
                }
            }
        }
    }

    protected bool SelectTargetRay(Transform zoneStart, ref Vector3 pointToSet, bool checkInRange = false)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                if (checkInRange)
                {
                    if (CheckInRange(zoneStart.position, hit.point))
                    {
                        Debug.Log("Position reference set for skill");
                        pointToSet = hit.point;
                        return true;
                    }
                }
                else
                {
                    pointToSet = hit.point;
                    return true;
                }
            }
        }
        return false;
    }

    protected virtual void CastSkill(Transform zoneStart) { }

    protected virtual void CastSkill(Transform zoneStart, List<Entity> entityList) { }

    protected virtual void ActivateSkill() { }

    protected virtual void ActivateSkill(Transform zoneStart, List<Entity> entityList) { }

    protected virtual void ActivateSkill(List<Entity> entityList) { }

    public virtual void TargetSkill(Transform zoneStart) { }

    public virtual void TargetSkill(Transform zoneStart, List<Entity> entityList) { }
}
