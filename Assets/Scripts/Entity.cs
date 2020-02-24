using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    
    public struct Condition
    {
        public int damage;  // If this condition deals damage, this is how much damage is dealt each time damage is dealt
        public float damageTickRate; // The rate at which this condition will deal its damage
        public float duration; //The duration of the condition
        public ConditionType conditionType; //Name of condition
        public float effectivePercent; // If this condition uses any percent value in any way

        public float timePassed;
        public bool begun;

        public Condition(float _duration, ConditionType _conditionType)
        {
            damage = 0;
            damageTickRate = 0;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            effectivePercent = 1.0f;
        }

        public Condition(float _duration, ConditionType _conditionType, int _damage, float _damageTickRate)
        {
            damage = _damage;
            damageTickRate = _damageTickRate;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            effectivePercent = 1.0f;
        }

        public Condition(float _duration, ConditionType _conditionType, float _effectivePercent, int _damage, float _damageTickRate)
        {
            damage = _damage;
            damageTickRate = _damageTickRate;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            effectivePercent = _effectivePercent;
        }

        public void ReduceDuration(float reducedBy)
        {
            duration -= reducedBy;

            timePassed += reducedBy;
        }

        public void ResetTimePassed()
        {
            timePassed = 0.0f;
        }

        public void BeginStart()
        {
            begun = true;
        }
    }

    public enum ConditionType
    {
        DELAYEDBLAST,
        [Tooltip("Deals damage over time based on damage and tickrate")]
        BURN,
         [Tooltip("Deals damage over time based on damage and tickrate, and also applies a slow multiplying movementSpeed by effective percent")]
        POISON
    }

    public struct RewindPoint
    {
        public int currentHealthRewind;
        public bool isDeadRewind;
        public Vector3 locationRewind;
        public Quaternion rotaionRewind;
        public List<Condition> currentConditionsRewind;

    }

    [HideInInspector] public bool rewind = false;

    [Header("Death")]
    public bool isDead;

    [Header("Level")]
    public int level;
    public int experience;
    [SerializeField] int xpToNextLevel;
    int[] levelBrackets;

    [Header("Stats")]
    public int strength;
    public int agility;
    public int constitution;
    public int intellect;
    public int physicalArmour;
    public int magicalArmour;

    [Header("Derived Stats")]
    public int maxHP;
    public int currentHP;
    public float movementSpeed;
    public int dodgeChance;
    public int physDamagePotential;
    public int magDamagePotential;
    public int experienceRequiredToNextLevel;
    public int physDamageReduction;
    public int magDamageReduction;

    [Header("Conditions and Immunities")]
    public List<Condition> currentConditions;
    public bool cannotBeTeleported;

    [Header("Rewind Point")]
    public List<RewindPoint> rewindPoints;

    [Header("Encounter")]
    public static Encounter currentEncounter;

    [Header("Damaged VFX")]
    [SerializeField]
    GameObject explosionParticles;

    // Variables needed for enemies to function efficiently without additional list
    [HideInInspector]
    public Vector3 destination;

    [HideInInspector]
    public BaseSkill chosenSkill;

    [HideInInspector]
    public NavMeshAgent nav;


    // Data for original values
    float originalMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //set ragdoll to false
        SetRigidBodyState(true);
        SetColliderState(false);

        //How you give a condition
        //Condition delayedBlastTest = new Condition();
        //delayedBlastTest.duration = 5.0f;
        //delayedBlastTest.conditionType = ConditionType.DELAYEDBLAST;

        //currentConditions.Add(delayedBlastTest);
    }

    // Update is called once per frame
    void Update()
    {
        //mAKE SURE to update all conditions for any other object that inhereits from entity
        UpdateAllConditions();

    }

     void GiveExperience(int value)
    {
        experience += value;
        if (experience >= levelBrackets[level])
        {
            //You leveled up!
            level++;

            CalculateMaxHP();
            CalculateMovementSpeed();
            CalculateDodgeChance();
            CalculateMagDamagePotential();
            CalculatePhysDamagePotential();

        }
    }

    //Function that is called when the player deals damage to you
    //Default condition format
    public virtual void TakeDamage(int amount)
    {
        Debug.Log("OOF x " + amount);
        if (isDead)
            return;

        currentHP -= amount;

        if (currentHP <= 0)
        {
            Debug.Log("here");
            if (gameObject.name == "Player")
            {
                //stop player form doing anything
                gameObject.GetComponentInChildren<Player>().enabled = false;
               //stop animating
                gameObject.GetComponentInChildren<Animator>().enabled = false;
                //stop all enemy for attacting //possable Win animation for enemy
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>().PlayerDeath();
                
            }
            else
            {
                //it is a enemy which die
                gameObject.GetComponentInChildren<Animator>().enabled = false;
                gameObject.GetComponentInChildren<SimpleEnemy>().enabled = false;
            }
            SetRigidBodyState(false);
            //migth stop rewind
           SetColliderState(true);
            Death();
            
            
        }
    }

   
   

    public virtual void Death()
    {
        isDead = true;

    }

    public void UpdateAllConditions()
    {
        if (currentConditions.Count != 0)
        {
            int conditionIndex = 0;
            foreach (Condition condition in currentConditions)
            {
                if (condition.conditionType == ConditionType.DELAYEDBLAST)
                {
                    //Do the thing!
                    //Wait for player input of kaboom
                    //If have condition, detonate and remove condition

                }
                else if (condition.conditionType == ConditionType.BURN)
                {
                    // Does this condition still have time to continue being active?
                    if (condition.duration > 0)
                    {
                        // Reduce the condition time
                        condition.ReduceDuration(Time.deltaTime);

                        // After every second deal burn damage
                        if (condition.timePassed >= condition.damageTickRate)
                        {
                            TakeDamage(condition.damage);

                            // Reset the timePassed value
                            condition.ResetTimePassed();
                        }
                    }
                    else
                    {
                        // This condition has timed out
                        // Remove it
                        currentConditions.RemoveAt(conditionIndex);
                    }
                }
                else if (condition.conditionType == ConditionType.POISON)
                {
                    if (!condition.begun)
                    {
                        condition.BeginStart();
                        originalMovementSpeed = movementSpeed;

                        movementSpeed = movementSpeed * condition.effectivePercent;
                    }
                    if (condition.duration > 0)
                    {
                        condition.ReduceDuration(Time.deltaTime);

                        if (condition.timePassed >= condition.damageTickRate)
                        {
                            TakeDamage(condition.damage);
                            condition.ResetTimePassed();
                        }
                    }
                    else
                    {
                        movementSpeed = originalMovementSpeed;

                        // This condition has timed out
                        // Remove it
                        currentConditions.RemoveAt(conditionIndex);
                    }
                }

                //At moment of adding condition
                //timeLeft = condition.duration;

                //timeLeft -= Time.DeltaTime;
                //if (timeLeft >= 0)
                //{
                //remove condition
                // }

                conditionIndex++;
            }
        }

        RecordRewind();
    }

    void CalculateMaxHP()
    {
        maxHP = ((5 + constitution) * level) * 10;
        currentHP = maxHP;
    }

    void CalculateMovementSpeed()
    {
        movementSpeed = agility;
    }

    void CalculateDodgeChance()
    {
        dodgeChance = 0; //No default, this is for later balance purpose potential
    }

    void CalculatePhysDamagePotential()
    {
        physDamagePotential = Mathf.Abs(strength * (strength / 2)) * level;
    }

    void CalculateMagDamagePotential()
    {
        magDamagePotential = Mathf.Abs(intellect * (intellect / 2)) * level;
    }

    public void InitialiseAll()
    {
        currentConditions = new List<Condition>();
        
        rewindPoints = new List<RewindPoint>();
        

        //Make level brackets accurate
        levelBrackets = new int[10];
        levelBrackets[0] = 0;
        levelBrackets[1] = 100;
        levelBrackets[2] = 250;
        levelBrackets[3] = 600;
        levelBrackets[4] = 1350;
        levelBrackets[5] = 2900;
        levelBrackets[6] = 6050;
        levelBrackets[7] = 16000;
        levelBrackets[8] = 32350;
        levelBrackets[9] = 65100;

        CalculateAllDerivedStats();

        isDead = false;
    }

    public void CalculateAllDerivedStats()
    {
        CalculateMaxHP();
        CalculateMovementSpeed();
        CalculateDodgeChance();
        CalculateMagDamagePotential();
        CalculatePhysDamagePotential();
    }


    public List<Condition> ReturnConditions()
    {
        return currentConditions;
    }

    public Encounter ReturnEncounter()
    {
        return currentEncounter;
    }

    public static void SetCurrentEncounter(Encounter encounter)
    {
        currentEncounter = encounter;
    }

    //Rewind Features and Functions

    public void RecordRewind()
    {
        RewindPoint temp;

        temp.currentHealthRewind = currentHP;
        temp.isDeadRewind = isDead;
        temp.locationRewind = transform.position;
        temp.rotaionRewind = transform.rotation;
        temp.currentConditionsRewind = currentConditions;

        rewindPoints.Add(temp);
    }
    public void RewindBack()
    {
        RewindPoint point = new RewindPoint();
        point = rewindPoints[0];
        Debug.Log(point.locationRewind);

        if (nav != null)
        {
            nav.Warp(point.locationRewind);
        }
        //transform.position = point.locationRewind;
        currentHP = point.currentHealthRewind;
        isDead = point.isDeadRewind;
        currentConditions = point.currentConditionsRewind;
        transform.rotation = point.rotaionRewind;
       // transform.position = new Vector3(0, 0, 0);
        rewind = false;

       
    }

    public void ClearList()
    {
        Debug.Log("List clear!");
        rewindPoints.Clear();
    }

    //End of Rewind Features

    public void ParticleExplosion()
    {
        if (explosionParticles != null)
        {
            explosionParticles.SetActive(false);
            explosionParticles.SetActive(true);
        }
    }
    public void SetRigidBodyState(bool currentState)
    {
        Rigidbody[] Rigidbody = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody item in Rigidbody)
        {
            item.isKinematic = currentState;
        }
       // GetComponent<Rigidbody>().isKinematic = !currentState;
    }
    public void SetColliderState(bool currentState)
    {
        Collider[] Rigidbody = GetComponentsInChildren<Collider>();
        foreach (Collider item in Rigidbody)
        {
            item.enabled = currentState;
        }
        //GetComponent<Collider>().enabled = !currentState;
    }
}
