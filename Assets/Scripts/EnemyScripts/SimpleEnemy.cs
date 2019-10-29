using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    
   
    Transform target;
    Entity player; //All intents and purposes, same as 

    //bool isAttacking = false;

    

    

    private void Start()
    {
        InitialiseAll();


        //THIS ONLY WORKS WITH CURRENT HIARCHY OF ENCOUNTER, SPAWN POINT AND THEN THE ENEMY!
        myEncounter = transform.parent.parent.GetComponent<Encounter>();


        nav = GetComponent<NavMeshAgent>();

        //Personal Variables
        enemyCooldown = 6.0f;
        initiativeSpeed = 1.5f;


    }

    void Awake()
    {

        target = GameObject.Find("Player").transform;
        player = GameObject.Find("Player").GetComponent<Entity>();

        foreach (SkillData checkedSkill in skillList)
        {
            checkedSkill.Initialise();
        }
        

    }


    void Update()
    {
        
        if (!isDead)
        {
            Movement();
            Turn();
            
            UpdateAllConditions();



            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                skillList[0].currentlyCasting = true;
                anim.SetTrigger("attacking");
                //simpleBasicAttack.currentlyCasting = true;

            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                TakeDamage(10000000);
            }


            if (skillList[0].currentlyCasting == true)
            {
                skillList[0].TargetSkill(transform, myEncounter.playerInclusiveInitiativeList);
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
        else if (!isDead)
        {
            nav.SetDestination(target.transform.position);

            //Later this should set to the range of the technique it chooses! For now, It is not important

            if (Vector3.Distance(transform.position, player.gameObject.transform.position) < skillList[0].range)
            {
                nav.SetDestination(transform.position);
                
                FaceTarget(player.transform);
                anim.SetBool("isWalking", false);

            }
            else
            {
                nav.SetDestination(player.transform.position);
                anim.SetBool("isWalking", true);
                

            }
        }

        else
        {
            nav.enabled = false;
            anim.SetBool("isWalking", false);
        }

    }



    

    public void Attack()
    {

    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        anim.SetTrigger("getHit");
    }

    public override void Death()
    {
        base.Death();
        anim.SetBool("isDead", true);
    }
}

