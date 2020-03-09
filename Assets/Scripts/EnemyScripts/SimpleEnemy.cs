using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SimpleEnemy : EnemyScript
{
    //System to match enemy stats to player stats in some capacity. If the player gets higher damage, increase health

    //[SerializeField] private Transform damageNumbers;
    Transform target;
    Player player; //All intents and purposes, same as target but target is just the transform

    //[HideInInspector]
    public bool isActive;

    [SerializeField]
    bool isAttacking = false;
    [SerializeField]
    bool isEvading = false;

    public bool skillIsCasting;

    List<Entity> fakeList = new List<Entity>();

    bool isRanged;

    public List<EnemyScript> enemyForces;

    TextSystem textSystem;
    PauseAbility pauseAbility;
    [SerializeField] private Transform damageNumbers;

    public BaseSkill basicAttack;
    public BaseSkill[] skillList;

    public TYPE type;
    public AGRESSION agression;


    //DecidingBools
    bool hasDecided;
    bool destinationLocked;

    public enum AGRESSION
    {
        AGRESSIVE,
        TIMID
    }

    public enum TYPE
    {
        MELEE,
        RANGED,
    }

    private void Start()
    {


    }

    void Awake()
    {

        InitialiseAll();


        //THIS ONLY WORKS WITH CURRENT HIARCHY OF ENCOUNTER, SPAWN POINT AND THEN THE ENEMY!
        myEncounter = transform.parent.parent.GetComponent<Encounter>();



        nav = GetComponent<NavMeshAgent>();


        //Personal Variables
        enemyCooldown = Random.Range(5.5f, 6.5f);//6.0f;
        initiativeSpeed = 1.5f;

        currentHP = maxHP;

        textSystem = GetComponent<TextSystem>();

        target = GameObject.Find("Player").transform;

        player = target.GetComponent<Player>();

        pauseAbility = GameObject.Find("PauseMenuUI").GetComponent<PauseAbility>();

        basicAttack = GetComponentInChildren<BaseSkill>();



        skillList = GetComponentsInChildren<BaseSkill>();

        chosenSkill = skillList[0];

        isActive = true;

    }



    void Update()
    {

        if(isActive)

        {
            //If we arent Dead...
            if (!isDead)
            {

                // Update turn cooldown
                Turn();

                //Update all cooldowns and conditons
                UpdateAllConditions();

                //Choose what attack it want's to make this turn assuming we haven't already chosen.
                if (!hasDecided)
                {
                    Decide();
                }
                //Choose where we want to go, unless we're melee because we will always want to rush the player
                if (!destinationLocked && type != TYPE.MELEE)
                {
                    ChooseDestination(chosenSkill);
                }




                Attack(chosenSkill);
                //Initiate the attack//skill at their earliest convenience

                ///Only call the attack function once because repreated calls stall the enemy
                if (isAttacking)
                {
                    //Trigger the skill!
                    chosenSkill.TriggerSkill(myEncounter.playerInclusiveInitiativeList);
                    if (!chosenSkill.currentlyCasting)
                    {
                        isAttacking = false;
                        hasDecided = false;

                    }
                }

                //at every frame where we AREN'T attacking or using a skill, do the following

                foreach (BaseSkill skill in skillList)
                {
                    if (!skill.currentlyCasting)
                    {
                        //Make sure we are able to move as we disable movement while attacking
                        nav.enabled = true;
                        //Provide evasive manouvres 
                        Evade();

                        if (type == TYPE.MELEE)
                        {
                            Movement(player.transform.position);
                        }
                        else if (type == TYPE.RANGED)
                        {
                            Movement(destination);
                        }


                    }

                    else
                    {
                        nav.enabled = false;
                    }

                }




                skillIsCasting = chosenSkill.currentlyCasting;

            }

                    else
                    {
                        nav.enabled = false;
                    }





        }



    }

    void EnemyScaling()
    {
        //player.constitution 
    }

    public Vector3 ChooseDestination(BaseSkill skill)
    {
        //Pick a random point near the player well within range
        float x = Random.Range(player.transform.position.x - (skill.skillData.maxRange * 0.5f), player.transform.position.x + (skill.skillData.maxRange * 0.5f));
        float z = Random.Range(player.transform.position.z - (skill.skillData.maxRange * 0.5f), player.transform.position.z + (skill.skillData.maxRange * 0.5f));

        


        //Otherwise all good, move on!
        return destination = new Vector3(x, player.transform.position.y, z);

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
            if (!isEvading)
            {
                if (!chosenSkill.currentlyCasting)
                {

                    //If the player is using an attack and we're close to death, retreat!
                    if (player.playerState == Player.PlayerState.DOINGSKILL && currentHP <= maxHP * 0.25f)
                    {
                        nav.enabled = true;
                        nav.SetDestination(Vector3.MoveTowards(transform.position, player.transform.position, -nav.speed));
                    }



                    //If we are well within attack range, stop moving towards player
                    else if (distance < chosenSkill.skillData.maxRange * 0.5f)
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

    }

    //Movement that uses chosen destination function
    public void Movement(Vector3 destination)
    {
        // REQUIREMENTS 
        // When the enemy is not attacking, approach the player until they are within range of their attack
        // If the enemy health is low

        // If dead, stop moving towards the player

        if (!isDead)
        {
            //Fetch distance betweeen self and the player
            float distance = Vector3.Distance(this.gameObject.transform.position, target.transform.position);
            if (!isEvading)
            {

            
                if (!chosenSkill.currentlyCasting)
                {

                    //If the player is using an attack and we're close to death, retreat!
                    if (player.playerState == Player.PlayerState.DOINGSKILL && currentHP <= maxHP * 0.25f)
                    {
                        nav.enabled = true;
                        nav.SetDestination(Vector3.MoveTowards(transform.position, destination, -nav.speed));
                    }



                    //If we are well within attack range, stop moving towards player
                    else if (distance <= chosenSkill.skillData.maxRange * 0.5f)
                    {
                        anim.SetBool("isWalking", false);
                        nav.enabled = false;
                    }
                    //If we are outside of range, walk towards player
                    else
                    {
                        anim.SetBool("isWalking", true);
                        nav.enabled = true;
                        nav.SetDestination(Vector3.MoveTowards(transform.position, destination, nav.speed));

                    }


                }

            }

            if (transform.position == destination)
            {
                destinationLocked = false;
            }
        }

    }


    public void Decide()
    {
        if(enemyCooldown <= 0)
        {

            //Choose how each enemy decides to take its actions
            //Later I would also want the skill cooldowns to come into effect

            //Step 1: If the turn is ready, begin the cycle
          
             //For each skill...
             foreach (BaseSkill checkedSkill in skillList)
             {
                 //Check if the cooldown is complete...
                 if (checkedSkill.timeBeenOnCooldown >= checkedSkill.skillData.cooldown)
                 {

                     //Check if we are in range...
                     if (checkedSkill.CheckInRange(transform.position, target.position))
                     {
                        //More performance intensive, check that the player would be hit if we used it now
                        if (checkedSkill.CheckLineSkillHit(target.position,
                                                            chosenSkill.skillData.minRange,
                                                            chosenSkill.skillData.maxRange,
                                                            chosenSkill.skillData.nearWidth,
                                                            chosenSkill.skillData.farWidth)) 

                        {
                            //Check if damage of prior skill is greater than base damange
                            if (chosenSkill.skillData.baseMagnitude <= checkedSkill.skillData.baseMagnitude)
                            {
                                isAttacking = true;
                                hasDecided = true;
                                chosenSkill = checkedSkill;

                                //Reset the enemy turn
                                enemyCooldown = 6;



                            }
                        }
                     }
                 }
             }
        }
    }

    



 

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
                  if (enemy.chosenSkill.CheckLineSkillHit(target.position,
                      chosenSkill.skillData.minRange,
                      chosenSkill.skillData.maxRange,
                      chosenSkill.skillData.nearWidth,
                      chosenSkill.skillData.farWidth))
                  {
                      //Get out of dodge!
                      return enemy;
                  }


                }


            }

      }



      return null;

    }

    public void Evade()
    {
      
      if (CheckAttackers() != null)
      {
          isEvading = true;
          anim.SetBool("isWalking", true);
          nav.SetDestination(Vector3.MoveTowards(transform.position, CheckAttackers().transform.position, -nav.speed));
      }

        else
        {
            isEvading = false;
        }
      
    }


    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        //Create(transform.position, amount, false);

        anim.SetTrigger("getHit");
    }

    public override void TakeDamage(int amount, SkillData.DamageType damageType, bool isCrit)
    {
        base.TakeDamage(amount, damageType, isCrit);
        anim.SetTrigger("getHit");
    }

    public override void Death()
    {
        base.Death();
        anim.SetBool("isDead", true);
        foreach(BaseSkill skill in skillList)
        {
            skill.DisableProjector();
        }
        anim.enabled = false;

        GetComponent<BloodOrbDropControl>().DropItem(transform.position);

        myEncounter.itemSpawner.SpawnItem(transform.position);
    }

    public void Create(Vector3 position, int damageAmount, bool crit)
    {
        float x = Random.Range(-0.9f, 0.3f);
        float y = Random.Range(-0.9f, 0.3f);
        Vector3 numberVec = new Vector3(x, y + 3, 0.0f);

        Transform damagePopUpTransfrom = Instantiate(damageNumbers, position + numberVec, Quaternion.identity );

        DamagePopUp damagePopUp = damagePopUpTransfrom.GetComponent<DamagePopUp>();
        damagePopUp.SetUp(damageAmount, crit);
    }

    public void Attack(BaseSkill chosenSkill)
    {
        //Fetch Distance
        float distance = Vector3.Distance(transform.position, player.transform.position);

        //If we are in range and ready to attack
        if (distance <= chosenSkill.skillData.maxRange * 0.5f && chosenSkill.isAllowedToCast)
        {
            FaceTarget(target);
            if(chosenSkill.CheckLineSkillHit(target.position, 
                chosenSkill.skillData.minRange,
                chosenSkill.skillData.maxRange,
                chosenSkill.skillData.nearWidth,
                chosenSkill.skillData.farWidth))
            {
                isAttacking = true;
                nav.enabled = false;
                anim.SetTrigger("attacking");
                
            }

 


            
        }
       else if (chosenSkill.skillData.maxRange <= distance && enemyCooldown <= 0.0f)
       {
           HoldTurn();
           Debug.Log("she's too far!");
           

       }



    }

    public void SwitchActiveBehavior()
    {
        nav.enabled = !nav.enabled;
        isActive = !isActive;
    }



}

