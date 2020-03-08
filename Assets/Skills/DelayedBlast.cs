using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skills/DelayedBlast", order = 1)]

public class DelayedBlast : BaseSkill
{
    // SUGGESTION: Make the Blast a targetted area.
    Entity entityTarget1 = null;
    
    [Header("Blast Explosion")]
    public float explosionRadius;
    public float explosionDamageMultiplier;

    private void Start()
    {
        Initialise();
    }

    protected override void Initialise()
    {
        base.Initialise();

        if (SaveManager.GetUpgradeList().blastExplosionRadius != null)
        {
            explosionRadius += SaveManager.GetUpgradeList().blastExplosionRadius.GetUpgradedMagnitude();
        }
    }

    private void Update()
    {
        SkillDeltaUpdate();
    }

    public override void TriggerSkill(List<Entity> entityList)
    {
        base.TriggerSkill();
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
                    TargetSkill();
                    break;
                }

            case SkillState.CASTING:
                {
                    //Debug.Log("Skill being cast!");
                    //UpdateCastTime();
                    CastSkill();
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

    protected override void TargetSkill()
    {
        // Entity is not set; therefore we need to wait until the user has set the entity
        if (entityTarget1 == null)
        {
            //ResetIndicatorImages();
            EnableProjector();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400, groundMask))
            {
                Vector3 lookAt = new Vector3(hit.point.x, casterSelf.transform.position.y, hit.point.z);
                casterSelf.transform.LookAt(lookAt);
            }

            // We are drawing the range indicator here so the player knows if what they are clicking is in range
            // If being in range is relevant
            //DrawRangeIndicator(zoneStart, shape);

            // Select our entity target
            SelectTargetRay(ref entityTarget1, true);
            // The true value in the SelectTargetRay function is specifying that we want to also make a check
            // to see if the target is in range
            // We can leave that extra field blank, or false, if we don't want to make that check
        }
        else if (entityTarget1 != null)
        {
            // Start casting the skill
            skillState = SkillState.CASTING;
            //CastSkill(entityList);
        }

    }

    protected override void CastSkill()
    {
        //SetFillType(fillType);

        currentlyCasting = true;

        //DrawRangeIndicator(zoneStart, shape);

        //float drawPercent = (timeSpentOnWindUp / skillData.windUp);
        //rangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, maxRange, drawPercent);

        // Increment the time spent winding up the skill
        //timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= skillData.windUp)
        {
            currentlyCasting = false;

            skillState = SkillState.DOAFFECT;
            //ActivateSkill(entityList);

            DisableProjector();
        }
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        timeBeenOnCooldown = 0.0f;
        timeSpentOnWindUp = 0.0f;

        skillState = SkillState.INACTIVE;

        entityTarget1.TakeDamage(skillData.baseMagnitude + casterSelf.GetIntellectDamageBonus());
        entityTarget1.ParticleExplosion();

        if (entityList != null)
        {
 
            //Deal splash damage to enemies
            //rangeIndicator.DrawIndicator(entityTarget1.transform, 360, 0, explosionRadius);
            foreach (Entity enemy in entityList)
            {
                //Make sure we don't deal double damage
                if (enemy == entityTarget1)
                {
                    continue;
                }
                if (Vector3.Distance(enemy.transform.position, entityTarget1.transform.position) < explosionRadius)
                {
                    enemy.TakeDamage(Mathf.RoundToInt((skillData.baseMagnitude + casterSelf.GetIntellectDamageBonus()) * explosionDamageMultiplier));
                }
            }

        }
        entityTarget1 = null;

    }
}
