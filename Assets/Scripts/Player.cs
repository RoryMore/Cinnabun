using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{
    [Header("Movement Raycasting Settings")]
    public LayerMask groundLayerMask;
    public float moveRaycastDistance;

    public enum PlayerState
    {
        FREE,
        DOINGSKILL
    }

    [Header("State")]
    public PlayerState playerState;

    [Header("Skills & Casting")]
    public WeaponAttack weaponAttack;
    public List<SkillData> skillList;
    [HideInInspector] public SkillData selectedSkill = null;

    public PauseAbility pause = null;
    PauseMenuUI pauseMenu = null;

    [Header("Navigation")]
    public float turningSpeed;
    float baseMovementSpeed;

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
    [SerializeField]
    GameObject inventory;
    bool inventoryBeginShit = false;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        // Using base given stats, get derived stats
        InitialiseAll();
        currentHP = maxHP;
        
        nav = GetComponent<NavMeshAgent>();
        nav.speed = movementSpeed;
        baseMovementSpeed = movementSpeed;

        playerState = PlayerState.FREE;
    }

    private void Awake()
    {
        pause = FindObjectOfType<PauseAbility>();
        pauseMenu = FindObjectOfType<PauseMenuUI>();
        InitialiseSkills();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventoryBeginShit)
        {
            inventoryBeginShit = true;
            inventory.SetActive(false);
        }
        UpdateSkillCooldowns();
        UpdateAllConditions();
        UpdateAnimator();

        //if () // Check if player is dead
        if (!isDead)
        {
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
                                Move();
                            }

                            if (Input.GetKeyDown(KeyCode.I))
                            {
                                if (!inventory.activeSelf)
                                {
                                    inventory.SetActive(true);
                                }
                                else
                                {
                                    inventory.SetActive(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        Move();
                    }

                    // Player can only select a skill to use if they have paused
                    if (pause.states == PauseAbility.GameStates.TIMESTOP)
                    {
                        //Time.timeScale = 0.001f;
                        EvaluateInputForSkillSelection();
                    }
                    break;

                case PlayerState.DOINGSKILL: // Player has selected a skill. Choose where to cast
                                             // Make the player stop moving

                    nav.speed = 0.0f;
                    nav.angularSpeed = 0.0f;

                    AnimatorClipInfo[] animInfo = animator.GetCurrentAnimatorClipInfo(0);
                    AnimationClip currentClip = animInfo[0].clip;
                    float animSpeed = currentClip.length;

                    //TargetSkill();
                    switch (selectedSkill.skill)
                    {
                        // Special skills that need different transform
                        case SkillData.SkillList.DELAYEDBLAST:
                            if (currentEncounter != null)
                            {
                                selectedSkill.TargetSkill(transform, currentEncounter.masterInitiativeList);

                                if (selectedSkill.currentlyCasting)
                                {
                                    if (!delayedBlastCastParticles.activeSelf)
                                    {
                                        delayedBlastCastParticles.SetActive(true);
                                    }
                                    animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.windUp));
                                    animator.SetBool("skillCast", true);
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
                            selectedSkill.TargetSkill(transform);

                            if (selectedSkill.currentlyCasting)
                            {
                                if (!rewindCastParticles.activeSelf)
                                {
                                    rewindCastParticles.SetActive(true);
                                }
                                animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.windUp));
                                animator.SetBool("skillCast", true);
                            }
                            break;

                        case SkillData.SkillList.TELEPORT:
                            selectedSkill.TargetSkill(transform);

                            if (selectedSkill.currentlyCasting)
                            {
                                if (!teleportCastParticles.activeSelf)
                                {
                                    teleportCastParticles.SetActive(true);
                                }
                                animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.windUp));
                                animator.SetBool("skillCast", true);
                            }
                            break;

                        default:
                            if (selectedSkill == weaponAttack)
                            {
                                if (currentEncounter != null)
                                {
                                    // Need a current entity list to put into function parameter
                                    selectedSkill.TargetSkill(transform, currentEncounter.masterInitiativeList);

                                    if (selectedSkill.currentlyCasting)
                                    {
                                        // We are currently casting a skill
                                        // Animate attack animation here

                                        animator.SetFloat("weaponAttackPlaybackMultiplier", (animSpeed / selectedSkill.windUp));
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
                                    animator.SetBool("skillCast", false);

                                    // Deactivate any active cast particles
                                    delayedBlastCastParticles.SetActive(false);
                                    rewindCastParticles.SetActive(false);
                                    teleportCastParticles.SetActive(false);
                                }
                            }
                            else
                            {
                                selectedSkill.TargetSkill(transform);

                                if (selectedSkill.currentlyCasting)
                                {
                                    // We are currently casting a skill
                                    // Animate cast animation here

                                    animator.SetFloat("castingPlaybackMultiplier", (animSpeed / selectedSkill.windUp));
                                    animator.SetBool("skillCast", true);
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
                            animator.SetBool("skillCast", false);

                            // Deactivate any active cast particles
                            delayedBlastCastParticles.SetActive(false);
                            rewindCastParticles.SetActive(false);
                            teleportCastParticles.SetActive(false);
                        }
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        CancelSkillSelection();
                    }

                    break;

                default:
                    break;
            }
        }
    }

    public override void TakeDamage(int amount)
    {
        animator.SetTrigger("gotHit");
        base.TakeDamage(amount);
    }

    void UpdateSkillCooldowns()
    {
        if (weaponAttack != null)
        {
            weaponAttack.ProgressCooldown();
        }
        foreach (SkillData checkedSkill in skillList)
        {
            checkedSkill.ProgressCooldown();
        }
    }

    void Move()
    {
        if (Input.GetMouseButton(0))
        {

            nav.speed = movementSpeed;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, moveRaycastDistance, groundLayerMask))
            {
                //if (hit.collider.tag.Contains("Finish"))
                //{
                nav.SetDestination(hit.point);

                Debug.Log("Move Player");
                //}
            }

        }
    }

    void EvaluateInputForSkillSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //SelectSkill(SkillData.SkillList.TELEPORT);
            //SelectSkill(0);
            if (weaponAttack != null)
            {
                if (weaponAttack.timeBeenOnCooldown >= weaponAttack.cooldown)
                {
                    selectedSkill = weaponAttack;
                    playerState = PlayerState.DOINGSKILL;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSkill(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSkill(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSkill(4);
        }
    }

    public void SelectSkill(SkillData.SkillList skill)
    {
        foreach (SkillData checkedSkill in skillList)
        {
            // Check if the skill is on cooldown
            // Proceed if not on cooldown
            if (checkedSkill.timeBeenOnCooldown >= checkedSkill.cooldown)
            {
                switch (checkedSkill.skill)
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
            SkillData checkedSkill = skillList[skillAtIndex-1];
            if (checkedSkill.timeBeenOnCooldown >= checkedSkill.cooldown)
            {
                 selectedSkill = skillList[skillAtIndex-1];
                 playerState = PlayerState.DOINGSKILL;
            }
            
        }
    }

    void InitialiseSkills()
    {
        weaponAttack.Initialise(this);
        foreach (SkillData checkedSkill in skillList)
        {
            checkedSkill.Initialise();
        }
    }

    public void CancelSkillSelection()
    {
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

    void UpdateAnimator()
    {
        if (nav.velocity.magnitude > 0.01f)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        animator.SetFloat("movementPlaybackMultiplier", nav.velocity.magnitude / movementSpeed);

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

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.forward * 2.0f);
    //}
}
