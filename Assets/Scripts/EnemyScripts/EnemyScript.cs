﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The base enemy script from which all enemies derrive 

public class EnemyScript : Entity
{


    //Value required before they take their action
    public float enemyCooldown;
    //Value of their initiative speed
    public float initiativeSpeed;
    
    public bool enemyTakingAction;

    public float turnsHeld;

    public float timeSpentDoingAction = 0.0f;

    public float turnSpeed = 1.0f;

    //public AudioClip deathClip;


    public Animator anim;
    //AudioSource enemyAudio;
    public ParticleSystem hitParticles;
    public NavMeshAgent nav;
    public List<SkillData> skillList;

    public EnemyManager enemyManager;
    public Encounter myEncounter;


    public void Turn()
    {
        enemyCooldown -= 1f * Time.deltaTime;
    }

    //Every time the enemy is unable to act on their turn, simply reset their initiative with a slight stacking advantage
    public void HoldTurn()
    {
        turnsHeld += 1;
        enemyCooldown = 6.0f - (0.5f * turnsHeld);
    }


    public void FaceTarget(Transform target)
    {
        var step = turnSpeed * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);

    }

    public override void TakeDamage(int amount)
    {
        Debug.Log("OOF x " + amount);
        if (isDead)
            return;

        currentHP -= amount;
        myEncounter.healList.Add(this);
        //anim.SetTrigger("damaged");

        if (currentHP <= 0)
        {
            Death();
        }
    }

    public void UpdateAllSkillCooldowns()
    {
        foreach (SkillData skill in skillList)
        {
            skill.ProgressCooldown();
        }
    }

    public override void Death()
    {
        isDead = true;
        //anim.SetTrigger("Dead");
        //Ensure that the target is no longer in the initiative 
        myEncounter.initiativeList.Remove(this);
        myEncounter.healList.Remove(this);
        nav.enabled = false;
    }



}

