using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    public float meleeAttackRange;
    

    Transform target;

    bool isAttacking = false;

    BasicAttack simpleBasicAttack;

    void Awake()
    {

        target = GameObject.Find("Player").transform;
        simpleBasicAttack.coneRangeIndicator.Init(simpleBasicAttack.angle);


        foreach (SkillData skill in skillList)
        {
            if (skill.cooldown != 0)
            {
                //This skill is not ready to use!
            }
            else
            {

            }
        }

        
        

    }


    void Update()
    {
        Movement();
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skillList[0].currentlyCasting = true;
            //simpleBasicAttack.currentlyCasting = true;
        }

        if (skillList[0].currentlyCasting == true)
        {
            skillList[0].CastSkill(transform);
        }

    }


    public void Movement()
    {
        nav.SetDestination(target.transform.position);

    }

    //TEMPORARY FUNCTION FOR WHEN JASMINE FINISHES HER TURN COUNTER



}

