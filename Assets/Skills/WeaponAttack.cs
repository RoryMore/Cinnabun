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

    [SerializeField]
    MeshCollider meshCollider;  // MUST BE SET TO A CONVEX MESH FOR ACCURACY

    private void Start()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();
        oldWeapon = UsedWeaponType.NotInitialised;

        meshCollider.sharedMesh = GenerateRectMesh();
    }

    private Mesh GenerateRectMesh()
    {
        float angleLookAt = GetForwardAngle(casterSelf.transform);
        float halfFarWidth = skillData.farWidth * 0.5f;
        float halfNearWidth = skillData.nearWidth * 0.5f;
        Vector3 posCurrentMin, posCurrentMax, posNextMin, posNextMax;
        posCurrentMin = Vector3.zero;// casterSelf.transform.position;
        posCurrentMin.x += skillData.minRange;
        posCurrentMin.z -= halfNearWidth;        posCurrentMin.y += skillData.verticalRange * 0.5f;
        posCurrentMax = Vector3.zero;// casterSelf.transform.position;
        posCurrentMax.x += skillData.maxRange;
        posCurrentMax.z -= halfFarWidth;        posCurrentMax.y += skillData.verticalRange * 0.5f;
        posNextMin = Vector3.zero; //casterSelf.transform.position;
        posNextMin.x += skillData.minRange;
        posNextMin.z += halfNearWidth;        posNextMin.y += skillData.verticalRange * 0.5f;
        posNextMax = Vector3.zero; //casterSelf.transform.position;
        posNextMax.z += halfFarWidth;
        posNextMax.x += skillData.maxRange;        posNextMax.y += skillData.verticalRange * 0.5f;
        Vector3[] hitCheckBounds = new Vector3[8];
        hitCheckBounds[0] = posCurrentMin;
        hitCheckBounds[1] = posCurrentMax;
        hitCheckBounds[2] = posNextMax;
        hitCheckBounds[3] = posNextMin;        posCurrentMin.y -= skillData.verticalRange;        posCurrentMax.y -= skillData.verticalRange;        posNextMax.y -= skillData.verticalRange;        posNextMin.y -= skillData.verticalRange;        hitCheckBounds[4] = posNextMin;        hitCheckBounds[5] = posNextMax;        hitCheckBounds[6] = posCurrentMax;        hitCheckBounds[7] = posCurrentMin;
        Quaternion qAngle = Quaternion.AngleAxis(angleLookAt - 90.0f, Vector3.up);
        for (int i = 0; i < hitCheckBounds.Length; i++)
        {
            //hitCheckBounds[i] -= casterSelf.transform.position;
            hitCheckBounds[i] = qAngle * hitCheckBounds[i];
            //hitCheckBounds[i] += casterSelf.transform.position;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = hitCheckBounds;
        int[] triangles = new int[12 * 3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 0;

        triangles[6] = 0;
        triangles[7] = 3;
        triangles[8] = 4;
        triangles[9] = 4;
        triangles[10] = 7;
        triangles[11] = 0;

        triangles[12] = 0;
        triangles[13] = 7;
        triangles[14] = 6;
        triangles[15] = 6;
        triangles[16] = 1;
        triangles[17] = 0;

        triangles[18] = 7;
        triangles[19] = 6;
        triangles[20] = 5;
        triangles[21] = 5;
        triangles[22] = 4;
        triangles[23] = 7;

        triangles[24] = 6;
        triangles[25] = 1;
        triangles[26] = 2;
        triangles[27] = 2;
        triangles[28] = 5;
        triangles[29] = 6;

        triangles[30] = 3;
        triangles[31] = 4;
        triangles[32] = 5;
        triangles[33] = 5;
        triangles[34] = 2;
        triangles[35] = 3;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
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
                        //if (CheckLineSkillHit(testedEntity.transform.position, skillData.minRange, skillData.maxRange, skillData.nearWidth, skillData.farWidth))
                        //{
                        //    weaponhit = true;
                        //    testedEntity.TakeDamage(skillData.baseMagnitude * swordDamageMultiplier);
                        //}
                        //if (CheckLineSkillHit(testedEntity.transform.position, skillData.minRange, skillData.maxRange, skillData.nearWidth, skillData.farWidth))
                        //{
                        //    weaponhit = true;

                        //    int swordDamage = Mathf.FloorToInt((float)skillData.baseMagnitude * swordDamageMultiplier);
                        //    swordDamage = Mathf.Clamp(swordDamage, 1, 99999);

                        //    testedEntity.TakeDamage(swordDamage + casterSelf.GetStrengthDamageBonus());
                        //}
                        if (CheckTargetInRectCollider(meshCollider, testedEntity.transform.position))
                        {
                            weaponhit = true;

                            int swordDamage = Mathf.FloorToInt((float)skillData.baseMagnitude * swordDamageMultiplier);
                            swordDamage = Mathf.Clamp(swordDamage, 1, 99999);

                            testedEntity.TakeDamage(swordDamage + casterSelf.GetStrengthDamageBonus());
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

    protected bool CheckTargetInRectCollider(MeshCollider meshCollider, Vector3 point)
    {
        // MESH MUST BE SET TO CONCAVE
        Vector3 direction = meshCollider.bounds.center - point;
        Ray ray = new Ray(point, direction);
        if (meshCollider.Raycast(ray, out RaycastHit hit, direction.magnitude))
        {
            // If the raycast hits the collider, the point is outside the collider
            return false;
        }
        //Debug.Log("Tested point inside collider");
        return true;
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
