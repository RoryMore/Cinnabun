using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{
    public List<SkillData> skillList;
    SkillData selectedSkill = null;

    // Locally used to avoid multiple checks every time we want to check if a skill is being used
    [SerializeField]
    bool playerCasting = false;
    [SerializeField]
    bool skillSelected = false;

    NavMeshAgent navAgent = null;

    ExampleSkill temp;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCasting)
        {
            if (!skillSelected)
            {
                Move();
                EvaluateInputForSkillSelection();
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 200.0f))
                {
                    Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    transform.LookAt(lookAt);

                    temp.DrawRangeIndicator(transform);

                    if (Input.GetMouseButtonDown(0))
                    {
                        temp.CastSkill(transform);
                        playerCasting = true;
                    }
                }
            }
        }
        else
        {
            if (temp.currentlyCasting)
            {
                temp.CastSkill(transform);
            }
            else
            {
                skillSelected = false;
                playerCasting = false;
            }
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
                temp.coneRangeIndicator.Init(temp.angle);
                skillSelected = true;
            }
        }
    }
}
