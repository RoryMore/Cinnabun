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
        //soundManager.EnemyStepSound();
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
        //soundManager.EnemyAttackSound();

        foreach (ParticleSystem item in AttackEffects)
        {
            item.Play();
        }
    }

    void EnemyHit()
    {
        //soundManager.EnemyHitSound();
        //takeDamage
        foreach (ParticleSystem item in HitEffect)
        {
            item.Play();
        }
    }

    void EnemyDeath()
    {
       // soundManager.EnemyDeathSound();

        foreach (ParticleSystem item in DeathEffect)
        {
            item.Play();
        }
    }


    //public void EnemyStepSound()
    //{

    //    AudioClip clip = enemyFootsteps[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
    //    //Debug.Log(clip.name);
    //    enemyFootAudioSource.PlayOneShot(clip);

    //}

    //public void EnemyAttackSound()
    //{

    //    AudioClip clip = enemyAttacks[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
    //    //Debug.Log(clip.name);
    //    enemyAttackAudioSource.PlayOneShot(clip);

    //}

    //public void EnemyHitSound()
    //{

    //    AudioClip clip = enemyHits[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
    //    //Debug.Log(clip.name);
    //    enemyHitAudioSource.PlayOneShot(clip);

    //}

    //public void EnemyDeathSound()
    //{

    //    AudioClip clip = enemyDeath[UnityEngine.Random.Range(0, enemyFootsteps.Length)];
    //    //Debug.Log(clip.name);
    //    enemyDeathAudioSource.PlayOneShot(clip);

    //}
}
