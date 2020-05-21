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


    List<Entity> fakeList = new List<Entity>();

    bool isRanged;

    public List<EnemyScript> enemyForces;


    TextSystem textSystem;
    PauseAbility pauseAbility;
    [SerializeField] private Transform damageNumbers;

    //public BaseSkill basicAttack;
    public BaseSkill[] skillList;

    public TYPE type;
    public AGRESSION agression;

    // public BaseSkill viewCurrentSkill;
    public bool CurrentlyCasting = false;// true is casting skill
    Vector3 offset;
    //DecidingBools
    bool hasDecided;
    bool destinationLocked;
    bool MovementEnable = true;
    bool canMove = true;

    bool goalset = false;
    public float radis = 0;

    float DelayAttack=0;

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
        baseHP = 35;
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

        // basicAttack = GetComponentInChildren<BaseSkill>();



        // skillList = GetComponentsInChildren<BaseSkill>();

        chosenSkill = skillList[0];

        isActive = true;

    }



    void Update()
    {

        //viewCurrentSkill = chosenSkill;

        /* if(isActive)

         {
             //If we arent Dead...
             if (!isDead)
             {
                 if (!CanAct())
                 {
                     return;
                 }
                 // Update turn cooldown
                 Turn();

                 //Update all cooldowns and conditons
                 UpdateAllConditions();

                 //Choose what attack it want's to make this turn assuming we haven't already chosen.
                 if (!hasDecided)
                 {
                     if (acttackdelay <= 0)
                     {
                         Decide();
                     }
                     else
                     {
                         Debug.LogWarning("enemy is on a break");
                         acttackdelay -= Time.deltaTime;
                     } 
                 }

                 if (acttackdelay <= 0)
                 {


                     //Choose where we want to go, unless we're melee because we will always want to rush the player
                     if (!destinationLocked && type != TYPE.MELEE)
                     {
                         if (chosenSkill != null)
                         {
                             ChooseDestination(chosenSkill);
                         }
                         else
                         {
                             //ChooseDestination(basicAttack);
                             Movement(player.transform.position);

                         }


                     }

                     /// SECTION #1
                     /// //if no skills can be used get closer to the player so when a skill can be used we are in range already
                     if (viewCurrentSkill == null)
                     {
                         //Attack(basicAttack);

                         nav.SetDestination(player.transform.position);
                         FaceTarget(player.transform);

                     }
                     else
                     {
                         if (chosenSkill != null)
                         {
                             Attack(chosenSkill);
                         }

                     }
                     /// SECTION END

                 }

                 //Attack(chosenSkill);
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


                 ///SECTION #2

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
                             IntendedAction(Action.Move);
                             if (type == TYPE.MELEE)
                             {
                                 Movement(player.transform.position);
                             }
                             else if (type == TYPE.RANGED)
                             {
                                 Movement(destination);
                             }
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

             }

                     else
                     {
                         nav.enabled = false;
                     }



             /// SECTION #2 End

         }*/

        ///new set of desiotions
        /*if not currently using skill {
            //if not on action cooldown {
                 //if range 
                    //basicAttack?
                        //which skills can it use
                            //if skill list has one or more skills
                                //decide from skill list
                            //esle
                                //move
                    //else
                        //if basic attack is in range
                            //basic attack
                        //else
                            //move
                        //

                //new functhion skills decide
                    //for each skill
                        //if in range max and min range
                            //do skill
//}
}*/
        // Update turn cooldown

        if (isActive)
        {

            if (!isDead)
            {
                Turn();

                //Update all cooldowns and conditons
                UpdateAllConditions();

                //basic attack is casting

                if (DelayAttack<=0)
                {

                    if (proatication())
                    {
                        return;
                    }
                   
                          

                    //choosen skill is casting //current only using the basic attack(range Var)
                    if (chosenSkill.currentlyCasting)
                    {
                        chosenSkill.TriggerSkill(myEncounter.playerInclusiveInitiativeList);
                        //if finsih casting exit loop with a delay before next attack
                        if (!chosenSkill.currentlyCasting)
                        {
                            isAttacking = false;
                            hasDecided = false;
                            
                            DelayAttack = chosenSkill.skillData.DelayAttack;
                            Debug.LogWarning(chosenSkill.skillData.DelayAttack+chosenSkill.skillData.name);

                            // decide function would go here
                            Deciding();
                        }
                    }
                    else
                    {
                        //move closer
                        if (Vector3.Distance(transform.position, player.transform.position) > (chosenSkill.skillData.maxRange - 2 / 2))
                        {
                            move();
                            goalset = true;
                            HoldTurn();
                        }
                        else
                        {
                            Debug.LogWarning("attack");
                            //stop moving on nav mesh
                            if (nav.enabled != false)
                            {
                                nav.isStopped = true;
                                goalset = false;

                                Debug.LogWarning("stop walking");
                                anim.SetBool("isWalking", false);
                                nav.enabled = false;

                                //a var of dicide function should go here. 
                                //this is to check if a better skill has gone of cooldown
                                Deciding();
                            }
                            //turn to face player then check if you can attack
                            FaceTarget(player.transform);
                            chosenAttack();
                        }
                    }
                }
                else
                {
                    //Debug.LogWarning("reruce");
                    DelayAttack -= Time.deltaTime;
                    FaceTarget(player.transform);
                }
            }
        }
    }

    public void Deciding() {

        chosenSkill = null;
        //For each skill...
        foreach (BasicSkill checkedSkill in skillList)
        {

            //Check if the cooldown is complete...
            if (checkedSkill.timeBeenOnCooldown >= checkedSkill.skillData.cooldown)
            {
                //check to see if the people target by the skills are inrange
                switch (checkedSkill.TargetEntity[0])
                {
                    //if main target is player
                    case "Player":
                        //target is player.
                        //Check if we are in range...
                        if (checkedSkill.CheckInRange(transform.position, target.position))
                        {
                            //Debug.LogWarning("skill");
                            basicSkillChecker(checkedSkill, target.gameObject);
                        }
                        break;

                    case "Self":
                        //no checks need as you are always in range
                        basicSkillChecker(checkedSkill, gameObject);
                        break;

                    default:
                        //target any possable entity which is in the TargetEntity.
                        for (int i = 0; i < checkedSkill.TargetEntity.Count; i++)
                        {
                            //find all posable target
                            GameObject[] AllTargets = GameObject.FindGameObjectsWithTag(checkedSkill.TargetEntity[i]);
                            foreach (var Target in AllTargets)
                            {
                                //check to see if the skill can hit a target
                                if (Vector3.Distance(this.transform.position, Target.transform.position)
                                    <= checkedSkill.skillData.maxRange)
                                {


                                    basicSkillChecker(checkedSkill, Target);
                                    break;
                                }
                            }
                        }

                        //can still cast skill on self

                        break;
                }
            }
        }


    }

    bool proatication()
    {
        bool action = false;

        //enemy behavior
        int a =0;
        switch (a)
        {
            //type of behavior
            case 1:
                //conduction is ment
                if (true)
                {
                    //do action


                    action = true;
                }
                break;
            default:
                break;
        }

        return action;
    }

    void chosenAttack()
    {
       
       
        //in area of attack

        if (chosenSkill.CheckLineSkillHit(target.position,
                chosenSkill.skillData.minRange,
                chosenSkill.skillData.maxRange,
                chosenSkill.skillData.nearWidth,
                chosenSkill.skillData.farWidth))
        {
            // lets attack and move to attack loop 
            if (!isAttacking)
            {
                isAttacking = true;
                anim.SetTrigger("attacking");
            }
            chosenSkill.TriggerSkill(myEncounter.playerInclusiveInitiativeList);
        }
        
    }

    void move()
    {
        //Make sure we are able to move as we disable movement while attacking
        nav.enabled = true;
        //Provide evasive manouvres 
        // Evade();
        if (canMove)
        {
            IntendedAction(Action.Move);
            if (type == TYPE.MELEE)
            {
                Movement(player.transform.position);
            }
            else if (type == TYPE.RANGED)
            {
                //move around player with a bit of a spread
                    ChooseDestination(chosenSkill);

                    Movement(destination);
            }
        }
        else
        {
            nav.SetDestination(Vector3.MoveTowards(transform.position, offset + transform.position, -nav.speed));
            
            canMove = true;
           // nav.SetDestination(Vector3.MoveTowards(transform.position, player.transform.position, nav.speed));

        }
       
    }

   
    


    void EnemyScaling()
    {
        //player.constitution 
    }

    public Vector3 ChooseDestination(BaseSkill skill)
    {
        //find the derction enemy needs to go in

        bool smarterAI = true;

        if (!goalset)
        {
            radis = Random.Range(0, 360);
          //  Debug.LogWarning(radis);
        }

        Vector3 diretion = (this.transform.position - player.transform.position).normalized;

        if (smarterAI)
        {
             destination = new Vector3((((skill.skillData.maxRange -2)/2) * Mathf.Cos(radis)),
                transform.position.y,
                ((skill.skillData.maxRange -2)/2) * Mathf.Sin(radis));

            float x1 = player.transform.position.x + destination.x;
            float z1 = player.transform.position.z  + destination.z;


            return destination = new Vector3(x1, player.transform.position.y, z1);
        }
        else
        {
            

            //debug to show line
            //Debug.DrawLine(player.transform.position, player.transform.position + diretion * skill.skillData.minRange, Color.red, Mathf.Infinity);

            //find closes spot to player
            float x1 = player.transform.position.x + offset.x + diretion.x * (skill.skillData.maxRange - 2) / 2;
            float z1 = player.transform.position.z + offset.x + diretion.z * (skill.skillData.maxRange - 2) / 2;
            offset = Vector3.zero;

            //Otherwise all good, move on!
            return destination = new Vector3(x1, player.transform.position.y, z1);
        }
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

            
               // if (!chosenSkill.currentlyCasting)
               // {

                    ////If the player is using an attack and we're close to death, retreat!
                    //if (player.playerState == Player.PlayerState.DOINGSKILL && currentHP <= maxHP * 0.25f)
                    //{
                    //    nav.enabled = true;
                    //    nav.SetDestination(Vector3.MoveTowards(transform.position, destination, -nav.speed));
                    //}



                    //If we are well within attack range, stop moving towards player
                    //else 
                        if (distance <= (chosenSkill.skillData.maxRange-2) * 0.5f)
                    {
                        Debug.LogWarning("stop walking");
                        anim.SetBool("isWalking", false);
                        nav.enabled = false;
                    }
                    //If we are outside of range, walk towards player
                    else
                    {
                        anim.SetBool("isWalking", true);
                        nav.enabled = true;
                    //nav.SetDestination(Vector3.MoveTowards(transform.position, destination, nav.speed));
                    nav.SetDestination(destination);
                    }


                //}

            }

            if (transform.position == destination)
            {
                destinationLocked = false;
            }
        }

    }

    public void Decide()
    {
        if (enemyCooldown <= 0)
        {

            //Choose how each enemy decides to take its actions
            //Later I would also want the skill cooldowns to come into effect

            //Step 1: If the turn is ready, begin the cycle

        

                    chosenSkill = null;
                    //For each skill...
                    foreach (BasicSkill checkedSkill in skillList)
                    {

                        //Check if the cooldown is complete...
                        if (checkedSkill.timeBeenOnCooldown >= checkedSkill.skillData.cooldown)
                        {
                            //check to see if the people target by the skills are inrange
                            switch (checkedSkill.TargetEntity[0])
                            {
                                //if main target is player
                                case "Player":
                                    //target is player.
                                    //Check if we are in range...
                                    if (checkedSkill.CheckInRange(transform.position, target.position))
                                    {
                                        //Debug.LogWarning("skill");
                                        basicSkillChecker(checkedSkill, target.gameObject);
                                    }
                                    break;

                                case "Self":
                                    //no checks need as you are always in range
                                    basicSkillChecker(checkedSkill, gameObject);
                                    break;

                                default:
                                    //target any possable entity which is in the TargetEntity.
                                    for (int i = 0; i < checkedSkill.TargetEntity.Count; i++)
                                    {
                                        //find all posable target
                                        GameObject[] AllTargets = GameObject.FindGameObjectsWithTag(checkedSkill.TargetEntity[i]);
                                        foreach (var Target in AllTargets)
                                        {
                                            //check to see if the skill can hit a target
                                            if (Vector3.Distance(this.transform.position, Target.transform.position)
                                                <= checkedSkill.skillData.maxRange)
                                            {


                                                basicSkillChecker(checkedSkill, Target);
                                                break;
                                            }
                                        }
                                    }

                                    //can still cast skill on self

                                    break;
                            }
                        }
                    }
                    //if (chosenSkill == null)
                    //{
                    //    chosenSkill = basicAttack;
                    //    isAttacking = true;
                    //    hasDecided = true;

                    //    enemyCooldown = 1.1f;
                    //}
                    // Debug.LogWarning(chosenSkill.name);
        }
    }


   //check to see if viable right now
    public void basicSkillChecker(BasicSkill checkedSkill , GameObject Target)
    {
        //if linear do one more check
        if (checkedSkill.fillType == BaseSkill.CastFillType.LINEAR)
        {
            //More performance intensive, check that the player would be hit if we used it now
            if (!checkedSkill.CheckLineSkillHit(target.position,
                                                checkedSkill.skillData.minRange,
                                                checkedSkill.skillData.maxRange,
                                                checkedSkill.skillData.nearWidth,
                                                checkedSkill.skillData.farWidth))

            {
                return;
            }

        }
        //if skill is healing check to see if they do need healing
        if (checkedSkill.skillData.skill == SkillData.SkillList.HEAL)
        {
            if (Target.GetComponent<SimpleEnemy>().currentHP == Target.GetComponent<SimpleEnemy>().maxHP)
            {
                return;
            }
        }

        //if their is no skill selected yet
        if ((chosenSkill == null))
        {
            isAttacking = true;
            hasDecided = true;
            chosenSkill = checkedSkill;

            //Reset the enemy turn
            enemyCooldown = chosenSkill.skillData.windUp + chosenSkill.skillData.DelayAttack;

            return;
        }

        //Check if damage of prior skill is greater than base damange
        if (chosenSkill.skillData.baseMagnitude <= checkedSkill.skillData.baseMagnitude)
            {
                isAttacking = true;
                hasDecided = true;
                chosenSkill = checkedSkill;

            //Reset the enemy turn
            enemyCooldown = chosenSkill.skillData.windUp + chosenSkill.skillData.DelayAttack;

        }
        

    }

    



 


    public Entity CheckAttackers()
    {

      // Check all enemies
      foreach (Entity enemy in myEncounter.initiativeList)
      {
            //If they are attacking... 
            if (enemy.chosenSkill != null)
            {
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
        ParticleHit();
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

        ItemSpawner.SpawnItem(transform.position);
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


    public override void SetMovement(bool move)
    {
        MovementEnable = move;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy1"))
        {
           // Debug.LogWarning("BUMP");
           
            Vector3 diretion = (this.transform.position - other.transform.position).normalized;
          
           gameObject.transform.parent.transform.Translate(diretion/10);
        }  
    }
}

