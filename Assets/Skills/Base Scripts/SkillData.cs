using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public enum DamageType
    {
        PHYSICAL,
        MAGICAL
    }

    public enum SkillShape  // Used to specify what hit detection will be used
    {
        RADIAL,
        LINE
    }

    public enum SkillList
    {
        TELEPORT,
        DELAYEDBLAST,
        REWIND,
        NotAppliccable
    }

    [Tooltip("The maximum distance the skill can be used at from the casters position")]
    public float range;

    [Tooltip("The maximum height difference allowed between units that makes it possible to be hit or not")]
    public float verticalRange;

    [Tooltip("The time (in seconds) that needs to pass before the player is able to cast this skill again")]
    public float cooldown;
    [HideInInspector]
    public float timeBeenOnCooldown = 10.0f;    // REMOVE FOR CHANGE - IS IN NEW BASE SKILL

    [Tooltip("The time (in seconds) it will take to cast the skill before it does any effect")]
    public float windUp;
    [HideInInspector]
    public float timeSpentOnWindUp = 0; // REMOVE FOR CHANGE - IS IN NEW BASE SKILL

    [Tooltip("The 'shape' that this skill will be.\n Radial: Uses Angle and Range values to determine the area from a point it will affect.\n Line: Uses Width and Range values to determine the area from a point it will affect.")]
    public SkillShape shape;    // TENTATIVELY KEPT - TO KNOW WHAT SORT OF HIT DETECTION WE REQUIRE FOR CHECKING HITS

    [Tooltip("The Width a line Skill will have, or the Angle a Radial skill will use")]
    public float angleWidth;    // TENTATIVELY KEPT - EASY USE HERE (DATA DRIVEN DESIGN)
                                // POSSIBLY GOING TO BE REMOVED AND EACH SKILL SHOULD SPECIFY WHAT LENGTHS/WIDTHS/ANGLES THEY REQIURE

    [Tooltip("Which skill this actually is")]
    public SkillList skill; // TENTATIVELY KEPT - USEFUL FOR KNOWING WHICH SKILL IS BEING CAST

    [Header("Damage Variables")]
    [Tooltip("The base amount before any additional calculations for how much damage this skill may deal")]
    public int baseDamage;

    [Tooltip("Whether this skill deals Physical or Magical damage")]
    public DamageType damageType;

    [Header("Indicator")]
    public RangeIndicator rangeIndicator = null;    // REMOVE FOR CHANGE - INDICATORS ARE PROJECTORS
    public Material indicatorMaterial;              // REMOVE FOR CHANGE

    [HideInInspector]
    public bool currentlyCasting = false;   // REMOVE FOR CHANGE - IS IN NEW BASE SKILL

    // The entity that casts this skill
    [HideInInspector]
    public Entity caster = null;    // REMOVE FOR CHANGE - ADD TO SKILL SCRIPTS THAT REQUIRE IT

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

    public void ProgressCooldown()  // Handled in new base skill Update
    {
        if (timeBeenOnCooldown < cooldown)
        {
            timeBeenOnCooldown += Time.deltaTime;
        }
    }

    // FUNCTION MOVED AND REWORKED IN BASE SKILL
    public bool CheckLineSkillHit(Vector3 hitCheckPosition)
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

    // FUNCTION MOVED TO NEW BASE SKILL
    public bool CheckRadialSkillHit(Vector3 hitCheckPosition, Transform zoneStart)
    {
        // Based on zoneStart (where the radial skill area is starting from with a given rotation) and the position we are checking
        // Returns whether hitCheckPosition is within the arc area of the radial skill
        // Note: zoneStart is equivelent to the zoneStart parameter used for the radial skill indicator
        //float forwardAngle = 90 - Mathf.Rad2Deg * Mathf.Atan2(zoneStart.forward.z, zoneStart.forward.x);
        //Debug.Log("ForwardAngle = " + forwardAngle);
        float positionAngle = Vector3.Angle(hitCheckPosition - zoneStart.position, zoneStart.forward);
        float distance = Vector3.Distance(hitCheckPosition, zoneStart.position);
        //Debug.Log("Target distance: " + distance);
        //Debug.Log("PositionAngle = " + positionAngle);
        if (positionAngle <= angleWidth)
        {
            if (distance <= range)
            {
                Debug.Log("Radial Skill Hit!");
                return true;
            }
            else
            {
                Debug.Log("Radial Skill out of range from target");
                return false;
            }
        }
        else
        {
            Debug.Log("Radial Skill target not within specified angle");
            return false;
        }
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

    public bool CheckInRange(Vector3 castPosition, Vector3 targetPosition)  // HANDLED IN NEW BASE SKILL
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

    // HANDLED/MOVED TO NEW BASE SKILL
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

    // HANDLED/MOVED TO NEW BASE SKILL
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

    // HANDLED/MOVED TO NEW BASE SKILL
    protected virtual void CastSkill(Transform zoneStart) { }

    // HANDLED/MOVED TO NEW BASE SKILL
    protected virtual void CastSkill(Transform zoneStart, List<Entity> entityList) { }

    // HANDLED/MOVED TO NEW BASE SKILL
    protected virtual void ActivateSkill() { }

    // HANDLED/MOVED TO NEW BASE SKILL
    protected virtual void ActivateSkill(Transform zoneStart, List<Entity> entityList) { }

    // HANDLED/MOVED TO NEW BASE SKILL
    protected virtual void ActivateSkill(List<Entity> entityList) { }

    // HANDLED/MOVED TO NEW BASE SKILL
    public virtual void TargetSkill(Transform zoneStart) { }

    // HANDLED/MOVED TO NEW BASE SKILL
    public virtual void TargetSkill(Transform zoneStart, List<Entity> entityList) { }
}
