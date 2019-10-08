using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{
    public enum PlayerState
    {
        FREE,
        DOINGSKILL
    }

    [Header("State")]
    public PlayerState playerState;

    [Header("Skills & Casting")]
    public List<SkillData> skillList;
    [HideInInspector] public SkillData selectedSkill = null;

    PauseAbility pause = null;

    [Header("Navigation")]
    public float turningSpeed;
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
        pause = FindObjectOfType<PauseAbility>();
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

                    // Player can only select a skill to use if they have paused
                    if (pause.states == PauseAbility.GameStates.TIMESTOP)
                    {
                        //Time.timeScale = 0.001f;
                        EvaluateInputForSkillSelection();
                    }
                    break;

                case PlayerState.DOINGSKILL: // Player has selected a skill. Choose where to cast
                                                // Make the player stop moving
                    
                    navAgent.speed = 0.0f;
                    navAgent.angularSpeed = 0.0f;
                    //TargetSkill();
                    switch (selectedSkill.skill)
                    {
                        // Special skills that need different transform

                        default:
                            selectedSkill.TargetSkill(transform);
                            
                            break;
                    }
                    // skill has ended and been fully cast
                    if (selectedSkill.timeBeenOnCooldown == 0.0f && !selectedSkill.currentlyCasting)
                    {
                        navAgent.angularSpeed = turningSpeed;
                        pause.actionsLeft--;
                        selectedSkill = null;
                        playerState = PlayerState.FREE;
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
            //SelectSkill(SkillData.SkillList.TELEPORT);
            SelectSkill(0);
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
                        playerState = PlayerState.DOINGSKILL;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void SelectSkill(int skillAtIndex)
    {
        if (skillAtIndex < skillList.Count)
        {
            selectedSkill = skillList[skillAtIndex];
            playerState = PlayerState.DOINGSKILL;
        }
    }

    //void TargetSkill()
    //{
    //    if (selectedSkill != null)
    //    {
    //        switch (selectedSkill.skill)
    //        {
    //            case SkillData.SkillList.TELEPORT:
    //                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
    //                {
    //                    Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
    //                    transform.LookAt(lookAt);

    //                    selectedSkill.DrawRangeIndicator(transform, selectedSkill.shape);

    //                    if (Input.GetMouseButtonDown(0))
    //                    {
    //                        selectedSkill.currentlyCasting = true;
    //                        playerState = PlayerState.SKILLCASTING;

    //                        Debug.Log("Player started Casting a skill");
    //                    }
    //                }
    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("While player is attempting to target skill; selectedSkill is null");
    //    }
    //}

    //void CastSelectedSkill()
    //{
    //    if (selectedSkill != null)
    //    {
    //        if (selectedSkill.currentlyCasting)
    //        {
    //            Debug.Log("Skill being cast");
    //            switch (selectedSkill.skill)
    //            {
    //                case SkillData.SkillList.TELEPORT:
    //                    selectedSkill.CastSkill(transform, selectedSkill.shape);

    //                    break;

    //                default:
    //                    selectedSkill.CastSkill(transform, selectedSkill.shape);

    //                    break;
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("Skill finished cast");
    //            navAgent.angularSpeed = turningSpeed;

    //            pause.actionsLeft--;
    //            playerState = PlayerState.FREE;
    //            selectedSkill = null;

                
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("While player is attempting to cast selected skill; selectedSkill is null");
    //        playerState = PlayerState.FREE;
    //    }
    //}

    void InitialiseSkills()
    {
        foreach (SkillData checkedSkill in skillList)
        {
            checkedSkill.Initialise();
        }
    }

    public void CancelSkillSelection()
    {
        selectedSkill = null;
        playerState = PlayerState.FREE;
        navAgent.angularSpeed = turningSpeed;
    }
}
