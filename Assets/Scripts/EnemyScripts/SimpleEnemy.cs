using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    public float meleeAttackRange;
    

    Transform target;
    public GameObject[] entity;

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



            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                skillList[0].currentlyCasting = true;
                //simpleBasicAttack.currentlyCasting = true;

            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
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
        nav.SetDestination(target.transform.position);
    }

    

  



}

