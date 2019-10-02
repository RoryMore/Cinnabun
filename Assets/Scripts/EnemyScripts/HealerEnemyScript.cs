//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class HealerEnemyScript : EnemyScript
//{
//    public float healRange;
//    public float attackRange;
//    public int healMagnitude;
//    public int damageMagnitude;
    
    
//    GameObject target;

//    PlayerAttack ourAttack;

//    bool firstTargetAqquired = false;
//    bool isAttacking = false;

//    // Start is called before the first frame update
//    void Awake()
//    {
//        anim = GetComponent<Animator>();
//        //enemyAudio = GetComponent<AudioSource>();
//        hitParticles = GetComponent<ParticleSystem>();
        
//        nav = GetComponent<NavMeshAgent>();

//        player = GameObject.Find("Player").GetComponent<PlayerScript>();

//        turnManger = GameObject.Find("TurnManager").GetComponent<turnManageScript>();

//        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

//        //Value the enemy has to reach before taking their turn
//        enemyCooldown = 6.0f;

//        //Current turn value
//        //enemyTurnCounter = 0.0f;

//        //enemyCooldown = 4.0f + Random.Range(1.0f, 2.0f);
//        initiativeSpeed = 1.0f;
//        currentHealth = startingHealth;


        
        
        


//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isDead != true)
//        {
//            if (turnManger.state == turnManageScript.BattleState.BATTLE || turnManger.state == turnManageScript.BattleState.ACTION)
//            {
//                Movement();
//                Heal();
//                Attack();
//                Turn();
//            }
//        }
//    }

//    public void Movement()
//    {
//        //Once the healer starts, pick a random enemy in the field
//        if (firstTargetAqquired == false)
//        {
//            target = enemyManager.initiativeList[(int)Random.Range(0, enemyManager.initiativeList.Count)];
//            firstTargetAqquired = true;

//            //Heal();
//        }
//        //From this moment on, we only look for the injured
//        // When enemies take damage, 

//        if (currentHealth > 0 && player.currentHealth > 0)
//        {

//            if (Vector3.Distance(transform.position, target.gameObject.transform.position) <= healRange)
//            {
//                nav.SetDestination(transform.position);
//                FaceTarget(target.transform);
//                anim.SetBool("isWalking", false);

//            }
//            else
//            {
//                nav.SetDestination(target.transform.position);
//                anim.SetBool("isWalking", true);
//            }

            
            
//        }
//        else
//        {
//            nav.enabled = false;
//            anim.SetBool("isWalking", false);
//        }
//    }

//    public void Turn()
//    {
//        enemyCooldown -= 1f * Time.deltaTime;
//        //Debug.Log("Enemy Cooldown Counter: " + enemyCooldown);

//    }

//    void Attack()
//    {
//        if (isAttacking == true)
//        {

//            timeSpentDoingAction += Time.fixedDeltaTime;
            
//            ourAttack.DrawCastTimeRangeIndicator(timeSpentDoingAction);
//            anim.SetBool("isAttacking", true);
//            if (timeSpentDoingAction >= ourAttack.actionSpeed)
//            {
            
//                if (ourAttack.ShouldEnemyInPositionBeDamaged(player.transform.position) == true)
//                {
//                    player.TakeDamage(damageMagnitude);
//                }
            
//                //Play Animation
//                enemyCooldown = 6.0f;
//                timeSpentDoingAction = 0.0f;
//                anim.SetBool("isAttacking", false);
//                isAttacking = false;
//            }
//        }
//    }
//    void Heal()
//    {
//        //If the heal list isnt empty
//        if (enemyManager.healList.Count != 0)
//        {
//            //Absurdly large number to remove
//            float previousDistanceToAlly = 20000000000000000000000.0f;
            
//            //Get distance between all enemies in the heal list and yourself, getting the closest one.
//            foreach (GameObject enemy in enemyManager.healList)
//            {
//                float distanceToAlly = Vector3.Distance(transform.position, enemy.transform.position);
//                if (distanceToAlly <= previousDistanceToAlly)
//                {
//                    //The closest one is now your target
//                    //MOVE TO MOVEMENT FUNCTION
//                    target = enemy;
                 
//                }

//                //Make sure our heal is in range
//                if (Vector3.Distance(transform.position, target.transform.position) < healRange && enemyCooldown <= 0.0f)
//                {
//                    //Heal Target
//                    anim.SetBool("isWalking", false);
//                    anim.SetBool("isAttacking", true);
//                    target.GetComponent<EnemyScript>().currentHealth += healMagnitude;
//                    enemyManager.healList.Remove(target);
//                    //Delay
//                    anim.SetBool("isAttacking", false);
//                    enemyCooldown = 6.0f;
//                }
//                //Out of range
//                else if (Vector3.Distance(transform.position, target.transform.position) > healRange && enemyCooldown <= 0.0f)
//                {
//                    // If we need healing, we can heal ourselves
//                    if (enemyManager.healList.Contains(this.gameObject))
//                    {
//                        this.currentHealth += healMagnitude;
//                        enemyManager.healList.Remove(this.gameObject);
//                        enemyCooldown = 6.0f;

//                    }
//                    // Otherwise if the player is in range, we can try attack them
//                    else if ((Vector3.Distance(transform.position, target.transform.position) < attackRange && enemyCooldown <= 0.0f))
//                    {

//                        isAttacking = true;

//                    }

//                    //Assuming we cant attack or heal ANYONE in range, then all we can do is hold turn
//                    else
//                    {
//                        HoldTurn();
//                    }

                    
//                }


//            }
            
//        }

//        //If no one at all needs healing and we are in range, simply attack!
//        else if ((Vector3.Distance(transform.position, target.transform.position) < attackRange && enemyCooldown <= 0.0f))
//        {
//            isAttacking = true;

//        }


//    }

//}
