using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCDRTrait : EquipmentTrait
{
    readonly public Trait trait = Trait.SkillCDR;

    private SkillData.SkillList effectedSkill;
    public SkillData.SkillList EffectedSkill { get => effectedSkill; }

    SkillData.SkillList[] possibleSkillsEffected = new SkillData.SkillList[] {  SkillData.SkillList.WEAPONATTACK,
                                                                                SkillData.SkillList.TELEPORT,
                                                                                SkillData.SkillList.REWIND,
                                                                                SkillData.SkillList.DELAYEDBLAST};

    
    public override void Initialise()
    {
        effectedSkill = possibleSkillsEffected[Random.Range(0, possibleSkillsEffected.Length)];

        description = "";
        // TODO: Get proper random magnitude
        magnitude = 0.0f;
    }
}
