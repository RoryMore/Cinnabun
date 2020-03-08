using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge1 : BasicSkill
{
    // Start is called before the first frame update
    BoxCollider Box;
    [SerializeField] float Timer;
    float MaxTimer;
    [SerializeField] bool TimerON = false;
    public float Speed=0;
    // Start is called before the first frame update
    void Start()
    {
        base.Initialise();
        Box = GetComponent<BoxCollider>();
        if (Speed == 0)
        {
            Speed = GetComponentInParent<SimpleEnemy>().movementSpeed;
        }
    }

    private void Update()
    {
        if (TimerON)
        {

            if (Timer <= 0)
            {
                Debug.Log("triger");
                Box.enabled = false;
                TimerON = false;

                skillState = SkillState.INACTIVE;
            }
            else
            {
                gameObject.transform.parent.transform.Translate(transform.forward * Speed);
                Timer -= Time.deltaTime;
            }
        }
    }

    protected override void ApplySkillProplys()
    {

        timeBeenOnCooldown = 0.0f;
        timeSpentOnWindUp = 0.0f;
        currentlyCasting = false;

        Box.enabled = true;
        Timer = MaxTimer;
        TimerON = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Entity>().TakeDamage(skillData.baseMagnitude);
        }
        else
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(skillData.baseMagnitude);
            }
        }
    }
}
