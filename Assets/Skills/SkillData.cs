using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public enum DamageType
    {
        PHYSICAL,
        MAGICAL
    }

    public enum SkillShape
    {
        RADIAL,
        LINE
    }

    public enum SkillList  // Go through different Skills with team - discuss. Only small amounts of discussion on skills have been had. Think about Skill tree as well. How many skills we'll have, what "category" they are, if they're all unlocked through the skill tree, or if they're unlocked by doing something in the world or killing something specific
    {
        TELEPORT
    }

    [Tooltip("The maximum distance the skill can be used at from the casters position")]
    public float range;

    [Tooltip("The time (in seconds) that needs to pass before the player is able to cast this skill again")]
    public float cooldown;

    [Tooltip("The time (in seconds) it will take to cast the skill before it does any effect")]
    public float windUp;

    [Tooltip("The 'shape' that this skill will be.\n Radial: Uses Angle and Range values to determine the area from a point it will affect.\n Line: Uses Width and Range values to determine the area from a point it will affect.")]
    public SkillShape shape;

    [Tooltip("Skills that require an angle are skills that use Radial shape.")]
    public float angle;

    [Tooltip("Skills that require a width are skills that use Line shape.")]
    public float width;

    [Tooltip("Which skill this actually is")]
    public SkillList skill;

    [Header("Damage Variables")]
    [Tooltip("The base amount before any additional calculations for how much damage this skill may deal")]
    public int baseDamage;

    [Tooltip("Whether this skill deals Physical or Magical damage")]
    public DamageType damageType;

    // Add access to functions that are of return type bool
    // These functions will calculate if a given objects position, if they are inside an area based on a Mesh for Line skills,
    // and area derived from angles and positions for Radial skills
    // Function parameter for Line skill damage check will require a mesh, and objects position
    // Check in other prototype if any parameters besides objects position is required for radial skill damage check
}
