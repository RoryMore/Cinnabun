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
        SKILLCASTING,
        DEAD
    }

    [Header("State")]
    public PlayerState playerState;

    [Header("Skills & Casting")]
    public List<SkillData> skillList;
    [HideInInspector] public SkillData selectedSkill = null;

    // Local unlisted reference
    ExampleSkill temp;

    NavMeshAgent navAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerState = PlayerState.FREE;


    }

    private void Awake()
    {
        InitialiseSkills();
    }

    // Update is called once per frame
    void Update()
    {
        //if () // Check if player is dead. Send them to dead state if so
        switch (playerState)
        {
            case PlayerState.FREE:  // Player can move, and if in combat can receive input for selecting a skill
                Move();
                EvaluateInputForSkillSelection();
                break;

            case PlayerState.SKILLSELECTED: // Player has selected a skill. Choose where to cast
                TargetSkill();
                break;

            case PlayerState.SKILLCASTING:  // Player is casting, skill will activate
                // Make the player stop moving
                CastSelectedSkill();
                break;

            case PlayerState.DEAD:

                break;

            default:
                break;
        }
    }

    void Move()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 200.0f))
            {
                //if (hit.collider.tag.Contains("Ground"))
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
            if (checkedSkill.skill == skill)
            {
                temp = (ExampleSkill)checkedSkill;
                playerState = PlayerState.SKILLSELECTED;
            }
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

                        temp.DrawRangeIndicator(transform);

                        if (Input.GetMouseButtonDown(0))
                        {
                            temp.currentlyCasting = true;
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
                        selectedSkill.CastSkill(transform);
                    }
                    else
                    {
                        playerState = PlayerState.FREE;
                    }
                    break;

                default:
                    if (selectedSkill.currentlyCasting)
                    {
                        selectedSkill.CastSkill(transform);
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
}
