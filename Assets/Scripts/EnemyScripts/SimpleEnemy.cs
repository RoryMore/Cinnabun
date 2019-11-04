using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    
   
    Transform target;
    Entity player; //All intents and purposes, same as 
    SkillData chosenSkill; //For deciding AI behavior
    bool isAttacking = false;





    private void Start()
    {
        InitialiseAll();


        //THIS ONLY WORKS WITH CURRENT HIARCHY OF ENCOUNTER, SPAWN POINT AND THEN THE ENEMY!
        myEncounter = transform.parent.parent.GetComponent<Encounter>();


        nav = GetComponent<NavMeshAgent>();


        //Initialise junk skill to be replaced by choose function
        chosenSkill = new SkillData();
        chosenSkill.baseDamage = 0;
        chosenSkill.range = 0;

        chosenSkill.currentlyCasting = false;

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
            Turn();
            UpdateAllSkillCooldowns();
            UpdateAllConditions();

            //Choose what attack it want's to make this turn

            Decide();

            if (chosenSkill.currentlyCasting == false)
            {
                Movement(chosenSkill);
            }

            Attack(chosenSkill);
            
            










        }
        

    }


    public void Movement(SkillData chosenSkill)
    {
        if (Vector3.Distance(myEncounter.gameObject.transform.position, target.transform.position) > myEncounter.enemyManager.maxEncounterDistance * 0.5)
        {
            //Return home
            nav.SetDestination(myEncounter.gameObject.transform.position);
        }

        if (myEncounter.initiativeList.Count != 0)
        {
            foreach (Entity entity in myEncounter.initiativeList)
            {
                if (entity != this)
                {
                    if (chosenSkill.CheckInRange(transform.position, entity.transform.position))
                    {
                        //If all prior conditions have been met, they are within potential range of the 

                        
                        
                        
                        
                    }
                }
            }
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


    public void Decide()
    {
        if(enemyCooldown <= 0)
        {
            chosenSkill.baseDamage = 0;
            chosenSkill.range = 0;

            //Choose how each enemy decides to take its actions
            //Later I would also want the skill cooldowns to come into effect

            //Step 1: If the turn is ready, begin the cycle

            //For each skill...
            foreach (SkillData checkedSkill in skillList)
            {
                //Check if the cooldown is complete...
                if (checkedSkill.timeBeenOnCooldown >= checkedSkill.cooldown)
                {
                    //Check if we are in range...
                    if (checkedSkill.CheckInRange(transform.position, target.position))
                    {
                        //Check if damage of prior skill is greater than base damange
                        if (chosenSkill.baseDamage < checkedSkill.baseDamage)
                        {
                            chosenSkill = checkedSkill;

                            //Reset the enemy turn
                            enemyCooldown = 6;
                            chosenSkill.currentlyCasting = true;
                            anim.SetTrigger("attacking");

                        }
                    }
                }
            }
        }
        
    }

    

    public void Attack(SkillData attack)
    {
        
        if (attack.currentlyCasting == true)
        {
            attack.TargetSkill(transform, myEncounter.playerInclusiveInitiativeList);
            
        }
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

