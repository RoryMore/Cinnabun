using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{
    [Header("Movement Raycasting Settings")]
    public LayerMask groundLayerMask;
    public float moveRaycastDistance;
    public LayerMask itemInteractLayerMask;
    public CameraController cameraShake;

	public bool attackSkill;
	public bool telepotSkill;
	public bool bombSkill;
	public bool rewindSkill;
	public bool tutorialDone;

	public bool playBlastSound = false;

	TextSystem textSystem;
    [HideInInspector] public bool triggerBox = false;
    public enum PlayerState
    {
        FREE,
        DOINGSKILL
    }

    [Header("State")]
    public PlayerState playerState;

    [Header("Skills & Casting")]
    public WeaponAttack weaponAttack;
    public List<BaseSkill> skillList;
    //[HideInInspector] 
    public BaseSkill selectedSkill = null;

    public PauseAbility pause = null;
    PauseMenuUI pauseMenu = null;

    [Header("Navigation")]
    public float turningSpeed;

    [Header("Animation")]
    public Animator animator;

    [Header("Skill VFX")]
    [SerializeField]
    GameObject teleportCastParticles;
    [SerializeField]
    GameObject rewindCastParticles;
    [SerializeField]
    GameObject delayedBlastCastParticles;

    [Header("Inventory")]
    //[SerializeField]
    public GameObject inventory;
    bool inventoryBeginShit = false;

	public bool WaterSounds = false;
	public bool BirdSounds = false;
	public bool checkInventory = false;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;

        if (SaveManager.GetUpgradeList().playerMovespeed != null)
        {
            baseMovementSpeed += SaveManager.GetUpgradeList().playerMovespeed.GetUpgradedMagnitude();
        }

        // Using base given stats, get derived stats
        InitialiseAll();
        currentHP = maxHP;
        
        nav = GetComponent<NavMeshAgent>();
        nav.speed = movementSpeed;
        //baseMovementSpeed = movementSpeed;

        playerState = PlayerState.FREE;

		telepotSkill = false;
		attackSkill = false;
		bombSkill = false;
		rewindSkill = false;
    }

    private void Awake()
    {
        pause = FindObjectOfType<PauseAbility>();
        pauseMenu = FindObjectOfType<PauseMenuUI>();
        textSystem = FindObjectOfType<TextSystem>();
        //InitialiseSkills();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventoryBeginShit)
        {
            inventoryBeginShit = true;
            inventory.SetActive(false);
        }
        //UpdateSkillCooldowns();
        UpdateAllConditions();
        UpdateAnimator();

        //if () // Check if player is dead
        if (!isDead)
        {
            if (!CanAct())
            {
                return;
            }
            switch (playerState)
            {
                case PlayerState.FREE:  // Player can move, and if in combat can receive input for selecting a skill
                    // If the game is paused. Player destination remains unchanged
                    if (pauseMenu != null)
                    {
                        if (!pauseMenu.isPaused)
                        {
                            if (!inventory.activeSelf)
                            {
                                if ((nav.velocity.x != 0) && (nav.velocity.z != 0))
                                {
                                    IntendedAction(Action.Move);
                                }
                                Move();
                            }

                            if (Input.GetKeyDown(SaveManager.GetSettings().keybindings.toggleInventory))
                            {
                                if (pause.states == PauseAbility.GameStates.PLAY)
                                {
									if (textSystem.novelActive == true)
									{
										if (!inventory.activeSelf)
										{

											inventory.SetActive(true);

										}
										else
										{

											inventory.SetActive(false);
											checkInventory = true;
										}
									}
                                }
                                else
                                {
                                    if (inventory.activeSelf)
                                    {
                                        
                                        inventory.SetActive(false);
										checkInventory = true;
									}
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((nav.velocity.x != 0) && (nav.velocity.z != 0))
                        {
                            IntendedAction(Action.Move);
                        }
                        Move();
                    }

                    if (pause != null)
                    {
                        // Player can only select a skill to use if they have paused
                        if (pause.states == PauseAbility.GameStates.TIMESTOP)
                        {
                            //Time.timeScale = 0.001f;
                            EvaluateInputForSkillSelection();
                        }
                    }
                    break;

                case PlayerState.DOINGSKILL: // Player has selected a skill. Choose where to cast
                                             // Make the player stop moving

                    if (!pauseMenu.isPaused)
                    {
                        
                       
                        nav.speed = 0.0f;
                        nav.angularSpeed = 0.0f;

                        AnimatorClipInfo[] animInfo = animator.GetCurrentAnimatorClipInfo(0);
                        AnimationClip currentClip = animInfo[0].clip;
                        float animSpeed = currentClip.length;

                        //TargetSkill();
                        switch (selectedSkill.skillData.skill)
                        {
                            // Special skills that need different transform
                            case SkillData.SkillList.DELAYEDBLAST:
                                if (currentEncounter != null)
                                {
                                    selectedSkill.TriggerSkill(currentEncounter.playerInclusiveInitiativeList);
                                    IntendedAction(Action.Skill);

                                    if (selectedSkill.currentlyCasting)
                                    {
                                        if (!delayedBlastCastParticles.activeSelf)
                                        {
                                            delayedBlastCastParticles.SetActive(true);
											
										}
                                        animator.SetFloat("castingPlaybackMultiplier", (animSpeed / (selectedSkill.skillData.windUp * 1.7f)));
                                        animator.SetBool("blastCast", true);
										
									}
                                }
                                else
                                {
									
									nav.angularSpeed = turningSpeed;

                                    selectedSkill = null;
                                    playerState = PlayerState.FREE;

                                    // Reset animator variables
                                    animator.SetBool("weaponAttack", false);
                                    animator.SetBool("skillCast", false);

                                    // Deactivate any active cast particles
									
                                    delayedBlastCastParticles.SetActive(false);
                                    rewindCastParticles.SetActive(false);
                                    teleportCastParticles.SetActive(false);

									
								}
                                break;

                            case SkillData.SkillList.REWIND:
                                selectedSkill.TriggerSkill();
                                IntendedAction(Action.Skill);

                                if (selectedSkill.currentlyCasting)
                                {
                                    if (!rewindCastParticles.activeSelf)
                                    {
                                        rewindCastParticles.SetActive(true);
                                    }
                                    animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.skillData.windUp));
                                    animator.SetBool("skillCast", true);
                                }
                                break;

                            case SkillData.SkillList.TELEPORT:
                                selectedSkill.TriggerSkill();
                                IntendedAction(Action.Skill);

                                if (selectedSkill.currentlyCasting)
                                {
                                    if (!teleportCastParticles.activeSelf)
                                    {
                                        teleportCastParticles.SetActive(true);
                                    }
                                    animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.skillData.windUp));
                                    animator.SetBool("teleportCast", true);
                                }
                                break;

                            default:
                                if (selectedSkill == weaponAttack)
                                {
                                    if (currentEncounter != null)
                                    {
                                        // Need a current entity list to put into function parameter
                                        selectedSkill.TriggerSkill(currentEncounter.masterInitiativeList, groundLayerMask);
                                        IntendedAction(Action.BasicAttack);
                                        if (selectedSkill.currentlyCasting)
                                        {
                                            // We are currently casting a skill
                                            // Animate attack animation here

                                            animator.SetFloat("weaponAttackPlaybackMultiplier", (animSpeed / (selectedSkill.skillData.windUp * 1.0f)));
                                            animator.SetBool("weaponAttack", true);
                                        }
                                    }
                                    else
                                    {
                                        nav.angularSpeed = turningSpeed;

                                        selectedSkill = null;
                                        playerState = PlayerState.FREE;

                                        // Reset animator variables
                                        animator.SetBool("weaponAttack", false);
                                        animator.SetBool("blastCast", false);
                                        animator.SetBool("teleportCast", false);

                                        // Deactivate any active cast particles
                                        delayedBlastCastParticles.SetActive(false);
                                        rewindCastParticles.SetActive(false);
                                        teleportCastParticles.SetActive(false);
                                    }
                                }
                                else
                                {
                                    selectedSkill.TriggerSkill();

                                    if (selectedSkill.currentlyCasting)
                                    {
                                        // We are currently casting a skill
                                        // Animate cast animation here

                                        animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.skillData.windUp));
                                        animator.SetBool("blastCast", true);
                                    }
                                }

                                break;
                        }
                        if (selectedSkill != null)
                        {
                            // skill has ended and been fully cast
                            if (selectedSkill.timeBeenOnCooldown == 0.0f && !selectedSkill.currentlyCasting)
                            {
                                nav.angularSpeed = turningSpeed;
                                pause.actionsLeft--;
                                selectedSkill = null;
                                playerState = PlayerState.FREE;

                                // Reset animator variables
                                animator.SetBool("weaponAttack", false);
                                animator.SetBool("teleportCast", false);
                                animator.SetBool("blastCast", false);

                                // Deactivate any active cast particles
                                delayedBlastCastParticles.SetActive(false);
                                rewindCastParticles.SetActive(false);
                                teleportCastParticles.SetActive(false);
                            }
                        }

                        if (Input.GetMouseButtonDown(1) && !selectedSkill.currentlyCasting)
                        {
                            CancelSkillSelection();
                        }
                    }

                    break;

                default:
                    break;
            }
        }
    }

    public override void TakeDamage(int amount, SkillData.DamageType damageType, bool isCrit)
    {
        StartCoroutine(cameraShake.cShake(.3f, 1f));

        base.TakeDamage(amount, damageType, isCrit);

        animator.SetTrigger("gotHit");
        ParticleHit();
    }

    public override void TakeDamage(int amount)
    {
        StartCoroutine(cameraShake.cShake(.3f, 1f));

        //Vector3 popUpSpawn = new Vector3(Random.Range(-0.9f, 0.3f), Random.Range(-0.9f, 0.3f) + 3, 0);

        //DamagePopUp damagePopUpNumber = Instantiate(damageNumber, transform.position + popUpSpawn, Quaternion.identity).GetComponent<DamagePopUp>();
        //damagePopUpNumber.SetUp(amount, false);

        base.TakeDamage(amount);

        animator.SetTrigger("gotHit");
        ParticleHit();
    }

    //void UpdateSkillCooldowns()
    //{
    //if (weaponAttack != null)
    //{
    //weaponAttack.ProgressCooldown();
    //}
    // foreach (SkillData checkedSkill in skillList)
    //{
    //checkedSkill.ProgressCooldown();
    //}
    //}

    void Move()
    {
        if (textSystem != null)
        {
            if (textSystem.novelActive == true)
            {

                if (Input.GetMouseButton(0))
                {

                    nav.speed = movementSpeed;

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // Set player movement destination based on where they clicked on the ground
                    if (Physics.Raycast(ray, out RaycastHit hit, moveRaycastDistance, groundLayerMask))
                    {
                        nav.SetDestination(hit.point);
                    }
                    // If they clicked on an item, set their destination to be the items location
                    if (Physics.Raycast(ray, out RaycastHit hit2, moveRaycastDistance, itemInteractLayerMask))
                    {
                        if (hit2.collider.tag.Contains("Item"))
                        {
                            nav.SetDestination(hit2.collider.transform.position);
                        }
                    }
                    // Note: This is done with two Raycasts due to the fact that we want to use the ground layer for movement and items have an item layer for more interactions
                }
            }
        }
    }

    void EvaluateInputForSkillSelection()
    {
        if (Input.GetKeyDown(SaveManager.GetSettings().keybindings.weaponAttack))
        {
            if (weaponAttack != null)
            {
				if (attackSkill == true)
				{
					if (weaponAttack.isAllowedToCast)
					{
						selectedSkill = weaponAttack;
						playerState = PlayerState.DOINGSKILL;
					}
				}
            }
        }
        else if (Input.GetKeyDown(SaveManager.GetSettings().keybindings.skillSlot2))
        {
			if (bombSkill == true)
			{
				SelectSkill(1);
			}
        }
        else if (Input.GetKeyDown(SaveManager.GetSettings().keybindings.skillSlot3))
        {
			if (telepotSkill == true)
			{
				SelectSkill(2);
			}
        }
        else if (Input.GetKeyDown(SaveManager.GetSettings().keybindings.skillSlot4))
        {
			if (rewindSkill == true)
			{
				SelectSkill(3);
			}
        }
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    SelectSkill(4);
        //}
    }

    public void SelectSkill(SkillData.SkillList skill)
    {
        foreach (BaseSkill checkedSkill in skillList)
        {
            // Check if the skill is on cooldown
            // Proceed if not on cooldown
            //if (checkedSkill.timeBeenOnCooldown >= checkedSkill.skillData.cooldown)
            if (checkedSkill.isAllowedToCast)
            {
                switch (checkedSkill.skillData.skill)
                {
                    case SkillData.SkillList.TELEPORT:
                        selectedSkill = checkedSkill;
                        playerState = PlayerState.DOINGSKILL;
                        break;

                    case SkillData.SkillList.DELAYEDBLAST:
                        selectedSkill = checkedSkill;
                        playerState = PlayerState.DOINGSKILL;
                        break;

                    case SkillData.SkillList.REWIND:
                        selectedSkill = checkedSkill;
                        playerState = PlayerState.DOINGSKILL;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void SelectSkill(int skillAtIndex)
    {
        // Are we allowed to access this index
        if (skillAtIndex-1 < skillList.Count)
        {
            // Is this skill on cooldown
            BaseSkill checkedSkill = skillList[skillAtIndex-1];
            //if (checkedSkill.timeBeenOnCooldown >= checkedSkill.cooldown)
            if (checkedSkill.isAllowedToCast)
            {
                 selectedSkill = skillList[skillAtIndex-1];
                 playerState = PlayerState.DOINGSKILL;
            }
            
        }
    }

    //void InitialiseSkills()
    //{
        //weaponAttack.Initialise(this);
        //foreach (SkillData checkedSkill in skillList)
        //{
            //checkedSkill.Initialise();
        //}
    //}

    public void CancelSkillSelection()
    {
        selectedSkill.DisableProjector();
        selectedSkill.ResetSkillVars();
        selectedSkill = null;
        playerState = PlayerState.FREE;
        nav.angularSpeed = turningSpeed;
    }

    //OVERLOADS
    public override void Death()
    {
        isDead = true;
        animator.SetBool("isDead", isDead);
        nav.destination = transform.position;

        if (selectedSkill != null)
        {
            selectedSkill.DisableProjector();
            selectedSkill.ResetSkillVars();
        }
        //selectedSkill = null;

        if (inventory.activeSelf)
        {
            inventory.SetActive(false);
        }
    }

    public void Revive()
    {
        currentHP = maxHP;
        isDead = false;
        animator.SetBool("isDead", isDead);
        nav.destination = transform.position;
    }

    public void ChangeWeapon(WeaponAttack.UsedWeaponType newUsedWeaponType)
    {
        if (weaponAttack != null)
        {
            weaponAttack.WeaponChange(newUsedWeaponType);
        }
    }

    public override bool CalculateCriticalStrike()
    {
        float agilityEffectiveness = 0.1f;
        agilityEffectiveness += SaveManager.GetUpgradeList().bonusAgilityCrit.GetUpgradedMagnitude();
        float agilityPointThreshold = 25.0f;

        // For every [agilityPointThreshold] points of agility, we gain [agilityEffectiveness * 100]% crit strike
        float result = agilityEffectiveness * (agility / agilityPointThreshold);

        if (Random.Range(0.0f, 1.0f) <= result)
        {
            return true;
        }

        return false;
    }

    protected override int DamageNegated(int originalDamage, SkillData.DamageType damageType)
    {
        // How effective armour is at 'armourPointThreshold' points of armour
        // 0.25f effectiveness && 100.0f threshold = 25% damage reduction at 100 points of armour
        float armourEffectiveness = 0.25f;
        Debug.LogWarning(SaveManager.GetUpgradeList().armourEffectiveness);
        if (SaveManager.GetUpgradeList().armourEffectiveness != null)
        {
            armourEffectiveness += SaveManager.GetUpgradeList().armourEffectiveness.GetUpgradedMagnitude();
        }
        float armourPointThreshold = 100.0f;

        if (damageType == SkillData.DamageType.PHYSICAL)
        {
            float percentReduced = Mathf.Clamp(armourEffectiveness * (physicalArmour / armourPointThreshold), 0, 0.95f);
            return Mathf.RoundToInt(originalDamage * percentReduced);
        }
        else
        {
            float percentReduced = Mathf.Clamp(armourEffectiveness * (magicalArmour / armourPointThreshold), 0, 0.95f);
            return Mathf.RoundToInt(originalDamage * percentReduced);
        }
    }

    void UpdateAnimator()
    {
        if (nav.velocity.magnitude > 0.5f)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        float movePlaybackSpeed = movementSpeed / baseMovementSpeed;
        animator.SetFloat("movementPlaybackMultiplier", movePlaybackSpeed);

        // This is a method to grab clip info to play with animation properties based on current clip
        //AnimatorClipInfo[] animInfo = animator.GetCurrentAnimatorClipInfo(0);
        //AnimationClip currentClip = animInfo[0].clip;
        //Debug.Log(currentClip.name);
    }

    void ResetAnimationVariables()
    {
        animator.SetBool("weaponAttack", false);
        animator.SetBool("skillCast", false);
    }

    public void SelectWeaponAttack()
    {
        selectedSkill = weaponAttack;
        playerState = PlayerState.DOINGSKILL;
    }

    public void SelectBlastAttack()
    {
        SelectSkill(1);
    }
    public void SelectTeleport()
    {
        SelectSkill(2);
    }
    public void SelectRewind()
    {
        SelectSkill(3);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.forward * 2.0f);
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (triggerBox == false)
        {
            if (other.tag == "TriggerBox")
            {
                //Debug.Log("I walked through it");
                triggerBox = true;
            }
        }

		if (other.tag == "Water")
		{
			//Debug.Log("I walked through it");
			WaterSounds = true;
			
		}

		if (other.tag == "Forest")
		{
			//Debug.Log("I walked through it");
			BirdSounds = true;

		}

		if (other.tag == "TutorialOver")
		{
			tutorialDone = true;
		}
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TriggerBox")
        {
            Destroy(other);
        }
    }
}
