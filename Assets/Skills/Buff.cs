using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : BaseSkill
{
    [Tooltip("All targets that can be buff by this needs to be here. the higher on the list the better of a target it is")]
    SimpleEnemy.TYPE[] MainTargets;

    public enum Target
    {
        All,
        AllPutPlayer,
        Type,
        One
    }
    public enum buffeType
    {
        Percent,
        number
    }

    [System.Serializable]
    public struct BuffEffect
    {
        public Entity.ConditionEffect Effects;
        public float Duration;
        public buffeType BuffType;

        [Tooltip("is buff type is percent strenth is the percent amount, if number then strength is a falt")]
        public float Strength;

        public Entity.ConditionEffect GetEffect() { return Effects; }
        public buffeType GetBuffType() { return BuffType; }
        public float GetDuration() { return Duration; }
        public float GetStrength() { return Strength; }

    }

    public Target CurrentTarget;

    [Header("buff")]
    [SerializeField] public BuffEffect[] BuffStats;

    //buff efffects
    // Start is called before the first frame update
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

    public override void TriggerSkill(List<Entity> entityList)
    {
        // This needs no alteration regardless of skill
        // However, if it requires the overloaded version with an entity list, activate skill in DOAFFECT 
        // needs the overloaded version with an entity list as well  
        Debug.Log("triggrer");
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

            //ActivateSkill();

            DisableProjector();
        }
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        base.ActivateSkill();


        // Intended effect here. Be it damage or otherwise
        // This includes checking if target is in range and such

        bool trigger = false;
        //
        switch (CurrentTarget)
        {
            case Target.All:
                foreach (Entity entity in entityList)
                {
                    //check if it is alive
                    if (!entity.isDead)
                    {
                        effectApllyed(entity);
                        trigger = true;
                    }
                }
                break;
            case Target.AllPutPlayer:
                foreach (Entity entity in entityList)
                {
                    if (entity.gameObject.name != "Player")
                    {
                        if (!entity.isDead)
                        {
                            effectApllyed(entity);
                            trigger = true;
                        }
                    }
                }

                break;
            case Target.Type:
                foreach (Entity entity in entityList)
                {
                    if (entity.gameObject.name != "Player")
                    {
                        //if (entity.gameObject.SimpleEnemy.TYPE== CurrentTarget)
                        {
                            if (!entity.isDead)
                            {
                                effectApllyed(entity);
                                trigger = true;
                            }
                        }
                    }
                }
                break;
            case Target.One:
                foreach (Entity entity in entityList)
                {
                    if (entity.gameObject.name != "Player")
                    {
                        if (!entity.isDead)
                        {
                            effectApllyed(entity);
                            trigger = true;
                            break;
                        }
                        
                    }
                }

                /*Entity entityCurrnet = null;
                //foreach (Entity entity in entityList)
                //{
                //    entity.gameObject.GetComponent<SimpleEnemy>().TYPE = SimpleEnemy.TYPE;
                //    if (entity.gameObject.name != "Player")
                //    {
                //        if (entityCurrnet == null)
                //        {
                //            entityCurrnet = entity;
                //            CurrentType = entity.gameObject.SimpleEnemy.TYPE;
                //        }
                //        if (entity.gameObject.SimpleEnemy.TYPE > CurrentType)
                //        {
                //            entityCurrnet = entity;
                //            CurrentType = entity.gameObject.SimpleEnemy.TYPE;
                //        }
                //    }
                //}
                //if (entityCurrnet == null)
                //{
                //    effectApllyed(entityCurrnet);
                //}
                */
                break;
            default:
                break;
        }


        if (trigger)
        {
            timeBeenOnCooldown = 0.0f;
            timeSpentOnWindUp = 0.0f;
        }

        skillState = SkillState.INACTIVE;
        currentlyCasting = false;


    }

    void effectApllyed(Entity entity)
    {
        Entity.Condition Con;

        for (int i = 0; i < BuffStats.Length; i++)
        {


            if (BuffStats[i].GetBuffType() == buffeType.number)
            {
                Con = new Entity.Condition(
                     BuffStats[i].GetDuration(),
                     BuffStats[i].GetEffect(),
                     BuffStats[i].GetStrength());

            }
            else
            {
                Con = new Entity.Condition(
                     BuffStats[i].GetDuration(),
                     BuffStats[i].GetEffect(),
                     BuffStats[i].GetStrength());
            }


            entity.currentConditions.Add(Con);

        }
    }
}
