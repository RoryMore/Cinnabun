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

    public EffectApplyType EfectApplyWhen;

    public float Duration;
    public Entity.ConditionBuff BuffType;

    public float buffnumber;
    public BuffStatType BuffStat;
}
