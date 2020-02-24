using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAnimationEventHandler : MonoBehaviour
{
    //partics effects
    public ParticleSystem[] AttackEffects;
    public ParticleSystem[] DeathEffect;
    public ParticleSystem[] HitEffect;
    public ParticleSystem[] RunEffect;

    void EnemyStep()
    {
        //sound
        //soundManager.EnemyStepSound();
        //play all partics effects which happen 
        //Debug.Log(SoundManager.EnemyFoot.Count);
        int i = UnityEngine.Random.Range(0, SoundManager.EnemyFoot.Count);

       SoundManager.EnemyFoot[i].Play();

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
        int i = UnityEngine.Random.Range(0, SoundManager.EnemyAttack.Count);

        SoundManager.EnemyAttack[i].Play();

        foreach (ParticleSystem item in AttackEffects)
        {
            item.Play();
        }
    }

    void EnemyHit()
    {
        //soundManager.EnemyHitSound();
        //takeDamage
        int i = UnityEngine.Random.Range(0, SoundManager.EnemyHit.Count);

        SoundManager.EnemyHit[i].Play();

        foreach (ParticleSystem item in HitEffect)
        {
            item.Play();
        }
    }

    void EnemyDeath()
    {
        // soundManager.EnemyDeathSound();
        int i = UnityEngine.Random.Range(0, SoundManager.Enemydeath.Count);

        SoundManager.Enemydeath[i].Play();


        foreach (ParticleSystem item in DeathEffect)
        {
            item.Play();
        }

        //stop animation here
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
