using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public enum Type
    {
        WeaponAttack,
        Defend,
        Item,
        Haste,
        Slow,
        Blink,
        Swap,
        InitiativeSwap,
        Wait
    };

    /*public enum BuffOrDebuff
    {
        Buff,
        Debuff,
        Both,
        Irrelevent
    };*/

    [Tooltip("How long (in seconds) the ability takes to cast before the effect happens")]
    public float actionSpeed;

    [Tooltip("The maximum distance the ability can be cast at.")]
    public float range;

    [Tooltip("The 'strength/effectiveness' of the ability")]
    public float magnitude;

    //[Tooltip("If the abilities effects are a buff, debuff, or does/has both, or is this irrelevent")]
    //public BuffOrDebuff buffOrDebuff;

    [Tooltip("How long the ability buff will last (in turns) if it has a lasting effect")]
    public int buffDuration;
    [HideInInspector] public int turnsBuffed;
    [HideInInspector] public bool isBuffActive;

    [Tooltip("How long the ability debuff will last (in turns) if it has a lasting effect")]
    public int debuffDuration;
    [HideInInspector] public int turnsDebuffed;
    [HideInInspector] public bool isDebuffActive;

    [Tooltip("How long you have to wait (in turns) before you can cast the ability again")]
    public int cooldown;
    public int turnsBeenOnCooldown;

    public Type type;

    public int id;

    // Start is called before the first frame update
    void Start()
    {
        turnsBeenOnCooldown = cooldown;

        turnsBuffed = 0;
        isBuffActive = false;

        turnsDebuffed = 0;
        isDebuffActive = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
