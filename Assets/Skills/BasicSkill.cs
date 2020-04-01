﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : BaseSkill
{
    public enum WhenApplyed
    {
        Start,
        Hit,
        End,
    }
    
    //public Effects[] StatusEffects;
   
    public Entity.BUffEFffect buf;
    [Tooltip("only put in here Entity.EntityType of the enemy which the skill want to hit")]
    public List<string> TargetEntity;
    //private Entity.Condition[] Conditions;
    // Start is called before the first frame update
    //void Start()
    //{
    //    //Call init in start
    //    Initialise();
    //}

    void Start()
    {
        base.Initialise();
    }
    protected virtual void Initialise(Entity _Entity)
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

    public override void TriggerSkill(List<Entity> entityList)
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
                    Debug.Log("effect");
                    ActivateSkill(entityList);

                    break;
                }
        }
    }

    protected override void TargetSkill()
    {
        // Because enemies handle their own targeting and such, this is just for enabling the initial projector
        base.TargetSkill();
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

           
          
            if (GetComponentInParent<StatusEfect>() != null)
            {
                GetComponentInParent<StatusEfect>().applyEffects(base.casterSelf, Effects.EffectApplyType.StartSkill);
            }

            if (GetComponentInParent<BuffEffect>() != null)
            {
                GetComponentInParent<BuffEffect>().applyEffects(base.casterSelf, Buff.EffectApplyType.StartSkill);
            }//ActivateSkill();

            DisableProjector();
        }
        ApplyCastSkillProplys();
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        Debug.Log("dummyattack");
        base.ActivateSkill();


        // Intended effect here. Be it damage or otherwise
        // This includes checking if target is in range and such

        //apply buff
        buf = casterSelf.ApplyBuffOff();


        foreach (Entity testedEntity in entityList)
        {
            //mutlplay checha to see if this is one of the skills target
            if (testedEntity != casterSelf)
            {
                for (int i = 0; i < TargetEntity.Count; i++)
                {
                    
                    if (TargetEntity[i] == "Self")
                    {

                    }
                    else if (TargetEntity[i] == testedEntity.gameObject.tag)//testedEntity.EntityTag.ToString()
                    {
                        //do skill effects
                        if (CheckLineSkillHit(testedEntity.transform.position, skillData.minRange, skillData.maxRange, skillData.nearWidth, skillData.farWidth))
                        {
                            if (skillData.skill != SkillData.SkillList.HEAL)
                            {
                                testedEntity.TakeDamage(skillData.baseMagnitude + (int)buf.damage, skillData.damageType, casterSelf.CalculateCriticalStrike());
                            }
                            else
                            {
                                if (skillData.baseMagnitude != 0)
                                {
                                    testedEntity.TakeHealth(skillData.baseMagnitude);
                                }
                               
                            }

                            // testedEntity.TakeDamage((int)(skillData.baseMagnitude * DamageMult));

                            if (GetComponentInParent<StatusEfect>())
                            {
                                GetComponentInParent<StatusEfect>().applyEffects(testedEntity, Effects.EffectApplyType.OnHit);
                            }

                            if (GetComponentInParent<BuffEffect>())
                            {
                                GetComponentInParent<BuffEffect>().applyEffects(testedEntity, Buff.EffectApplyType.OnHit);
                            }
                        }
                    }
                }
            }
        }

        if (GetComponentInParent<StatusEfect>())
        {
            GetComponentInParent<StatusEfect>().applyEffects(base.casterSelf, Effects.EffectApplyType.EndSkill);
        }

        if (GetComponentInParent<BuffEffect>())
        {
            GetComponentInParent<BuffEffect>().applyEffects(base.casterSelf, Buff.EffectApplyType.EndSkill);
        }


        ApplySkillProplys();



    }
    protected virtual void ApplySkillProplys()
    {
        timeBeenOnCooldown = 0.0f + buf.cooldown;
        timeSpentOnWindUp = 0.0f;
        currentlyCasting = false;
        skillState = SkillState.INACTIVE;
    }

    protected virtual void ApplyCastSkillProplys()
    {

    }
    public Entity GetEnitiy() {
        return base.casterSelf;
    }
}