using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    SoundManager soundManager;
    public ParticleSystem[] AttackEffects;
    public ParticleSystem[] DeathEffect;
    public ParticleSystem[] HitEffect;
    public ParticleSystem[] RunEffect;

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
        //sound
        soundManager.EnemyStepSound();
        //play all partics effects which happen 
        Debug.Log("run effe"+RunEffect.Length);
        foreach (ParticleSystem item in RunEffect)
        {
            if (!item.IsAlive())
            {
                Debug.Log("Play effect");
                item.Play();
            }
        }
    }

    void EnemyAttack()
    {
        soundManager.EnemyAttackSound();

        foreach (ParticleSystem item in AttackEffects)
        {
            item.Play();
        }
    }

    void EnemyHit()
    {
        soundManager.EnemyHitSound();

        foreach (ParticleSystem item in HitEffect)
        {
            item.Play();
        }
    }

    void EnemyDeath()
    {
        soundManager.EnemyDeathSound();

        foreach (ParticleSystem item in DeathEffect)
        {
            item.Play();
        }
    }

    
}
