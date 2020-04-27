using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCDRTrait : EquipmentTrait
{
    private SkillData.SkillList effectedSkill;
    public SkillData.SkillList EffectedSkill { get => effectedSkill; }

    SkillData.SkillList[] possibleSkillsEffected = new SkillData.SkillList[] {  SkillData.SkillList.TELEPORT,
                                                                                SkillData.SkillList.REWIND,
                                                                                SkillData.SkillList.DELAYEDBLAST};

    
    public override void Initialise()
    {
        Debug.Log("SkillCDRTrait Initialised");

        trait = Trait.SkillCDR;

        effectedSkill = possibleSkillsEffected[Random.Range(0, possibleSkillsEffected.Length)];

        // TODO: Get proper random magnitude
        magnitude = Random.Range(0.05f, 1.5f);
        magnitude = Mathf.Round(magnitude * 10.0f) / 10.0f;

        description = "";
        switch (effectedSkill)
        {
            case SkillData.SkillList.TELEPORT:
                {
                    description = "your cooldown for Teleport is <color=cyan>" + magnitude.ToString() + "s</color> shorter";
                    break;
                }
            case SkillData.SkillList.REWIND:
                {
                    description = "your cooldown for Rewind is <color=cyan>" + magnitude.ToString() + "s</color> shorter";
                    break;
                }
            case SkillData.SkillList.DELAYEDBLAST:
                {
                    description = "your cooldown for Blast is <color=cyan>" + magnitude.ToString() + "s</color> shorter";
                    break;
                }
        }
    }

    public override void OnEquip()
    {
        Player player = Object.FindObjectOfType<Player>();
        Debug.Log("EquipmentTrait OnEquip");
        if (player != null)
        {
            Debug.Log("EquipmentTrait OnEquip Player is NOT null");
            foreach (BaseSkill skill in player.skillList)
            {
                Debug.Log("Trait OnEquip checking skill");
                if (skill.skillData.skill == effectedSkill)
                {
                    skill.cooldownReduction += magnitude;
                    Debug.Log("Skill CDR increased");
                }
            }
        }
    }

    public override void OnRemove()
    {
        Player player = Object.FindObjectOfType<Player>();
        Debug.Log("EquipmentTrait OnRemove");
        if (player != null)
        {
            Debug.Log("EquipmentTrait OnRemove Player is NOT null");
            foreach (BaseSkill skill in player.skillList)
            {
                Debug.Log("Trait OnRemove checking skill");
                if (skill.skillData.skill == effectedSkill)
                {
                    skill.cooldownReduction -= magnitude;
                    Debug.Log("Skill CDR decreased");
                }
            }
        }
    }
}
