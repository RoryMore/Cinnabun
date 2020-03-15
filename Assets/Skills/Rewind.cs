using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skills/Rewind", order = 1)]


public class Rewind : BaseSkill
{

    Entity entity = null;
    // If wanting to draw indicators here without doing it outside the skill
    // This function needs to take in a Transform, otherwise it doesn't need any parameter

    private void Start()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();
    }

    private void Update()
    {
        SkillDeltaUpdate();
    }

    public override void TriggerSkill()
    {
        base.TriggerSkill();
        switch (skillState)
        {
            case SkillState.INACTIVE:
                {
                    if (isAllowedToCast)
                    {
                        skillState = SkillState.TARGETTING;
                    }
                    break;
                }

            case SkillState.TARGETTING:
                {
                    //Debug.Log("Skill being Targetted");
                    TargetSkill();
                    break;
                }

            case SkillState.CASTING:
                {
                    //Debug.Log("Skill being cast!");
                    //UpdateCastTime();
                    CastSkill();
                    break;
                }

            case SkillState.DOAFFECT:
                {
                    //Debug.Log("Skill Effect Activated");
                    ActivateSkill();
                    break;
                }
        }
    }

    protected override void TargetSkill()
    {
        if (entity == null)
        {
            EnableProjector();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400, groundMask))
            {
                Vector3 lookAt = new Vector3(hit.point.x, casterSelf.transform.position.y, hit.point.z);
                casterSelf.transform.LookAt(lookAt);
            }

            //DrawRangeIndicator(zoneStart, shape);
            SelectTargetRay(ref entity);

        }
        else
        {
            skillState = SkillState.CASTING;
            //CastSkill();
        }
    }

    protected override void CastSkill()
    {
        currentlyCasting = true;
        //DrawRangeIndicator(zoneStart, shape);

        //float drawPercent = (timeSpentOnWindUp / skillData.windUp);
        //rangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, maxRange, drawPercent);

        // Increment the time spent winding up the skill
        //timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= skillData.windUp)
        {
            //ActivateSkill();
            skillState = SkillState.DOAFFECT;
            
            currentlyCasting = false;
            DisableProjector();
        }
    }

    protected override void ActivateSkill()
    {
        timeBeenOnCooldown = 0.0f;
        entity.RewindBack();
        timeSpentOnWindUp = 0.0f;
        entity = null;
    }
}