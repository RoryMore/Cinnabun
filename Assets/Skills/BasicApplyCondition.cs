using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BasicApplyCondition", order = 1)]
public class BasicApplyCondition : SkillData
{
    [Header("Damage Over Time Setting")]
    public float damageTickRate;
    [Tooltip("How long the condition will last in seconds")]
    public float duration;
    public Entity.ConditionType damageOverTimeCondition;
    public float effectivePercent;

    [Header("Is Enemy Casting")]
    public bool enemyCaster;

    Entity target = null;
    bool isTargetSet = false;

    public override void TargetSkill(Transform zoneStart)
    {
        // Enemy is using this skill
        if (enemyCaster && target == null)
        {
            if (!isTargetSet)
            {
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
                isTargetSet = true;
            }

            // We don't want the enemy models to tilt backwards or forwards weirdly - no head swiveling
            // So use a point with our own y value
            Vector3 lookAtPoint = new Vector3(target.transform.position.x, zoneStart.position.y, target.transform.position.z);
            zoneStart.LookAt(lookAtPoint);
        }
        // Player is using this skill
        else if (!enemyCaster && target == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400))
            {
                Vector3 lookat = new Vector3(hit.point.x, zoneStart.position.y, hit.point.z);
                zoneStart.LookAt(lookat);
            }

            DrawRangeIndicator(zoneStart, shape);
            SelectTargetRay(zoneStart, ref target, true);
        }

        if (target != null)
        {
            CastSkill(zoneStart);
        }
    }

    protected override void CastSkill(Transform zoneStart)
    {
        currentlyCasting = true;
        DrawRangeIndicator(zoneStart, shape);

        float drawPercent = (timeSpentOnWindUp / windUp);
        rangeIndicator.DrawCastTimeIndicator(zoneStart, angleWidth, 0.0f, range, drawPercent);

        timeSpentOnWindUp += Time.deltaTime;

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

        if (target != null)
        {
            Entity.Condition dotCon = new Entity.Condition(duration, damageOverTimeCondition, effectivePercent, baseDamage, damageTickRate);

            target.currentConditions.Add(dotCon);
        }

        target = null;
        isTargetSet = false;
    }
}
