using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new stats", menuName = "ScriptableObjects/StatusEffect", order = 4) ]
public class Effects : ScriptableObject
{
    public enum EffecteType
    {
        Percent,//not imputated yet
        Number
    }
    public enum EffectApplyType 
    {
        StartSkill,//trigger on BaseSkill
        OnDamage,
        OnHit,//trigger on BaseSkill
       // OnSkill,
        EndSkill,//trigger on BaseSkill
        Death
    }
    [Header("Effect appucation")]
    [Tooltip(" when Effect is applyed")]
    public EffectApplyType EfectApplyWhen;

    public EffecteType EffectType;
    [Tooltip("what type of effect is applyed. you can edit the list in 'Entity - ConditionEffect'")]
    public Entity.ConditionEffect Effect;

    [Header("duration")]
    [Tooltip("Effect that don't get removed, if they do then (currently not impulated) ")]
    public bool permanent;
    [Tooltip("How Long it effect last for")] 
    public float Duration;
    

    [Header("Damage")]
    [Tooltip("is buff type is percent strength is the percent amount, if number then strength is a falt. if 'Skill' is 'HEAL' heals intsead")]
    public float Damage;
    [Tooltip("damage which happens every second. if 'Skill' is 'HEAL' heals intsead")]public float TickDamage;
}
