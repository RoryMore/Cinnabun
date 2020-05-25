using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skills/DelayedBlast", order = 1)]

public class DelayedBlast : BaseSkill
{
    // SUGGESTION: Make the Blast a targetted area.
    Vector3 explosionLocation = Vector3.zero;
    bool explosionLocationSet = false;

    [Header("Blast Explosion")]
    [SerializeField]
    Projector explosionProjector;
    public float explosionRadius;
    public float explosionDamageMultiplier;
    Material explosionMaterial;
    [SerializeField]
    Sprite explosionMainCookie;
    [SerializeField]
    Sprite explosionFillCookie;
	

    CameraController cameraController;

    [SerializeField]
    GameObject explosionParticles;
    Quaternion explosionRotation;

    private void Start()
    {
	
        Initialise();
    }

    public override void DisableProjector()
    {
        base.DisableProjector();
        if (explosionProjector.enabled)
        {
            explosionProjector.enabled = false;
        }
    }

    public override void ResetSkillVars()
    {
        base.ResetSkillVars();
        explosionLocationSet = false;
        explosionLocation = Vector3.zero;
    }

    protected override void Initialise()
    {
        base.Initialise();
        cameraController = FindObjectOfType<CameraController>();

        // Explosion Projector Setup
        explosionMaterial = new Material(Shader.Find("Projector/Tattoo"));

        explosionMaterial.SetTexture("_ShadowTex", explosionMainCookie.texture);
        explosionMaterial.SetTexture("_FillTex", explosionFillCookie.texture);

        explosionMaterial.SetInt("_SkillType", 1);

        explosionProjector.material = explosionMaterial;
        explosionProjector.orthographicSize = explosionRadius;
        explosionProjector.farClipPlane = skillData.verticalRange;
        explosionProjector.nearClipPlane = -skillData.verticalRange;

        // Upgrade Initialisation if they are active
        if (SaveManager.GetUpgradeList().blastExplosionRadius != null)
        {
            explosionRadius += SaveManager.GetUpgradeList().blastExplosionRadius.GetUpgradedMagnitude();
            skillData.maxRange += (SaveManager.GetUpgradeList().blastExplosionRadius.GetUpgradedMagnitude() * 0.5f);

            explosionDamageMultiplier += SaveManager.GetUpgradeList().blastExplosionDamage.GetUpgradedMagnitude();
        }
        
    }

    private void Update()
    {
        SkillDeltaUpdate();
        if (explosionProjector.enabled)
        {
            explosionProjector.material.SetFloat("_Progress", (timeSpentOnWindUp / skillData.windUp) * 0.5f);
        }
        if (explosionParticles.activeSelf)
        {
            explosionParticles.transform.position = explosionLocation;
            explosionParticles.transform.rotation = explosionRotation;
        }
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
					SoundManager.blastsound.Play(0);
                    break;
                }
        }
    }

    protected override void TargetSkill()
    {
        // Entity is not set; therefore we need to wait until the user has set the entity
        if (!explosionLocationSet)
        {
            //ResetIndicatorImages();
            EnableProjector();
            if (!explosionProjector.enabled)
            {
                explosionProjector.enabled = true;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 400, groundMask))
            {
                Vector3 lookAt = new Vector3(hit.point.x, casterSelf.transform.position.y, hit.point.z);
                casterSelf.transform.LookAt(lookAt);

                explosionProjector.transform.position = hit.point;
            }

            // We are drawing the range indicator here so the player knows if what they are clicking is in range
            // If being in range is relevant
            //DrawRangeIndicator(zoneStart, shape);

            // Select our entity target
            explosionLocationSet = SelectTargetRay(ref explosionLocation, groundMask, true);

            // The true value in the SelectTargetRay function is specifying that we want to also make a check
            // to see if the target is in range
            // We can leave that extra field blank, or false, if we don't want to make that check
        }
        else if (explosionLocationSet)
        {
            // Start casting the skill
            skillState = SkillState.CASTING;
            explosionProjector.transform.position = explosionLocation;
            //CastSkill(entityList);
        }

    }

    protected override void CastSkill()
    {
        DisableProjector();
        if (!explosionProjector.enabled)
        {
            explosionProjector.enabled = true;
        }
        //SetFillType(fillType);

        currentlyCasting = true;

        //DrawRangeIndicator(zoneStart, shape);

        //float drawPercent = (timeSpentOnWindUp / skillData.windUp);
        //rangeIndicator.DrawCastTimeIndicator(zoneStart, angle, 0.0f, maxRange, drawPercent);

        // Increment the time spent winding up the skill
        //timeSpentOnWindUp += Time.deltaTime;

        // When the skill can be activated
        if (timeSpentOnWindUp >= GetCalculatedWindUp())
        {
            currentlyCasting = false;

            skillState = SkillState.DOAFFECT;
            //ActivateSkill(entityList);
            if (explosionProjector.enabled)
            {
                explosionProjector.enabled = false;
            }
        }
    }

    protected override void ActivateSkill(List<Entity> entityList)
    {
        timeBeenOnCooldown = 0.0f;
        timeSpentOnWindUp = 0.0f;

        skillState = SkillState.INACTIVE;

        // Whether the hit is a crit or not is rolled independantly for each target
        // Create local bool here which equals casterSelf.CalculateCriticalStrike() and use that result to have the entire skill crit or not. Not independant per target

        // Camera shake effect on explosion
        StartCoroutine(cameraController.cShake(0.5f, 1.5f));

        //entityTarget1.TakeDamage(skillData.baseMagnitude + casterSelf.GetIntellectDamageBonus(), skillData.damageType, casterSelf.CalculateCriticalStrike());
        ParticleExplosion(explosionLocation);

        if (entityList != null)
        {
 
            //Deal splash damage to enemies
            //rangeIndicator.DrawIndicator(entityTarget1.transform, 360, 0, explosionRadius);
            foreach (Entity enemy in entityList)
            {
                if (Vector3.Distance(enemy.transform.position, explosionLocation) < explosionRadius)
                {
                    enemy.TakeDamage(Mathf.RoundToInt((skillData.baseMagnitude + casterSelf.GetIntellectDamageBonus()) * explosionDamageMultiplier), skillData.damageType, casterSelf.CalculateCriticalStrike());
                }
            }

        }
        explosionLocationSet = false;
        //explosionLocation = Vector3.zero;

    }

    public void ParticleExplosion(Vector3 location)
    {
        if (explosionParticles != null)
        {
            explosionParticles.transform.position = location;
            explosionRotation = transform.rotation;

            explosionParticles.SetActive(false);
            explosionParticles.SetActive(true);
        }
    }
}
