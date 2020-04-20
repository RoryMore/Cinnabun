using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public enum DamageType
    {
        PHYSICAL,
        MAGICAL
    }

    public enum SkillList
    {
        TELEPORT,
        DELAYEDBLAST,
        REWIND,
        WEAPONATTACK,
        BUFF,
        HEAL,
        AOE,
        STATUS
    }

    [Header("Ranges")]
    [Tooltip("The maximum distance the skill can be used at from the casters position. \nFor skills that want a constant radius sized indicator that is moveable, maxRange can be used as the radius of circle indicator")]
    public float maxRange;
    [Tooltip("The minimum distance the skill can have an effect from the casters position. \nFor skills that want a constant radius sized indicator that is moveable, minRange can be used as the maximum distance the skill can be cast from the caster")]
    public float minRange;
    [Tooltip("The width of a rectangular shaped skill at the maxRange")]
    public float farWidth;
    [Tooltip("The width of a rectangular shaped skill at the minRange")]
    public float nearWidth;

    [Tooltip("Half the maximum height difference allowed between units that makes it possible to be hit or not \nProjector near and far clip planes will equal this value")]
    public float verticalRange;

    [Tooltip("The Width a line Skill will have, or the Angle a Radial skill will use")]
    public float angle;

    [Header("Skill Delays")]
    [Tooltip("The time (in seconds) that needs to pass before the player is able to cast this skill again")]
    public float cooldown;

    [Tooltip("The time (in seconds) it will take to cast the skill before it does any effect")]
    public float windUp;

    [Tooltip("delay before next attack")]
    public float DelayAttack;

    [Tooltip("Which skill this actually is")]
    public SkillList skill;

    [Header("Damage Variables")]
    [Tooltip("The base amount before any additional calculations for how much damage/heal/etc. this skill may deal")]
    public int baseMagnitude;

    [Tooltip("Whether this skill deals Physical or Magical damage")]
    public DamageType damageType;   
}
