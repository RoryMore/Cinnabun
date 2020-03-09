using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BasicSkill
{
    // Start is called before the first frame update
    void Start()
    {
        base.Initialise();
    }


    protected override void ApplySkillProplys()
    {

        timeBeenOnCooldown = 0.0f;
        timeSpentOnWindUp = 0.0f;
        skillState = SkillState.INACTIVE;
        currentlyCasting = false;

    }

    protected override void ApplyCastSkillProplys()
    {
        //test enemy to prep to counter
    }
}
