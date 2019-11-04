using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    
    public struct Condition
    {

        public float duration; //The duration of the condition
        public ConditionType conditionType; //Name of condition

    }

    public enum ConditionType
    {
        DELAYEDBLAST,
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
    public int movementSpeed;
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
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public SkillData chosenSkill;


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
        Debug.Log("OOF x " + amount);
        if (isDead)
            return;

        currentHP -= amount;

        if (currentHP <= 0)
        {
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
            foreach (Condition condition in currentConditions)
            {


                //At moment of adding condition
                //timeLeft = condition.duration;

                //timeLeft -= Time.DeltaTime;
                //if (timeLeft >= 0)
                //{
                //remove condition
                // }
            }
        }

        RecordRewind();
    }

    void CalculateMaxHP()
    {
        maxHP = (6 + constitution) * level;
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

    public void SetCurrentEncounter(Encounter encounter)
    {
        currentEncounter = encounter;
    }

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
        transform.position = point.locationRewind;
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

    public void ParticleExplosion()
    {
        if (explosionParticles != null)
        {
            explosionParticles.SetActive(false);
            explosionParticles.SetActive(true);
        }
    }
}
