using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public enum SkillState
    {
        INACTIVE,
        TARGETTING,
        CASTING,
        DOAFFECT
    }

    public enum SkillShape  // Used to specify what hit detection will be used
    {
        RADIAL,
        RECTANGULAR
    }

    public enum CastFillType    // Used to specify the Projector Indicator fill type
    {
        LINEAR,
        CIRCULAR
    }

    public enum IndicatorMoveType
    {
        ALWAYSNEARCASTER,
        MOVEABLE
    }

    [SerializeField]
    protected Projector projector;
    Material material;

    public SkillData skillData;

    public SkillState skillState;

    [Tooltip("The 'shape' that this skill will be.\n Radial: Uses Angle and Range values to determine the area from a point it will affect.\n Rectangular: Uses Width and Range values to determine the area from a point it will affect.")]
    public SkillShape shape;    // TENTATIVELY KEPT - TO KNOW WHAT SORT OF HIT DETECTION WE REQUIRE FOR CHECKING HITS

    [Tooltip("How the skill progress is indicated on the projector")]
    public CastFillType fillType;

    [Tooltip("Indicator Moveability for this skill. \nMOVEABLE: For skills that wants the indicator to follow a targeted position for example. \nALWAYSNEARCASTER: Will always stay at the caster and rotate where the caster is facing. Circular Fill type won't use rotation, Linear fill type will rotate where the player is looking")]
    public IndicatorMoveType moveType;

    [HideInInspector]
    public float timeBeenOnCooldown = 10.0f;

    protected float timeSpentOnWindUp = 0;
    [HideInInspector]
    public bool currentlyCasting = false;

    [HideInInspector]
    public bool isAllowedToCast = true;
    protected bool skillTriggered = false;

    [Tooltip("SET CASTER SELF TO PARENT OBJECT. \nE.G: Player object is set to this on the players skills")]
    public Entity casterSelf;

    [Header("Indicator Image Settings")]
    [SerializeField]
    Sprite mainCookie;
    [SerializeField]
    Sprite fillCookie;

    protected virtual void Initialise()
    {
        if (skillData != null)
        {
            timeBeenOnCooldown = skillData.cooldown;
            timeSpentOnWindUp = 0.0f;
            currentlyCasting = false;
        }
        material = new Material(Shader.Find("Projector/Tattoo"));

        material.SetTexture("_ShadowTex", mainCookie.texture);
        material.SetTexture("_FillTex", fillCookie.texture);

        projector.material = material;

        switch (fillType)
        {
            case CastFillType.LINEAR:
                {
                    projector.material.SetInt("_SkillType", 0);
                    break;
                }

            case CastFillType.CIRCULAR:
                {
                    projector.material.SetInt("_SkillType", 1);
                    break;
                }

            default:
                {
                    Debug.LogError("Skill 'fillType' not set?");
                    break;
                }
        }
        skillTriggered = false;
        skillState = SkillState.INACTIVE;
    }

    protected void ResetIndicatorImages()
    {
        projector.material.SetTexture("_ShadowTex", mainCookie.texture);
        projector.material.SetTexture("_FillTex", fillCookie.texture);
    }

    protected void SetFillType(CastFillType castFillType)
    {
        fillType = castFillType;
        switch(fillType)
        {
            case CastFillType.LINEAR:
                {
                    projector.material.SetInt("_SkillType", 0);
                    break;
                }

            case CastFillType.CIRCULAR:
                {
                    projector.material.SetInt("_SkillType", 1);
                    break;
                }
        }
    }

    protected void SetProjectorMoveType(IndicatorMoveType indicatorMoveType)
    {
        moveType = indicatorMoveType;
    }

    protected void EnableProjector()
    {
        if (!projector.enabled)
        {
            projector.enabled = true;
        }
    }

    public void DisableProjector()
    {
        if (projector.enabled)
        {
            projector.enabled = false;
        }
    }

    void UpdateCastTime()
    {
        switch(skillState)
        {
            case SkillState.CASTING:
                {
                    // Increment the delta value for time spent casting ability
                    timeSpentOnWindUp += Time.deltaTime;
                    //Debug.Log("Cast time for Windup being calculated and passed to shader");
                    //Debug.Log(timeSpentOnWindUp / skillData.windUp);
                    
                    break;
                }
        }
        if (projector.enabled)
        {
            switch(fillType)
            {
                case CastFillType.LINEAR:
                    {
                        projector.material.SetFloat("_Progress", (timeSpentOnWindUp / skillData.windUp));
                        break;
                    }

                case CastFillType.CIRCULAR:
                    {
                        // Convert this percentage value to cap at 0.5 rather than 1. Circular fill fills from the centre outward; half as much to fill
                        projector.material.SetFloat("_Progress", (timeSpentOnWindUp / skillData.windUp) * 0.5f);
                        break;
                    }
            }
            
        }
    }

    /// <summary>
    /// Must be used in inherited skills Update method
    /// </summary>
    void UpdateCooldownTime()
    {
        // If the time this skill has been on cooldown is less than the cooldown time
        if (timeBeenOnCooldown < skillData.cooldown)
        {
            isAllowedToCast = false;
            // Increment timeBeenOnCooldown
            timeBeenOnCooldown += Time.deltaTime;
        }
        else
        {
            isAllowedToCast = true;
        }
    }

    /// <summary>
    /// Must be used in inherited skills Update method
    /// </summary>
    protected void SkillDeltaUpdate()   // THIS FUNCTION SHOULD BE CALLED ONCE IN EACH OF THIS CLASS' CHILDS UPDATE
    {
        UpdateCastTime();
        UpdateCooldownTime();
    }

    /// <summary>
    /// Checks if the given vector3 is within the rectangular area of a skill hit based on width and length
    /// </summary>
    /// <param name="hitCheckPosition">Given Vector3 to check if it's within bounds</param>
    /// <param name="minLength"></param>
    /// <param name="maxLength"></param>
    /// <param name="farWidth"></param>
    /// <returns>Returns TRUE if given Vector3 is within bounds and can can be damaged</returns>
    protected bool CheckLineSkillHit(Vector3 hitCheckPosition, float minLength, float maxLength, float nearWidth, float farWidth)
    {
        float angleLookAt = GetForwardAngle(casterSelf.transform);

        float halfFarWidth = farWidth * 0.5f;
        float halfNearWidth = nearWidth * 0.5f;

        Vector3 posCurrentMin, posCurrentMax, posNextMin, posNextMax;

        posCurrentMin = casterSelf.transform.position;
        posCurrentMin.x += minLength;
        posCurrentMin.z -= halfNearWidth;

        posCurrentMax = casterSelf.transform.position;
        posCurrentMax.x += maxLength;
        posCurrentMax.z -= halfFarWidth;

        posNextMin = casterSelf.transform.position;
        posNextMin.x += minLength;
        posNextMin.z += halfNearWidth;

        posNextMax = casterSelf.transform.position;
        posNextMax.z += halfFarWidth;

        posNextMax.x += maxLength;

        Vector3[]  hitCheckBounds = new Vector3[4];

        hitCheckBounds[0] = posCurrentMin;
        hitCheckBounds[1] = posCurrentMax;
        hitCheckBounds[2] = posNextMax;
        hitCheckBounds[3] = posNextMin;

        Quaternion qAngle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);

        for (int i = 0; i < hitCheckBounds.Length; i++)
        {
            hitCheckBounds[i] -= casterSelf.transform.position;
            hitCheckBounds[i] = qAngle * hitCheckBounds[i];
            hitCheckBounds[i] += casterSelf.transform.position;
        }

        // hitCheckBounds holds the 4 coordinates of where an enemy has to be standing within to be hit
        // continue to calculate if the target location is within given rectangle
        // Area A = [ x1(y2 – y3) + x2(y3 – y1) + x3(y1-y2)]/2 + [ x1(y4 – y3) + x4(y3 – y1) + x3(y1-y4)]/2

        return CheckPointInBounds(hitCheckBounds[0], hitCheckBounds[1], hitCheckBounds[2], hitCheckBounds[3], hitCheckPosition);

        // These statements include a height check
        //if (CheckPointInBounds(hitCheckBounds[0], hitCheckBounds[1], hitCheckBounds[2], hitCheckBounds[3], hitCheckPosition))
        //{
        //    if (casterSelf.transform.position.y + (skillData.verticalRange*0.5f) >= hitCheckPosition.y)
        //    {
        //        if (casterSelf.transform.position.y - (skillData.verticalRange*0.5f) <= hitCheckPosition.y)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //return false;
    }

    protected bool CheckRadialSkillHit(Vector3 hitCheckPosition)
    {
        // Based on zoneStart (where the radial skill area is starting from with a given rotation) and the position we are checking
        // Returns whether hitCheckPosition is within the arc area of the radial skill
        // Note: zoneStart is equivelent to the zoneStart parameter used for the radial skill indicator
        //float forwardAngle = 90 - Mathf.Rad2Deg * Mathf.Atan2(zoneStart.forward.z, zoneStart.forward.x);
        //Debug.Log("ForwardAngle = " + forwardAngle);
        float positionAngle = Vector3.Angle(hitCheckPosition - casterSelf.transform.position, casterSelf.transform.forward);
        float distance = Vector3.Distance(hitCheckPosition, casterSelf.transform.position);
        //Debug.Log("Target distance: " + distance);
        //Debug.Log("PositionAngle = " + positionAngle);
        if (positionAngle <= skillData.angle)
        {
            if (distance <= skillData.maxRange)
            {
                //Debug.Log("Radial Skill Hit!");
                return true;
            }
            else
            {
                //Debug.Log("Radial Skill out of range from target");
                return false;
            }
        }
        else
        {
            //Debug.Log("Radial Skill target not within specified angle");
            return false;
        }
    }

    float GetForwardAngle(Transform t)
    {
        return 90 - Mathf.Rad2Deg * Mathf.Atan2(t.forward.z, t.forward.x);
    }

    float TriArea(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        return Mathf.Abs((v1.x * (v2.z - v3.z) + v2.x * (v3.z - v1.z) + v3.x * (v1.z - v2.z)) / 2.0f);
    }

    bool CheckPointInBounds(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 point)
    {
        // Area of full rectangle
        float area = TriArea(v1, v2, v3) + TriArea(v1, v4, v3);

        // Calculate triangle areas with point and bound corners
        float pointA1 = TriArea(point, v1, v2);
        float pointA2 = TriArea(point, v2, v3);
        float pointA3 = TriArea(point, v3, v4);
        float PointA4 = TriArea(point, v1, v4);

        return (area == pointA1 + pointA2 + pointA3 + PointA4);
    }

    bool CheckInRange(Vector3 castPosition, Vector3 targetPosition)
    {
        // If the targets position is within the range of the skill,
        // Return true
        if (Vector3.Distance(castPosition, targetPosition) <= skillData.maxRange)
        {
            return true;
        }
        // Targets position from caster position is out of skill range
        return false;
    }

    protected void SelectTargetRay(ref Entity entityToSet, bool checkInRange = false)
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
                        if (CheckInRange(casterSelf.transform.position, hit.point))
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
    protected bool SelectTargetRay(ref Vector3 pointToSet, bool checkInRange = false)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                if (checkInRange)
                {
                    if (CheckInRange(casterSelf.transform.position, hit.point))
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

    protected virtual void CastSkill() { }
    protected virtual void CastSkill(List<Entity> entityList) { }

    protected virtual void ActivateSkill() { }
    protected virtual void ActivateSkill(List<Entity> entityList) { }

    protected virtual void TargetSkill() { }
    protected virtual void TargetSkill(Entity entity) { }
    protected virtual void TargetSkill(List<Entity> entityList) { }

    public virtual void TriggerSkill() { }
    public virtual void TriggerSkill(Entity entity) { }
    public virtual void TriggerSkill(List<Entity> entityList) { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float farWidth = skillData.farWidth;
        float nearWidth = skillData.nearWidth;
        float maxLength = skillData.maxRange;
        float minLength = skillData.minRange;

        float angleLookAt = GetForwardAngle(casterSelf.transform);

        float halfFarWidth = farWidth * 0.5f;
        float halfNearWidth = nearWidth * 0.5f;

        Vector3 posCurrentMin, posCurrentMax, posNextMin, posNextMax;

        posCurrentMin = casterSelf.transform.position;
        posCurrentMin.x += minLength;
        posCurrentMin.z -= halfNearWidth;

        posCurrentMax = casterSelf.transform.position;
        posCurrentMax.z -= halfFarWidth;

        posCurrentMax.x += maxLength;

        posNextMin = casterSelf.transform.position;
        posNextMin.x += minLength;
        posNextMin.z += halfNearWidth;

        posNextMax = casterSelf.transform.position;
        posNextMax.z += halfFarWidth;

        posNextMax.x += maxLength;

        Vector3[] hitCheckBounds = new Vector3[4];

        hitCheckBounds[0] = posCurrentMin;
        hitCheckBounds[1] = posCurrentMax;
        hitCheckBounds[2] = posNextMax;
        hitCheckBounds[3] = posNextMin;

        Quaternion qAngle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);

        for (int i = 0; i < hitCheckBounds.Length; i++)
        {
            hitCheckBounds[i] -= casterSelf.transform.position;
            hitCheckBounds[i] = qAngle * hitCheckBounds[i];
            hitCheckBounds[i] += casterSelf.transform.position;
        }

        Gizmos.DrawLine(hitCheckBounds[0], hitCheckBounds[1]);
        Gizmos.DrawLine(hitCheckBounds[1], hitCheckBounds[2]);
        Gizmos.DrawLine(hitCheckBounds[2], hitCheckBounds[3]);
        Gizmos.DrawLine(hitCheckBounds[3], hitCheckBounds[0]);
    }
}
