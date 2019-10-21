using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    public float meleeAttackRange;
    

    Transform target;

    //bool isAttacking = false;

    

    

    private void Start()
    {
        InitialiseAll();


        //THIS ONLY WORKS WITH CURRENT HIARCHY OF ENCOUNTER, SPAWN POINT AND THEN THE ENEMY!
        myEncounter = transform.parent.parent.GetComponent<Encounter>();


        nav = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {

        target = GameObject.Find("Player").transform;

        skillList[0].Initialise();

        //foreach (SkillData skill in skillList)
        //{
        //    if (skill.cooldown != 0)
        //    {
        //        //This skill is not ready to use!
        //    }
        //    else
        //    {

        //    }
        //}



        
        

    }


    void Update()
    {
        if (!isDead)
        {
            Movement();
            UpdateAllConditions();



            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                skillList[0].currentlyCasting = true;
                //simpleBasicAttack.currentlyCasting = true;

            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                TakeDamage(10000);
            }


            if (skillList[0].currentlyCasting == true)
            {
                skillList[0].TargetSkill(transform);
                //skillList[0].CastSkill(transform);
            }
        }
        

    }


    public void Movement()
    {
        if (Vector3.Distance(myEncounter.gameObject.transform.position, target.transform.position) > myEncounter.enemyManager.maxEncounterDistance * 0.5)
        {
            //Return home
            nav.SetDestination(myEncounter.gameObject.transform.position);
        }
        else
        {
            nav.SetDestination(target.transform.position);
        }
        
    }

    

  



}

