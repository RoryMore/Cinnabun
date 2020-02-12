﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    public SkillData skillData;

    [HideInInspector]
    public float timeBeenOnCooldown = 10.0f;
    [HideInInspector]
    public float timeSpentOnWindUp = 0;
    [HideInInspector]
    public bool currentlyCasting = false;

    [Header("SET CASTER SELF TO PARENT OBJECT. E.G: Player object is set to this on the players skills")]
    public Entity casterSelf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void Initialise()
    {
        if (skillData != null)
        {
            timeBeenOnCooldown = skillData.cooldown;
            timeSpentOnWindUp = 0.0f;
            currentlyCasting = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateCastTime()
    {
        // If we are currently casting
        if (currentlyCasting)
        {
            // Increment the delta value for time spent casting ability
            timeSpentOnWindUp += Time.deltaTime;
        }
    }

    void UpdateCooldownTime()
    {
        // If the time this skill has been on cooldown is less than the cooldown time
        if (timeBeenOnCooldown < skillData.cooldown)
        {
            // Increment timeBeenOnCooldown
            timeBeenOnCooldown += Time.deltaTime;
        }
    }

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
    /// <param name="width"></param>
    /// <returns>Returns TRUE if given Vector3 is within bounds and can can be damaged</returns>
    protected bool CheckLineSkillHit(Vector3 hitCheckPosition, float minLength, float maxLength, float width)
    {
        float angleLookAt = GetForwardAngle(casterSelf.transform);

        float halfWidth = width * 0.5f;
        float effectiveLength = maxLength - minLength;

        Vector3 posCurrentMin, posCurrentMax, posNextMin, posNextMax;

        posCurrentMin = casterSelf.transform.position;
        posCurrentMin.z -= halfWidth;

        posCurrentMax = casterSelf.transform.position;
        posCurrentMax.z -= halfWidth;

        posCurrentMax.x += effectiveLength;

        posNextMin = casterSelf.transform.position;
        posNextMin.z += halfWidth;

        posNextMax = casterSelf.transform.position;
        posNextMax.z += halfWidth;

        posNextMax.x += effectiveLength;

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

    protected bool CheckRadialSkillHit(Vector3 hitCheckPosition, Transform zoneStart)
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
        if (positionAngle <= skillData.angleWidth)
        {
            if (distance <= skillData.range)
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
}
