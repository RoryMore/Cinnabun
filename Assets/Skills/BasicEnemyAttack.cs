using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAttack : BaseSkill
{
    Entity target;

    // Start is called before the first frame update
    void Start()
    {
        //Call init in start
        Initialise();
    }

    protected override void Initialise()
    {
        //Init to make sure its clean
        base.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        //Every skill uses this in their update, zero exceptions
        SkillDeltaUpdate();
    }

    public override void TriggerSkill(Entity entity)
    {
        // This needs no alteration regardless of skill
        // However, if it requires the overloaded version with an entity list, activate skill in DOAFFECT 
        // needs the overloaded version with an entity list as well  

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
                    
                    TargetSkill(entity);
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

    protected override void TargetSkill(Entity entity)
    {
        // Because enemies handle their own targeting and such, this is just for enabling the initial projector
        base.TargetSkill();
        target = entity;
        EnableProjector();
        skillState = SkillState.CASTING;
    }

    protected override void CastSkill()
    {
        // For the wind up, any specific functionality needed during casting needs to be done here 
        // Other than that, below is all thats required
        //base.CastSkill();
        currentlyCasting = true;
        if (timeSpentOnWindUp >= skillData.windUp)
        {
            skillState = SkillState.DOAFFECT;
            currentlyCasting = false;
            //ActivateSkill();

            DisableProjector();
        }
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        base.ActivateSkill();
        timeBeenOnCooldown = 0.0f;
        timeSpentOnWindUp = 0.0f;
        skillState = SkillState.INACTIVE;

        // Intended effect here. Be it damage or otherwise
        // This includes checking if target is in range and such
        foreach (Entity testedEntity in entityList)
        {
            if (CheckLineSkillHit(testedEntity.transform.position, skillData.minRange, skillData.maxRange, skillData.nearWidth, skillData.farWidth))
            {
                testedEntity.TakeDamage(skillData.baseMagnitude);
            }

        }

    }
}
