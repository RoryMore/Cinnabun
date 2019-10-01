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

    public List<Condition> currentConditions;

    //Attributes
    public int level;
    public int experience;
    [SerializeField] int xpToNextLevel;
    int[] levelBrackets;

    public int strength;
    public int agility;
    public int constitution;
    public int intellect;
    public int physicalArmour;
    public int magicalArmour;

    //Derrived attributes
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] int movementSpeed;
    [SerializeField] int dodgeChance;
    [SerializeField] int physDamagePotential;
    [SerializeField] int magDamagePotential;
    [SerializeField] int experienceRequiredToNextLevel;
    [SerializeField] int physDamageReduction;
    [SerializeField] int magDamageReduction;

    //Condition Immunities
    public bool cannotBeTeleported;


    // Start is called before the first frame update
    void Start()
    {
        currentConditions = new List<Condition>();
        //Make level brackets accurate
        levelBrackets = new int[9];
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


        CalculateMaxHP();
        CalculateMovementSpeed();
        CalculateDodgeChance();
        CalculateMagDamagePotential();
        CalculatePhysDamagePotential();


        //How you give a condition
        //Condition delayedBlastTest = new Condition();
        //delayedBlastTest.duration = 5.0f;
        //delayedBlastTest.conditionType = ConditionType.DELAYEDBLAST;

        //currentConditions.Add(delayedBlastTest);
    }

    // Update is called once per frame
    void Update()
    {
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

    void UpdateAllConditions()
    {
        foreach (Condition condition in currentConditions)
        {
            if (condition.conditionType == ConditionType.DELAYEDBLAST)
            {
                //Do the thing!
                //Wait for player input of kaboom
                //If have condition, detonate and remove condition

                
            }

            //At moment of adding condition
            //timeLeft = condition.duration;

            //timeLeft -= Time.DeltaTime;
            //if (timeLeft >= 0)
            //{
                //remove condition
           // }
        }

    }

    void CalculateMaxHP()
    {
        maxHP = (6 + constitution) * level;
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



}
