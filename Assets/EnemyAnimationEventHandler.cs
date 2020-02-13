using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    SoundManager soundManager;


    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EnemyStep()
    {
        soundManager.EnemyStepSound();
       
    }

    void EnemyAttack()
    {
        soundManager.EnemyAttackSound();
    }

    void EnemyHit()
    {
        soundManager.EnemyHitSound();
    }

    void EnemyDeath()
    {
        soundManager.EnemyDeathSound();
    }

    
}
