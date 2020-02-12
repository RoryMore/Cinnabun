using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : BaseSkill
{

    protected override void Initialise()
    {
        base.Initialise();
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
