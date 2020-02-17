using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : BaseSkill
{
    public enum UsedWeaponType
    {
        Unarmed,
        Sword,
        Staff,
        Bow,
        NotInitialised
    }
    public UsedWeaponType usedWeapon;
    UsedWeaponType oldWeapon;

    [Header("Overwritten Ranges - USE THESE")]
    public float unarmedRange;
    public float swordRange;
    public float staffRange;
    public float bowRange;

    [Header("Damage Multiplier Based on Weapon")]
    public int unarmedDamageMultiplier;
    public int swordDamageMultiplier;
    public int staffDamageMultiplier;
    public int bowDamageMultiplier;

    [Tooltip("The width the line indicator will use. \nAngleWidth will be what is used for sword")]
    //public float lineWidth;

    Entity entityTarget = null;
    //Entity caster = null;

    bool attackAreaChosen = false;

    private void Start()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();
        oldWeapon = UsedWeaponType.NotInitialised;
    }

    private void Update()
    {
        SkillDeltaUpdate();
    }

    public override void TriggerSkill(List<Entity> entityList)
    {
        base.TriggerSkill(entityList);
        switch (skillState)
        {
            case SkillState.INACTIVE:
                {
                    if (isAllowedToCast)
                    {
                        skillState = SkillState.TARGETTING;
                    }
                    break;
                }

            case SkillState.TARGETTING:
                {
                    //Debug.Log("Skill being Targetted");
                    TargetSkill(entityList);
                    break;
                }

            case SkillState.CASTING:
                {
                    //Debug.Log("Skill being cast!");
                    //UpdateCastTime();
                    CastSkill(entityList);
                    break;
                }

            case SkillState.DOAFFECT:
                {
                    //Debug.Log("Skill Effect Activated");
                    ActivateSkill(entityList);
                    break;
                }
        }
    }

    protected override void TargetSkill(List<Entity> entityList)
    {
        
        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                if (oldWeapon != usedWeapon)
                {
                    oldWeapon = usedWeapon;
                    skillData.maxRange = unarmedRange;
                    
                }
                if (entityTarget == null)
                {
                    //ResetIndicatorImages();
                    EnableProjector();

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit.point.x, casterSelf.transform.position.y, hit.point.z);
                        casterSelf.transform.LookAt(lookAt);
                    }

                    // Select who we're punching
                    SelectTargetRay(ref entityTarget, true);
                }
                else if (entityTarget != null)
                {
                    // We are not hitting ourselves
                    if (entityTarget != casterSelf)
                    {
                        skillState = SkillState.CASTING;
                        //CastSkill(entityList);
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
                    //rangeIndicator.Init(SkillShape.RADIAL, angle);
                    oldWeapon = usedWeapon;
                    skillData.maxRange = swordRange;
                    
                }
                if (!attackAreaChosen)
                {
                    //ResetIndicatorImages();
                    EnableProjector();

                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit2.point.x, casterSelf.transform.position.y, hit2.point.z);
                        casterSelf.transform.LookAt(lookAt);
                    }

                    //DrawRangeIndicator(zoneStart, SkillShape.RADIAL, swordRange, angle);

                    if (Input.GetMouseButtonDown(0))
                    {
                        attackAreaChosen = true;
                    }
                }
                else
                {
                    skillState = SkillState.CASTING;
                    //CastSkill(entityList);
                }
                break;

            case UsedWeaponType.Staff:
                if (oldWeapon != usedWeapon)
                {
                    //rangeIndicator.Init(SkillShape.RECTANGULAR, lineWidth);
                    oldWeapon = usedWeapon;
                    skillData.maxRange = staffRange;
                    
                }
                if (!attackAreaChosen)
                {
                    //ResetIndicatorImages();
                    EnableProjector();

                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit2.point.x, casterSelf.transform.position.y, hit2.point.z);
                        casterSelf.transform.LookAt(lookAt);
                    }

                    //DrawRangeIndicator(zoneStart, SkillShape.RECTANGULAR, staffRange, lineWidth);

                    if (Input.GetMouseButtonDown(0))
                    {
                        attackAreaChosen = true;
                    }
                }
                else
                {
                    skillState = SkillState.CASTING;
                    //CastSkill(entityList);
                }
                break;

            case UsedWeaponType.Bow:
                if (oldWeapon != usedWeapon)
                {
                    //rangeIndicator.Init(SkillShape.RADIAL, 360.0f);
                    oldWeapon = usedWeapon;
                    skillData.maxRange = bowRange;
                    
                }
                if (entityTarget == null)
                {
                    //ResetIndicatorImages();
                    EnableProjector();

                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity))
                    {
                        Vector3 lookAt = new Vector3(hit2.point.x, casterSelf.transform.position.y, hit2.point.z);
                        casterSelf.transform.LookAt(lookAt);
                    }

                    //DrawRangeIndicator(zoneStart, SkillShape.RADIAL, bowRange, 360.0f);

                    SelectTargetRay(ref entityTarget, true);
                }
                else
                {
                    // Did we accidentally try shoot ourselves
                    if (entityTarget != casterSelf)
                    {
                        skillState = SkillState.CASTING;
                        //CastSkill(entityList);
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

    protected override void CastSkill(List<Entity> entityList)
    {
        currentlyCasting = true;

        //float drawPercent = (timeSpentOnWindUp / skillData.windUp);

        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                SetFillType(CastFillType.CIRCULAR);
                break;

            case UsedWeaponType.Sword:
                SetFillType(CastFillType.LINEAR);
                break;

            case UsedWeaponType.Staff:
                SetFillType(CastFillType.LINEAR);
                break;

            case UsedWeaponType.Bow:
                SetFillType(CastFillType.CIRCULAR);
                break;
            default:
                break;
        }

        // When the skill can be activated
        if (timeSpentOnWindUp >= skillData.windUp)
        {
            skillState = SkillState.DOAFFECT;
            //ActivateSkill(entityList);
            currentlyCasting = false;
            //timeSpentOnWindUp = 0.0f;
            DisableProjector();
        }
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        timeBeenOnCooldown = 0.0f;

        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                entityTarget.TakeDamage(skillData.baseMagnitude * unarmedDamageMultiplier);

                //SoundManager.meleeSwing.Play();
                break;

            case UsedWeaponType.Sword:
                {
                    bool weaponhit = false;
                    foreach (Entity testedEntity in entityList)
                    {
                        if (CheckRadialSkillHit(testedEntity.transform.position))
                        {
                            weaponhit = true;
                            testedEntity.TakeDamage(skillData.baseMagnitude * swordDamageMultiplier);
                        }
                    }

                    if (weaponhit)
                    {
                        SoundManager.meleeSwing.Play(0);
                    }
                    //SoundManager.meleeSwing.Play(0);
                    break;
                }

            case UsedWeaponType.Staff:
                foreach (Entity testedEntity in entityList)
                {
                    if (CheckLineSkillHit(testedEntity.transform.position, skillData.minRange, skillData.maxRange, skillData.nearWidth, skillData.farWidth))
                    {
                        testedEntity.TakeDamage(skillData.baseMagnitude * staffDamageMultiplier);
                    }
                }
                break;

            case UsedWeaponType.Bow:
                entityTarget.TakeDamage(skillData.baseMagnitude * bowDamageMultiplier);
                break;
            default:
                break;
        }

        entityTarget = null;
        attackAreaChosen = false;
        timeSpentOnWindUp = 0.0f;
        skillState = SkillState.INACTIVE;
    }

    // Needs to be called if they start using a new weapon
    public void WeaponChange(UsedWeaponType newUsedWeapon)
    {
        usedWeapon = newUsedWeapon;

        if (usedWeapon == UsedWeaponType.Staff)
        {
            skillData.damageType = SkillData.DamageType.MAGICAL;
        }
        else
        {
            skillData.damageType = SkillData.DamageType.PHYSICAL;
        }
    }
}
