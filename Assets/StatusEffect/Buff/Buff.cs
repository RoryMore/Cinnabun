using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new buff", menuName = "ScriptableObjects/Buffeffect", order = 5)]

public class Buff: ScriptableObject
{
    public enum EffectApplyType
    {
        StartSkill,//trigger on BaseSkill
        OnDamage,
        OnHit,//trigger on BaseSkill
              // OnSkill,
        EndSkill,//trigger on BaseSkill
        Death
    }

    public enum BuffStatType
    {
        Attack,
        Death,
        SkillCooldown,
        Dodge
    }
    [Tooltip(" when buff is applyed")]
    public EffectApplyType EfectApplyWhen;

    public float Duration;
    [Tooltip(" what type of buff is applyed. you can edit the list in 'Entity - ConditionEffect'")]
    public Entity.ConditionBuff BuffType;
    [Tooltip(" how large of a buff to selected condition/stat")]
    public float buffnumber;
    public BuffStatType BuffStat;
}
