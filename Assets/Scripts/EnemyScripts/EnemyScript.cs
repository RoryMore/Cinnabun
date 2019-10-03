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
    public NavMeshAgent nav;

    public List<SkillData> skillList;

    
    public EnemyManager enemyManager;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    //Take Damage
    //enemyManager.healList.Add(this.gameObject);
    //anim.SetTrigger("damaged");

    //Death
    //anim.SetTrigger("Dead");
    //enemyManager.healList.Remove(this.gameObject);
    //enemyManager.initiativeList.Remove(this.gameObject);
    //nav.enabled = false;

}

