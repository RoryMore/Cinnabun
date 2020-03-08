using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new stats", menuName = "ScriptableObjects/StatusEffect", order = 4) ]
public class Effects : ScriptableObject
{
    public enum buffeType
    {
        Percent,
        Number
    }
    public enum EffectApplyType 
    {
        StartSkill,//trigger on BaseSkill
        OnDamage,
        OnHit,//trigger on BaseSkill
        OnSkill,
        EndSkill,//trigger on BaseSkill
        Death
    }
    public EffectApplyType EfectApplyWhen;

#if EfectApplyWhen == OnSkill
    [Tooltip("Name of the skill which causes this effect")] public string Skill;
#endif

    public Entity.ConditionEffect Effect;
    [Tooltip("Effect that don't get removed, if they do then ")]
    public bool permanent;
    [Tooltip("How Long it effect last for")] 
    public float Duration;
    public buffeType BuffType;

    [Tooltip("is buff type is percent strength is the percent amount, if number then strength is a falt. - heals intsead")]
    public float Damage;
    [Tooltip(" - heals intsead")]public float TickDamage;
}
