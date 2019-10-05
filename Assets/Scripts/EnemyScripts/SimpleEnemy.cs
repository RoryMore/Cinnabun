using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    public float meleeAttackRange;
    

    Transform target;

    bool isAttacking = false;

    BasicAttack simpleBasicAttack;

    

    private void Start()
    {
        InitialiseAll();

        //GetComponent<Entity>().rewindPoint = new RewindPoint();



        nav = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {

        target = GameObject.Find("Player").transform;
        skillList[0].rangeIndicator.Init(skillList[0].shape, skillList[0].angleWidth);


        //foreach (SkillData skill in skillList)
        //{
        //    if (skill.cooldown != 0)
        //    {
        //        //This skill is not ready to use!
        //    }
        //    else
        //    {

        //    }
        //}



        
        

    }


    void Update()
    {
        Movement();
        UpdateAllConditions();

        
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            skillList[0].currentlyCasting = true;
            //simpleBasicAttack.currentlyCasting = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print(rewindPoint.locationRewind.position.x);
            //SaveRewindPoint();
            
            Debug.Log("Saved Point");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadRewindPoint();
            Debug.Log("Loaded Point");
        }

        if (skillList[0].currentlyCasting == true)
        {
            skillList[0].CastSkill(transform, skillList[0].shape);
        }

    }


    public void Movement()
    {
        nav.SetDestination(target.transform.position);

    }

    //TEMPORARY FUNCTION FOR WHEN JASMINE FINISHES HER TURN COUNTER



}

