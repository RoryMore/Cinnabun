using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

   
    Transform target;
    Player player; //All intents and purposes, same as target but target is just the transform
    bool isAttacking = false;
    bool isEvading = false;

    
    bool isRanged;

    public List<EnemyScript> enemyForces;

    PauseAbility pauseAbility;

    //VERT SLICE USE ONLY
    EnemyAttack attack; 
    ConeRangeIndicator coneRangeIndicator;
    RectangleRangeIndicator rectangleRangeIndicator;
    //END OF VERT ONLY CONTENT

    public BaseSkill basicAttack;

    private void Start()
    {
        InitialiseAll();


        //THIS ONLY WORKS WITH CURRENT HIARCHY OF ENCOUNTER, SPAWN POINT AND THEN THE ENEMY!
        myEncounter = transform.parent.parent.GetComponent<Encounter>();
        


        nav = GetComponent<NavMeshAgent>();


        //Personal Variables
        enemyCooldown = Random.Range(5.5f, 6.5f);//6.0f;
        initiativeSpeed = 1.5f;

        currentHP = maxHP / 2;


        //OLD AWAKE
        target = GameObject.Find("Player").transform;

        player = target.GetComponent<Player>();

        pauseAbility = GameObject.Find("PauseMenuUI").GetComponent<PauseAbility>();

       // basicAttack = GetComponentInChildren<BaseSkill>();



    }

    void Awake()
    {

        

    }



    void Update()
    {
        //If we arent Dead...
        if (!isDead)
        {
            
            // Update turn cooldown
            Turn();

            //Update all cooldowns and conditons
            
            UpdateAllConditions();

            //Choose what attack it want's to make this turn
            //Decide();

            //bool isInRangeOfFriendlyAttack = CheckAttackers();

            //So long as we aren't attacking, move!
            
            if (!basicAttack.currentlyCasting)
            {
                nav.enabled = true;
                Movement();
            }
            else
            {
                nav.enabled = false;
            }

            Attack(basicAttack);
            
        }
        

    }

    public Vector3 ChooseDestination(SkillData skill)
    {
        //Pick a random point near the player well within range
        float x = Random.Range(player.transform.position.x - (skill.maxRange * 0.5f), player.transform.position.x + (skill.maxRange * 0.5f));
        float z = Random.Range(player.transform.position.z - (skill.maxRange * 0.5f), player.transform.position.z + (skill.maxRange * 0.5f));

        


        //Otherwise all good, move on!
        return destination = new Vector3(x, 0, z);

    }


    public void Movement()
    {
        // REQUIREMENTS 
        // When the enemy is not attacking, approach the player until they are within range of their attack
        // If the enemy health is low

        // If dead, stop moving towards the player

        if (!isDead)
        {
            //Fetch distance betweeen self and the player
            float distance = Vector3.Distance(this.gameObject.transform.position, target.transform.position);

            if (!basicAttack.currentlyCasting)
            {

                //If the player is using an attack and we're close to death, retreat!
                if (player.playerState == Player.PlayerState.DOINGSKILL && currentHP <= maxHP * 0.25f)
                {
                    nav.enabled = true;
                    nav.SetDestination(Vector3.MoveTowards(transform.position, player.transform.position, -nav.speed));
                }

                //If we are well within attack range, stop moving towards player
                else if (distance < basicAttack.skillData.maxRange * 0.5f)
                {
                    anim.SetBool("isWalking", false);
                    nav.enabled = false;
                }
                //If we are outside of range, walk towards player
                else
                {
                    anim.SetBool("isWalking", true);
                    nav.enabled = true;
                    nav.SetDestination(Vector3.MoveTowards(transform.position, player.transform.position, nav.speed));

                }
                

            }
        }

    }


    //public void Decide()
    //{
    //    if(enemyCooldown <= 0)
    //    {

    //        //Choose how each enemy decides to take its actions
    //        //Later I would also want the skill cooldowns to come into effect

    //        //Step 1: If the turn is ready, begin the cycle

    //        //For each skill...
    //        foreach (SkillData checkedSkill in skillList)
    //        {
    //            //Check if the cooldown is complete...
    //            if (checkedSkill.timeBeenOnCooldown >= checkedSkill.cooldown)
    //            {
    //                //Check if we are in range...
    //                if (checkedSkill.CheckInRange(transform.position, target.position))
    //                {
    //                    //int choice = (int)Random.Range(0.0f, skillList.Count);
                    
    //                    //Check if damage of prior skill is greater than base damange
    //                    if (chosenSkill.baseMagnitude <= checkedSkill.baseMagnitude)
    //                    {
    //                        chosenSkill = checkedSkill;
                    
    //                        //Reset the enemy turn
    //                        enemyCooldown = 6;
    //                        chosenSkill.currentlyCasting = true;
    //                        anim.SetTrigger("attacking");
                    
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    



 

    public Entity CheckAttackers()
    {

        // Check all enemies
        foreach (Entity enemy in myEncounter.initiativeList)
        {
            //If they are attacking... 
            if (enemy.chosenSkill.currentlyCasting == true)
            {
               //If it isn't us...
                if (enemy != this)
                {
                    //If we are in range
                    if (enemy.chosenSkill.skillData.CheckInRange(enemy.transform.position, transform.position))
                    {
                        //Get out of dodge!
                        return enemy;
                    }


                }


            }


        }

        return null;

    }
    //if player is attacking. needs attack damage and until skill goes of 
    //called when entering range, and afterAttack while in range
    public void Evade(int damage,float skillCooldown)
    {
        //HP is low
        if (currentHP <= damage)  
        {
            //if we attacking
            if (isAttacking)
            {
                // findout how will finish first
                if (turnSpeed < skillCooldown)
                {
                    //if we finish first
                    return;
                }
                else
                {
                    //we need to get out a evade
                    EvadeNow();
                }
            }
            else
            {
                //we need to get out a evade
                EvadeNow();
            }
        }

        //if (isEvading)
        //{
        //    //float x = Random.Range(0 - (skill.maxRange * 0.5f), player.transform.position.x + (skill.maxRange * 0.5f));
        //    //float z = Random.Range(player.transform.position.z - (skill.maxRange * 0.5f), player.transform.position.z + (skill.maxRange * 0.5f));

        //    ////Otherwise all good, move on!
        //    //return destination = new Vector3(0, 0, 0);
        //}
        
        //if (CheckAttackers() != null)
        //{
        //    isEvading = true;
        //    nav.SetDestination(Vector3.MoveTowards(transform.position, CheckAttackers().transform.position, -nav.speed));
        //}

        //else
        //{
        //    isEvading = false;
        //}
        
    }
    //must evade Now
    void EvadeNow()
    {
        //get new postion
        isEvading = true;
    }

    //if enemy gets here 
    //if (isEvading){
    //delay 0.5f before moving
    //}
    //}

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        anim.SetTrigger("getHit");
    }

    public override void Death()
    {
        base.Death();
        anim.SetBool("isDead", true);
        anim.enabled = false;
    }


    public void Attack(BaseSkill basicAttack)
    {
        //Fetch Distance
        float distance = Vector3.Distance(transform.position, player.transform.position);

        //If we are in range and ready to attack
        if (distance <= basicAttack.skillData.maxRange)
        

        if (basicAttack.isAllowedToCast)
        {

            basicAttack.TriggerSkill(myEncounter.playerInclusiveInitiativeList);


        }
    }


    public void VertSliceAttack()
    {

            float distance = Vector3.Distance(transform.position, player.transform.position);

            //we are ready to make our attack, and we are in range. attack!
            if (distance <= attack.range && enemyCooldown <= 0.0f)
            {
                isAttacking = true;
                nav.enabled = false;
            }
            if (isAttacking == true)
            {
                anim.SetTrigger("attacking");

                 if (pauseAbility.states != PauseAbility.GameStates.TIMESTOP)
                 {
                     timeSpentDoingAction += Time.fixedDeltaTime;
                 }

                 attack.DrawCastTimeRangeIndicator(timeSpentDoingAction);//drawcasttimerangeindicator(timespentdoingaction);

                     if (timeSpentDoingAction >= attack.actionSpeed)
                     {
                         if (attack.ShouldEnemyInPositionBeDamaged(player.transform.position) == true)
                         {
                             player.TakeDamage((int)attack.magnitude);
                         }

                         //play animation
                         enemyCooldown = 6.0f;
                         timeSpentDoingAction = 0.0f;
                         anim.SetTrigger("attacking");
                         nav.enabled = true;
                         isAttacking = false;

                     }
            }
            //debug.log("attack!");
            //if its the melee enemy turn but we are out of range, we go into defence stance!
            else if (attack.range <= distance && enemyCooldown <= 0.0f)
            {
                HoldTurn();
                //debug.log("she's too far!");
            }
            else if (attack.range <= distance && 0.0f <= enemyCooldown)
            {
                //debug.log("");
            }
       

            
        
    }



}

