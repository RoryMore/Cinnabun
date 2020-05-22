using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skills/Teleport", order = 1)]

public class Teleport : BaseSkill
{
    Entity entityTarget1 = null;
    Vector3 teleportLocation;
    bool destination1Set = false;

    [Header("Damage")]
    [SerializeField]
    float damageMultiplier = 0.1f;
    
    private void Start()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();

        if (SaveManager.GetUpgradeList().teleportRange != null)
        {
            skillData.maxRange += SaveManager.GetUpgradeList().teleportRange.GetUpgradedMagnitude();
        }
    }

    /// <summary>
    /// Used when the player wishes to unSelect the skill
    /// </summary>
    public override void ResetSkillVars()
    {
        base.ResetSkillVars();
        destination1Set = false;
        entityTarget1 = null;
        teleportLocation.Set(0, 0, 0);
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
        //If no data has yet been specified
        if (entityTarget1 == null)
        {
            //ResetIndicatorImages();
            EnableProjector();

            //SelectTargetRay(ref entityTarget1, true);
            entityTarget1 = casterSelf;

        }
        //If we have our target but no destination
        else if (entityTarget1 != null && !destination1Set)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400, groundMask))
            {
                Vector3 lookat = new Vector3(hit.point.x, casterSelf.transform.position.y, hit.point.z);
                casterSelf.transform.LookAt(lookat);
            }

            //DrawRangeIndicator(zoneStart, shape);
            destination1Set = SelectTargetRay(ref teleportLocation, groundMask, true);


        }
        //If we have both, proceed
        else if(entityTarget1 != null && destination1Set)
        {
            skillState = SkillState.CASTING;
            //CastSkill();
        }
    }

    protected override void CastSkill()
    {
        //SetFillType(fillType);

        currentlyCasting = true;
        //DrawRangeIndicator(zoneStart, shape);

        //float drawPercent = (timeSpentOnWindUp / skillData.windUp);
        //rangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, maxRange, drawPercent);

        //timeSpentOnWindUp += Time.deltaTime;

        if (timeSpentOnWindUp >= GetCalculatedWindUp())
        {
            skillState = SkillState.DOAFFECT;
            currentlyCasting = false;
            //ActivateSkill();
            
            DisableProjector();
        }
    }


    protected override void ActivateSkill()
    {
        timeBeenOnCooldown = 0.0f;
        // What happens when the skill is activated

        if (entityTarget1.nav != null)
        {
            entityTarget1.nav.Warp(teleportLocation);
        }
        //entityTarget1.transform.position = teleportLocation;
        
        entityTarget1.TakeDamage(Mathf.RoundToInt((skillData.baseMagnitude + casterSelf.GetIntellectDamageBonus()) * damageMultiplier), skillData.damageType, casterSelf.CalculateCriticalStrike());

        entityTarget1 = null;
        teleportLocation.Set(0, 0, 0);
        destination1Set = false;

        timeSpentOnWindUp = 0.0f;
        skillState = SkillState.INACTIVE;

        //Target position is remade every time the skill is activated so no need to reset/null
        //Debug.Log("Activated!");

    }
}


