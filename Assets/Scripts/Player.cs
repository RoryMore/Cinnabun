using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{
    public enum PlayerState
    {
        FREE,
        SKILLSELECTED,
        SKILLCASTING
    }

    [Header("State")]
    public PlayerState playerState;

    [Header("Skills & Casting")]
    public List<SkillData> skillList;
    [HideInInspector] public SkillData selectedSkill = null;

    PauseAbility pause;

    NavMeshAgent navAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        // Using base given stats, get derived stats
        InitialiseAll();
        currentHP = maxHP;
        
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = movementSpeed;
        playerState = PlayerState.FREE;


    }

    private void Awake()
    {
        InitialiseSkills();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkillCooldowns();
        UpdateAllConditions();
        //if () // Check if player is dead
        if (!isDead)
        {
            switch (playerState)
            {
                case PlayerState.FREE:  // Player can move, and if in combat can receive input for selecting a skill
                    Move();

                    if (pause.states == PauseAbility.GameStates.TIMESTOP)
                    {
                        EvaluateInputForSkillSelection();
                    }
                    break;

                case PlayerState.SKILLSELECTED: // Player has selected a skill. Choose where to cast
                                                // Make the player stop moving
                    
                    if (Input.GetMouseButtonDown(1))
                    {
                        CancelSkillSelection();
                    }

                    navAgent.speed = 0.0f;
                    TargetSkill();
                    break;

                case PlayerState.SKILLCASTING:  // Player is casting, skill will activate

                    CastSelectedSkill();
                    break;

                default:
                    break;
            }
        }
    }

    void UpdateSkillCooldowns()
    {
        foreach (SkillData checkedSkill in skillList)
        {
            checkedSkill.ProgressCooldown();
        }
    }

    void Move()
    {
        if (Input.GetMouseButton(0))
        {
            navAgent.speed = movementSpeed;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 200.0f))
            {
                //if (hit.collider.tag.Contains("Finish"))
                //{
                    navAgent.SetDestination(hit.point);
                //}
            }

        }
    }

    void EvaluateInputForSkillSelection()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSkill(SkillData.SkillList.TELEPORT);
        }
    }

    void SelectSkill(SkillData.SkillList skill)
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
                        playerState = PlayerState.SKILLSELECTED;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void TargetSkill()
    {
        if (selectedSkill != null)
        {
            switch (selectedSkill.skill)
            {
                case SkillData.SkillList.TELEPORT:
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 200.0f))
                    {
                        Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        transform.LookAt(lookAt);

                        selectedSkill.DrawRangeIndicator(transform, selectedSkill.shape);

                        if (Input.GetMouseButtonDown(0))
                        {
                            selectedSkill.currentlyCasting = true;
                            playerState = PlayerState.SKILLCASTING;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        else
        {
            Debug.Log("While player is attempting to target skill; selectedSkill is null");
        }
    }

    void CastSelectedSkill()
    {
        if (selectedSkill != null)
        {
            switch (selectedSkill.skill)
            {
                case SkillData.SkillList.TELEPORT:
                    if (selectedSkill.currentlyCasting)
                    {
                        selectedSkill.CastSkill(transform, selectedSkill.shape);
                    }
                    else
                    {
                        playerState = PlayerState.FREE;
                    }
                    break;

                default:
                    if (selectedSkill.currentlyCasting)
                    {
                        selectedSkill.CastSkill(transform, selectedSkill.shape);
                    }
                    else
                    {
                        playerState = PlayerState.FREE;
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("While player is attempting to cast selected skill; selectedSkill is null");
            playerState = PlayerState.FREE;
        }
    }

    void InitialiseSkills()
    {
        foreach (SkillData checkedSkill in skillList)
        {
            if (checkedSkill.radialRangeIndicator != null)
            {
                checkedSkill.radialRangeIndicator.Init(checkedSkill.angle);
            }
            if (checkedSkill.rectangleRangeIndicator != null)
            {
                checkedSkill.rectangleRangeIndicator.Init();
            }
        }
    }

    public void CancelSkillSelection()
    {
        selectedSkill = null;
        playerState = PlayerState.FREE;
    }
}
