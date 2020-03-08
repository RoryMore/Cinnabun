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
    [Space][Space]
    public UsedWeaponType usedWeapon;
    UsedWeaponType oldWeapon;

    [Header("Overwritten Ranges - USE THESE")]
    public float unarmedRange;
    public float swordRange;
    public float staffRange;
    public float bowRange;

    [Header("Damage Multiplier Based on Weapon")]
    public float unarmedDamageMultiplier;
    public float swordDamageMultiplier;
    public float staffDamageMultiplier;
    public float bowDamageMultiplier;

    [Tooltip("The width the line indicator will use. \nAngleWidth will be what is used for sword")]
    //public float lineWidth;

    Entity entityTarget = null;
    //Entity caster = null;

    bool attackAreaChosen = false;

    [SerializeField]
    MeshCollider meshCollider;  // MUST BE SET TO A CONVEX MESH FOR ACCURACY

    // Sword will use the default existing mainCookie and fillCookie
    [Header("Other Weapon Projector Images")]
    [SerializeField]
    Sprite unarmedCookie;
    [SerializeField]
    Sprite unarmedFillCookie;
    [SerializeField]
    Sprite bowCookie;
    [SerializeField]
    Sprite bowfillCookie;
    [SerializeField]
    Sprite staffCookie;
    [SerializeField]
    Sprite staffFillCookie;

    private void Start()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();
        oldWeapon = UsedWeaponType.NotInitialised;

        meshCollider.sharedMesh = GenerateRectHitboxMesh();
        meshCollider.enabled = false;
    }

    protected void SetIndicatorImages(Sprite mainCookie, Sprite fillCookie)
    {
        projector.material.SetTexture("_ShadowTex", mainCookie.texture);
        projector.material.SetTexture("_FillTex", fillCookie.texture);
    }

    private void Update()
    {
        SkillDeltaUpdate();
    }

    public override void TriggerSkill(List<Entity> entityList, LayerMask layerMask)
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
                    TargetSkill(entityList, layerMask);
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

    protected override void TargetSkill(List<Entity> entityList, LayerMask layerMask)
    {
        
        switch (usedWeapon)
        {
            case UsedWeaponType.Unarmed:
                if (oldWeapon != usedWeapon)
                {
                    oldWeapon = usedWeapon;
                    skillData.maxRange = unarmedRange;
                    SetIndicatorImages(unarmedCookie, unarmedFillCookie);
                    
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
                    SetIndicatorImages(mainCookie, fillCookie);
                }
                if (!attackAreaChosen)
                {
                    //ResetIndicatorImages();
                    EnableProjector();

                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, layerMask))
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
                    SetIndicatorImages(staffCookie, staffFillCookie);
                }
                if (!attackAreaChosen)
                {
                    //ResetIndicatorImages();
                    EnableProjector();

                    Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, Mathf.Infinity, layerMask))
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
                    SetIndicatorImages(bowCookie, bowfillCookie);
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
                int unarmedDamage = Mathf.FloorToInt((skillData.baseMagnitude + casterSelf.GetStrengthDamageBonus()) * unarmedDamageMultiplier);
                //unarmedDamage += casterSelf.GetStrengthDamageBonus();
                unarmedDamage = Mathf.Clamp(unarmedDamage, 1, int.MaxValue);

                entityTarget.TakeDamage(unarmedDamage, SkillData.DamageType.PHYSICAL);

                //SoundManager.meleeSwing.Play();
                break;

            case UsedWeaponType.Sword:
                {
                    meshCollider.enabled = true;
                    bool weaponhit = false;

                    int swordDamage = Mathf.FloorToInt((skillData.baseMagnitude + casterSelf.GetStrengthDamageBonus()) * swordDamageMultiplier);
                    //swordDamage += casterSelf.GetStrengthDamageBonus();
                    swordDamage = Mathf.Clamp(swordDamage, 1, int.MaxValue);

                    foreach (Entity testedEntity in entityList)
                    {
                        if (CheckPointInRectCollider(meshCollider, testedEntity.transform.position))
                        {
                            weaponhit = true;

                            testedEntity.TakeDamage(swordDamage, SkillData.DamageType.PHYSICAL);
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
                int staffDamage = Mathf.FloorToInt((skillData.baseMagnitude + casterSelf.GetIntellectDamageBonus()) * staffDamageMultiplier);
                //staffDamage += casterSelf.GetIntellectDamageBonus();
                staffDamage = Mathf.Clamp(staffDamage, 1, int.MaxValue);

                foreach (Entity testedEntity in entityList)
                {
                    if (CheckLineSkillHit(testedEntity.transform.position, skillData.minRange, skillData.maxRange, skillData.nearWidth, skillData.farWidth))
                    {
                        testedEntity.TakeDamage(staffDamage, SkillData.DamageType.MAGICAL);
                    }
                }
                break;

            case UsedWeaponType.Bow:
                int bowDamage = Mathf.FloorToInt((skillData.baseMagnitude + casterSelf.GetStrengthDamageBonus()) * bowDamageMultiplier);
                //bowDamage += casterSelf.GetStrengthDamageBonus();
                bowDamage = Mathf.Clamp(bowDamage, 1, int.MaxValue);

                entityTarget.TakeDamage(bowDamage, SkillData.DamageType.PHYSICAL);
                break;
            default:
                break;
        }

        entityTarget = null;
        attackAreaChosen = false;
        timeSpentOnWindUp = 0.0f;
        skillState = SkillState.INACTIVE;

        meshCollider.enabled = false;
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
