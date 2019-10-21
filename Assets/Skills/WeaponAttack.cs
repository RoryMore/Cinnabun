using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponAttack", order = 1)]
public class WeaponAttack : SkillData
{
    public enum UsedWeaponType
    {
        Unarmed,
        Sword,
        Staff,
        Bow
    }
    public UsedWeaponType usedWeapon;
    UsedWeaponType oldWeapon;

    [Header("Overwritten Ranges - USE THESE")]
    public float unarmedRange;
    public float swordRange;
    public float staffRange;
    public float bowRange;

    [Header("Overwritten Damage Values for Weapons")]
    public int baseUnarmedDamage;
    public int baseSwordDamage;
    public int baseStaffDamage;
    public int baseBowDamage;

    [Tooltip("The width the line indicator will use. \nAngleWidth will be what is used for sword")]
    public float lineWidth;

    Entity entityTarget = null;
    Entity caster = null;

    bool attackAreaChosen = false;

    public override void Initialise(Entity ownCaster)
    {
        base.Initialise();
        oldWeapon = usedWeapon;
        caster = ownCaster;
    }

    public override void TargetSkill(Transform zoneStart, List<Entity> entityList)
    {
        
        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                if (oldWeapon != usedWeapon)
                {
                    rangeIndicator.Init(SkillShape.RADIAL, 360.0f);
                    oldWeapon = usedWeapon;
                }
                if (entityTarget == null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit.point.x, zoneStart.position.y, hit.point.z);
                        zoneStart.LookAt(lookAt);
                    }

                    DrawRangeIndicator(zoneStart, shape, unarmedRange, 360.0f);

                    // Select who we're punching
                    SelectTargetRay(zoneStart, ref entityTarget, true);
                }
                else if (entityTarget != null)
                {
                    // We are not hitting ourselves
                    if (entityTarget != caster)
                    {
                        CastSkill(zoneStart, entityList);
                    }
                    else // Woops we tried to punch ourselves
                    {
                        entityTarget = null;
                    }
                }
                break;

            case UsedWeaponType.Sword:
                if (oldWeapon != usedWeapon)
                {
                    rangeIndicator.Init(SkillShape.RADIAL, angleWidth);
                    oldWeapon = usedWeapon;
                }
                if (!attackAreaChosen)
                {
                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit2.point.x, zoneStart.position.y, hit2.point.z);
                        zoneStart.LookAt(lookAt);
                    }

                    DrawRangeIndicator(zoneStart, shape, swordRange, angleWidth);

                    if (Input.GetMouseButtonDown(0))
                    {
                        attackAreaChosen = true;
                    }
                }
                else
                {
                    CastSkill(zoneStart, entityList);
                }
                break;

            case UsedWeaponType.Staff:
                if (oldWeapon != usedWeapon)
                {
                    rangeIndicator.Init(SkillShape.LINE, lineWidth);
                    oldWeapon = usedWeapon;
                }
                if (!attackAreaChosen)
                {
                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit2.point.x, zoneStart.position.y, hit2.point.z);
                        zoneStart.LookAt(lookAt);
                    }

                    DrawRangeIndicator(zoneStart, shape, staffRange, lineWidth);

                    if (Input.GetMouseButtonDown(0))
                    {
                        attackAreaChosen = true;
                    }
                }
                else
                {
                    CastSkill(zoneStart, entityList);
                }
                break;

            case UsedWeaponType.Bow:
                if (oldWeapon != usedWeapon)
                {
                    rangeIndicator.Init(SkillShape.RADIAL, 360.0f);
                    oldWeapon = usedWeapon;
                }
                if (entityTarget == null)
                {
                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit2.point.x, zoneStart.position.y, hit2.point.z);
                        zoneStart.LookAt(lookAt);
                    }

                    DrawRangeIndicator(zoneStart, shape, bowRange, 360.0f);

                    SelectTargetRay(zoneStart, ref entityTarget, true);
                }
                else
                {
                    // Did we accidentally try shoot ourselves
                    if (entityTarget != caster)
                    {
                        CastSkill(zoneStart, entityList);
                    }
                    else
                    {
                        entityTarget = null;
                    }
                }
                break;

            default:
                break;
        }
    }

    protected override void CastSkill(Transform zoneStart, List<Entity> entityList)
    {
        currentlyCasting = true;

        float drawPercent = (timeSpentOnWindUp / windUp);

        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                DrawRangeIndicator(zoneStart, SkillShape.RADIAL, unarmedRange, 360.0f);

                rangeIndicator.DrawCastTimeIndicator(zoneStart, 360.0f, 0.0f, unarmedRange, drawPercent);
                break;

            case UsedWeaponType.Sword:
                DrawRangeIndicator(zoneStart, SkillShape.RADIAL, swordRange, angleWidth);

                rangeIndicator.DrawCastTimeIndicator(zoneStart, angleWidth, 0.0f, swordRange, drawPercent);
                break;

            case UsedWeaponType.Staff:
                DrawRangeIndicator(zoneStart, SkillShape.LINE, staffRange, lineWidth);

                rangeIndicator.DrawCastTimeIndicator(zoneStart, lineWidth, 0.0f, staffRange, drawPercent);
                break;

            case UsedWeaponType.Bow:
                DrawRangeIndicator(zoneStart, SkillShape.RADIAL, bowRange, 360.0f);

                rangeIndicator.DrawCastTimeIndicator(zoneStart, 360.0f, 0.0f, bowRange, drawPercent);
                break;
            default:
                break;
        }

        // Increment the time spent winding up the skill
        timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= windUp)
        {
            currentlyCasting = false;
            ActivateSkill(zoneStart, entityList);
            timeSpentOnWindUp = 0.0f;
        }
    }

    protected override void ActivateSkill(Transform zoneStart, List<Entity> entityList)
    {
        timeBeenOnCooldown = 0.0f;

        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                entityTarget.TakeDamage(baseUnarmedDamage);
                break;

            case UsedWeaponType.Sword:
                foreach (Entity testedEntity in entityList)
                {
                    if (CheckRadialSkillHit(testedEntity.transform.position, zoneStart))
                    {
                        testedEntity.TakeDamage(baseSwordDamage);
                    }
                }
                break;

            case UsedWeaponType.Staff:
                foreach (Entity testedEntity in entityList)
                {
                    if (CheckLineSkillHit(testedEntity.transform.position))
                    {
                        testedEntity.TakeDamage(baseStaffDamage);
                    }
                }
                break;

            case UsedWeaponType.Bow:
                entityTarget.TakeDamage(baseBowDamage);
                break;
            default:
                break;
        }

        entityTarget = null;
        attackAreaChosen = false;
    }

    // Needs to be called if they start using a new weapon
    public void WeaponChange(UsedWeaponType newUsedWeapon)
    {
        usedWeapon = newUsedWeapon;

        if (usedWeapon == UsedWeaponType.Staff)
        {
            damageType = DamageType.MAGICAL;
        }
        else
        {
            damageType = DamageType.PHYSICAL;
        }
    }
}
