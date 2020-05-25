using System.Collections;
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
        Vector3 dir = target.position - transform.position;
        //need to find a way to stop enemy from rotating to much y
        dir.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), step);

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

    public override void Death()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        //Ensure that the target is no longer in the initiative 
        myEncounter.initiativeList.Remove(this);
        myEncounter.healList.Remove(this);
        //myEncounter.masterInitiativeList.Remove(this);
        //myEncounter.playerInclusiveInitiativeList.Remove(this);
        nav.enabled = false;

        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }



}

