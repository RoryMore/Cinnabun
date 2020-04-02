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
        public ConditionEffect conditionType; //Name of condition
        public int BuffStat;
        public float effectivePercent; // If this condition uses any percent value in any way

        public float timePassed;
        public bool begun;

       

        public Condition(float _duration, ConditionEffect _conditionType)
        {
            damage = 0;
            damageTickRate = 0;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            BuffStat = 0;
            effectivePercent = 1.0f;
        }
        public Condition(float _duration, ConditionEffect _conditionType, float Effectiveness)
        {
            damage = 0;
            damageTickRate = 0;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            BuffStat = 0;
            effectivePercent = Effectiveness;
        }

        public Condition(float _duration, ConditionEffect _conditionType, int _damage, float _damageTickRate)
        {
            damage = _damage;
            damageTickRate = _damageTickRate;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            BuffStat = 0;
            effectivePercent = 1.0f;
        }

        public Condition(float _duration, ConditionEffect _conditionType, float _effectivePercent, int _damage, float _damageTickRate)
        {
            damage = _damage;
            damageTickRate = _damageTickRate;
            duration = _duration;
            conditionType = _conditionType;
            timePassed = 0;
            begun = false;
            BuffStat = 0;
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
    public enum ConditionTypeA
    {
        DELAYEDBLAST,
        [Tooltip("Deals damage over time based on damage and tickrate")]
        BURN,
        [Tooltip("Deals damage over time based on damage and tickrate, and also applies a slow multiplying movementSpeed by effective percent")]
        POISON
    }
    public enum ConditionEffect
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
    [SerializeField]
    protected float baseMovementSpeed;
    public float movementSpeed;
    public int dodgeChance;
    public int physDamagePotential;
    public int magDamagePotential;
    public int experienceRequiredToNextLevel;
    public int physDamageReduction;
    public int magDamageReduction;
    protected float criticalStrikeMultiplier;

    [Header("Conditions and Immunities")]
    public List<Condition> currentConditions;
    public bool cannotBeTeleported;

    [Header("Rewind Point")]
    public List<RewindPoint> rewindPoints;

    [Header("Encounter")]
    public static Encounter currentEncounter;

    [Header("VFX")]
    //GameObject explosionParticles;    // DelayedBlast handles the explosion now
    [SerializeField]
    GameObject hitParticles;

    // Variables needed for enemies to function efficiently without additional list
    [HideInInspector]
    public Vector3 destination;

    [HideInInspector]
    public BaseSkill chosenSkill;

    [HideInInspector]
    public NavMeshAgent nav;

    [SerializeField]
    protected GameObject damageNumber;

    // Data for original values
    float originalMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {



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
        //Debug.Log("OOF x " + amount);
        if (isDead)
            return;

        currentHP -= amount;

        if (currentHP <= 0)
        {
            Death();
        }
    }
    //check to see if any possable actions can be made. if not the return fasle.
    public virtual bool CanAct() {
        foreach (var item in currentEffConditions)
        {
            switch (item.conditionType)
            {
                case ConditionEffect.STUN:
                    Debug.LogWarning("stuned");
                    return false;
                case ConditionEffect.SPIKED:
                    break;
                    default:
                    break;
            }
        }
        return true;
    }
    //determinated if the action the entity wants have any consequences
    public virtual bool IntendedAction(Action action)
    {
        int temp = 0;
        foreach (var item in currentEffConditions)
        {
            switch (item.conditionType)
            {
                case ConditionEffect.STUN:
                    break;
                case ConditionEffect.SPIKED:
                    switch (action)
                    {
                        case Action.Move:
                            if (item.delay <=0)
                            {
                                TakeDamage((int)item.damageTickRate, SkillData.DamageType.PHYSICAL, false);
                                item.ResetDelay(0.5f);
                                Debug.LogWarning(("delay reset" + item.delay));
                                currentEffConditions[temp] = item;
                            }
                            else
                            {
                                item.ReduceDelay(Time.deltaTime);
                                Debug.LogWarning(("delay" + item.delay));
                                currentEffConditions[temp] = item;
                               
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            temp++;
        }
        return true;
    }
    //public virtual void DealAttack(SkillData damageType)
    //{

    //}

    public struct BUffEFffect
    {
        public int cooldown;
        public float damage;

        public void Init(){
            cooldown = 0;
            damage = 0;
        }
        public void CooldownMinus(int Minus){ cooldown -= Minus; }
        public void DamagePlus(int Plus) { damage -= Plus; }
    }

    public BUffEFffect ApplyBuffOff()
    {
        BUffEFffect buff;
        buff.cooldown = 0;
        buff.damage = 0;
        buff.Init();

        buff.cooldown = 0;
        if (currentBufConditions.Count != 0)
        {
            foreach (var item in currentBufConditions)
            {


                switch (item.conditionType)
                {
                    case ConditionBuff.FOCUS:
                        buff.cooldown -= (int)item.Buff;
                        break;
                    case ConditionBuff.RAGE:
                        buff.damage += (int)item.Buff;
                        break;
                    case ConditionBuff.DAMAGEBUFF:
                        buff.damage = (buff.damage * item.Buff);
                        currentBufConditions.Remove(item);
                        break;
                    default:
                        break;
                }
            }
        }
        return buff;
    }
    public virtual void TakeHealth(int amount)
    {
        if (currentHP <= 0)
        {
            Death();
            return;
        }
        if (currentHP+amount > maxHP)
        {
            amount = maxHP - currentHP;
            currentHP = maxHP;
        }
        else
        {
            currentHP += amount;
        }

        Vector3 popUpSpawn = new Vector3(Random.Range(-0.9f, 0.3f), Random.Range(-0.9f, 0.3f) + 3, 0);

        DamagePopUp damagePopUpNumber = Instantiate(damageNumber, transform.position + popUpSpawn, Quaternion.identity).GetComponent<DamagePopUp>();
        damagePopUpNumber.SetUp(amount, false);
    }


    //add cooldown
    public virtual void TakeDamage(int amount, SkillData.DamageType damageType, bool isCrit)
    {
        // Code inside here may actually be irrelevant. Actual characters (player/enemies) can use this function in their class which can call the other TakeDamage function in itself
        if (isDead)
        {
            return;
        }

        int damageTaken = Mathf.Clamp(amount - DamageNegated(amount, damageType), 0, int.MaxValue);
        if (isCrit)
        {
            damageTaken = Mathf.RoundToInt(damageTaken * criticalStrikeMultiplier);
        }

        Vector3 popUpSpawn = new Vector3(Random.Range(-0.9f, 0.3f), Random.Range(-0.9f, 0.3f) + 3, 0);

        DamagePopUp damagePopUpNumber = Instantiate(damageNumber, transform.position + popUpSpawn, Quaternion.identity).GetComponent<DamagePopUp>();
        damagePopUpNumber.SetUp(damageTaken, isCrit);

        TakeDamage(damageTaken);

        //currentHP -= amount;

        //if (currentHP <= 0)
        //{
        //    Death();
        //}
    }


    public virtual void Death()
    {
        isDead = true;

    }

    public void UpdateAllConditions()
    {
       //update all effects
        if (currentEffConditions.Count != 0)
        {
            int conditionIndex = 0;
            foreach (Condition condition in currentConditions)
            {
                if (condition.conditionType == ConditionEffect.DELAYEDBLAST)
                {
                    //reduce condition duration
                    condition.ReduceDuration(Time.deltaTime);
                    currentEffConditions[conditionIndex] = condition;
                }
                else if (condition.conditionType == ConditionEffect.BURN)
                {
                    // Does this condition still have time to continue being active?
                    if (condition.duration > 0)
                    {
                        // Reduce the condition time
                        condition.ReduceDuration(Time.deltaTime);

        //update all buffs
        if (currentBufConditions.Count != 0)
        {
            int conditionIndex = 0;
            foreach (ConditionBuf condition in currentBufConditions.ToArray())
            {
                
                if (condition.duration > 0)
                {
                    condition.ReduceDuration(Time.deltaTime);
                    currentBufConditions[conditionIndex] = condition;
                    
                }
                else if (condition.conditionType == ConditionEffect.POISON)
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

    public void CalculateMaxHP()
    {
        // probably needs rebalancing imo
        maxHP = ((5 + constitution) * level) * 10;
        //currentHP = maxHP;
    }

    void CalculateMovementSpeed()
    {
        // I believe each unit should equal something along the lines of
        float agilityEffectiveness = 0.1f;
        float agilityPointThreshold = 10.0f;
        movementSpeed = baseMovementSpeed + (baseMovementSpeed * ((agility * agilityEffectiveness) / agilityPointThreshold));
        // for each 10 points of agility you move 10% faster with this formula.

        //movementSpeed = agility;
    }
    // TODO: Implement function for Critical Chance
    public virtual bool CalculateCriticalStrike()
    {
        float agilityEffectiveness = 0.1f;
        float agilityPointThreshold = 25.0f;

        // For every [agilityPointThreshold] points of agility, we gain [agilityEffectiveness * 100]% crit strike
        float result = agilityEffectiveness * (agility / agilityPointThreshold);

        if (Random.Range(0.0f, 1.0f) <= result)
        {
            return true;
        }

        return false;
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

    // ENTITY ADDITIONS BY SUNNY TO MAKE OUR STATS USEFUL FOR SKILLS - IMPLEMENT WHERE NEEDED
    // PUBLIC SO SKILLS CAN GRAB THIS INT AS EXTRA DAMAGE FOR THEIR ATTACKS
    // with these functions, idk what damagePotential is going to do for us as a stat. Those formulas above seem to snowball pretty hard
    public int GetStrengthDamageBonus()
    {
        // We get 25% of our strength value as added damage to physical attacks
        float strengthEffectiveness = 0.5f;
        // Should we floor to an int, or round to nearest
        // Floor could be safe enough, for now this function means we get 1 extra damage every 2 strength
        return Mathf.FloorToInt(strength * strengthEffectiveness);
    }

    public int GetIntellectDamageBonus()
    {
        // We get 25% of our intellect value as added damage to magical attacks
        float intellectEffectiveness = 0.5f;
        return Mathf.FloorToInt(intellect * intellectEffectiveness);
    }

    /// <summary>
    /// Minus the Return value of this function from the damage being taken to reduce damage properly
    /// </summary>
    /// <param name="originalDamage"></param>
    /// <param name="damageType"></param>
    /// <returns></returns>
    protected int DamageNegated(int originalDamage, SkillData.DamageType damageType)
    {
        // How effective armour is at 'armourPointThreshold' points of armour
        // 0.25f effectiveness && 100.0f threshold = 25% damage reduction at 100 points of armour
        float armourEffectiveness = 0.25f;
        float armourPointThreshold = 100.0f;

        if (damageType == SkillData.DamageType.PHYSICAL)
        {
            float percentReduced = Mathf.Clamp(armourEffectiveness * (physicalArmour / armourPointThreshold), 0, 0.95f);
            return Mathf.RoundToInt(originalDamage * percentReduced);
        }
        else
        {
            float percentReduced = Mathf.Clamp(armourEffectiveness * (magicalArmour / armourPointThreshold), 0, 0.95f);
            return Mathf.RoundToInt(originalDamage * percentReduced);
        }
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

        criticalStrikeMultiplier = 1.5f;
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
        //Debug.Log("List clear!");
        rewindPoints.Clear();
    }

    //End of Rewind Features

    public void ParticleHit()
    {
        if (hitParticles != null)
        {
            hitParticles.SetActive(false);
            hitParticles.SetActive(true);
        }
    }
}
