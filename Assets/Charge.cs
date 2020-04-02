using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{

    BoxCollider Box;
     float Timer;
    public float MaxTimer;
    [SerializeField] bool TimerON = false;
    public float Speed = 0;
    int damage;
    private BasicSkill SKills;

    // Start is called before the first frame update
    void Start()
    {
        SKills = GetComponent<BasicSkill>();
        Box = GetComponentInParent<BoxCollider>();
        if (Speed == 0)
        {
            Speed = GetComponentInParent<SimpleEnemy>().movementSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerON)
        {

            if (Timer <= 0)
            {
                //disable charge and switch to normal
                Box.enabled = false;
                TimerON = false;
                SKills.GetEnitiy().SetMovement(true);
                SKills.skillState = BasicSkill.SkillState.INACTIVE;
            }
            else
            {
                
                gameObject.transform.parent.transform.Translate(transform.parent.forward * Speed, Space.World);
                Timer -= Time.deltaTime;
            }
        }
    }

    public void ExtraSkillProplys(float _damage)
    {
        //start charging
        damage = (int)_damage;
        Box.enabled = true;
        Timer = MaxTimer;
        TimerON = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {

            case "Player":
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                break;

            case "Enemy1":
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                break;

            case "Enemy2":
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                break;
            case "Enemy3":
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                break;
            case " "://crash into evionment
                Timer = 0;
                break;

            default:
                break;
        }
       
    }
}
