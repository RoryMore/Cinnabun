using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseSkill
{
    BoxCollider Box;
    [SerializeField] float Timer;
    float MaxTimer;
    [SerializeField]  bool TimerON = false;
    public float Speed;
    void Start()
    {
        //Call init in start
        Initialise();
        MaxTimer = skillData.maxRange/ (100 * Speed);
        
        Box = GetComponent<BoxCollider>();
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
        if (TimerON)
        {

            if (Timer <= 0)
            {
                Debug.Log("triger");
                Box.enabled = false;
                TimerON = false;
               
                skillState = SkillState.INACTIVE;
            }
            else
            { 
                gameObject.transform.parent.transform.Translate(transform.forward* Speed);
                Timer -= Time.deltaTime;
            }
        }
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
        timeBeenOnCooldown = 0.0f;
        timeSpentOnWindUp = 0.0f;
        currentlyCasting = false;


        // Intended effect here. Be it damage or otherwise
        // This includes checking if target is in range and such

        Box.enabled = true;
        Timer = MaxTimer;
        TimerON = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Entity>().TakeDamage(skillData.baseMagnitude);
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(skillData.baseMagnitude);
            }
        }
    }
}
