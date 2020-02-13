using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    [SerializeField]
    protected Projector projector;
    Material material;

    public SkillData skillData;

    [HideInInspector]
    public float timeBeenOnCooldown = 10.0f;
    [HideInInspector]
    public float timeSpentOnWindUp = 0;
    [HideInInspector]
    public bool currentlyCasting = false;

    [Tooltip("SET CASTER SELF TO PARENT OBJECT. \nE.G: Player object is set to this on the players skills")]
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
        material = new Material(Shader.Find("Projector/Tattoo"));

        projector.material = material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateCastTime()
    {
        // If we are currently casting
        if (skillData.currentlyCasting)
        {
            // Increment the delta value for time spent casting ability
            timeSpentOnWindUp += Time.deltaTime;

            projector.material.SetFloat("_Progress", (skillData.timeSpentOnWindUp / skillData.windUp));
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
        float effectiveLength = maxLength - minLength;

        Vector3 posCurrentMin, posCurrentMax, posNextMin, posNextMax;

        posCurrentMin = casterSelf.transform.position;
        posCurrentMin.z -= halfNearWidth;

        posCurrentMax = casterSelf.transform.position;
        posCurrentMax.z -= halfFarWidth;

        posCurrentMax.x += effectiveLength;

        posNextMin = casterSelf.transform.position;
        posNextMin.z += halfNearWidth;

        posNextMax = casterSelf.transform.position;
        posNextMax.z += halfFarWidth;

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

    bool CheckInRange(Vector3 castPosition, Vector3 targetPosition)
    {
        // If the targets position is within the range of the skill,
        // Return true
        if (Vector3.Distance(castPosition, targetPosition) <= skillData.range)
        {
            return true;
        }
        // Targets position from caster position is out of skill range
        return false;
    }

    protected void SelectTargetRay(Transform zoneStart, ref Entity entityToSet, bool checkInRange = false)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        float farWidth = 4.0f;
        float nearWidth = 2.0f;
        float maxLength = 6.0f;
        float minLength = 0.0f;

        float angleLookAt = GetForwardAngle(casterSelf.transform);

        float halfFarWidth = farWidth * 0.5f;
        float halfNearWidth = nearWidth * 0.5f;
        float effectiveLength = maxLength - minLength;

        Vector3 posCurrentMin, posCurrentMax, posNextMin, posNextMax;

        posCurrentMin = casterSelf.transform.position;
        posCurrentMin.z -= halfNearWidth;

        posCurrentMax = casterSelf.transform.position;
        posCurrentMax.z -= halfFarWidth;

        posCurrentMax.x += effectiveLength;

        posNextMin = casterSelf.transform.position;
        posNextMin.z += halfNearWidth;

        posNextMax = casterSelf.transform.position;
        posNextMax.z += halfFarWidth;

        posNextMax.x += effectiveLength;

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
