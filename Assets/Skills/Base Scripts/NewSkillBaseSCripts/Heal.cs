using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : BasicSkill
{
    float BasicRecoryAmount;
    // Start is called before the first frame update
    void Start()
    {
        base.Initialise();
    }


    protected override void ApplySkillProplys()
    {

        timeBeenOnCooldown = 0.0f + buf.cooldown;
        timeSpentOnWindUp = 0.0f;
        skillState = SkillState.INACTIVE;
        currentlyCasting = false;

    }
}
