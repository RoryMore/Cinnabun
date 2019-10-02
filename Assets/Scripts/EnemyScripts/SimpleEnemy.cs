using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemy : EnemyScript
{

    public float meleeAttackRange;
    

    Transform target;

    bool isAttacking = false;

    void Awake()
    {

        target = GameObject.Find("Player").transform;

        





    }


    void Update()
    {
        Movement();


    }


    public void Movement()
    {
        nav.SetDestination(target.transform.position);
    }

    //TEMPORARY FUNCTION FOR WHEN JASMINE FINISHES HER TURN COUNTER



}

