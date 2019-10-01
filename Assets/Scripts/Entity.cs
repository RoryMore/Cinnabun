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

    public int strength;
    public int aglility;
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



    // Start is called before the first frame update
    void Start()
    {
        currentConditions = new List<Condition>();

        Condition delayedBlastTest = new Condition();
        delayedBlastTest.duration = 5.0f;
        delayedBlastTest.conditionType = ConditionType.DELAYEDBLAST;

        currentConditions.Add(delayedBlastTest);

        CalculateMaxHP();
        CalculateMovementSpeed();
        CalculateDodgeChance();
        CalculateMagDamagePotential();
        CalculatePhysDamagePotential();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAllConditions();
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

                //Debug.Log("You have explosion sickness");
            }
        }
        
    }

    void CalculateMaxHP()
    {
        maxHP = (6 + constitution) * level;
    }

    void CalculateMovementSpeed()
    {
        movementSpeed = aglility;
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
