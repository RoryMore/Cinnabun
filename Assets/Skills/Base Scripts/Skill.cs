using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : BaseSkill
{
    

    protected override void Initialise()
    {
        base.Initialise();
        if (skillData != null)
        {
            // SETTING PROJECTOR SHADER TO USE CIRCULAR IN2OUT FILL MODE
            projector.material.SetInt("_SkillType", 2);

            // SETTING PROJECTOR SHADER TO USER LINEAR BASE2END FILL MODE
            projector.material.SetInt("_SkillType", 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        SkillDeltaUpdate();
    }
}
