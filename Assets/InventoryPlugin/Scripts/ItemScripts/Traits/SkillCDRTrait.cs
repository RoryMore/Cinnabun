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

    
    public override void Initialise(ItemData.ItemRarity rarity)
    {
        trait = TraitType.SkillCDR;

        effectedSkill = possibleSkillsEffected[Random.Range(0, possibleSkillsEffected.Length)];

        //switch (rarity)
        //{
        //    case ItemData.ItemRarity.COMMON:
        //        {
        //            magnitude = Random.Range(0.01f, 0.15f);
        //            break;
        //        }
        //    case ItemData.ItemRarity.UNCOMMON:
        //        {
        //            magnitude = Random.Range(0.05f, 0.25f);
        //            break;
        //        }
        //    case ItemData.ItemRarity.RARE:
        //        {
        //            magnitude = Random.Range(0.15f, 0.40f);
        //            break;
        //        }
        //    case ItemData.ItemRarity.ULTRA:
        //        {
        //            magnitude = Random.Range(0.30f, 0.70f);
        //            break;
        //        }
        //}
        magnitude = Random.Range(0.05f, 0.40f);
        magnitude = Mathf.Round(magnitude * 100.0f) / 100.0f;

        description = "";
        switch (effectedSkill)
        {
            case SkillData.SkillList.TELEPORT:
                {
                    description = "your cooldown for Teleport is <color=cyan>" + (magnitude * 100.0f) + "%</color> shorter";
                    break;
                }
            case SkillData.SkillList.REWIND:
                {
                    description = "your cooldown for Rewind is <color=cyan>" + (magnitude * 100.0f) + "%</color> shorter";
                    break;
                }
            case SkillData.SkillList.DELAYEDBLAST:
                {
                    description = "your cooldown for Blast is <color=cyan>" + (magnitude * 100.0f) + "%</color> shorter";
                    break;
                }
        }

        //Debug.Log("SkillCDRTrait Initialised");
    }

    public override void OnEquip()
    {
        Player player = Object.FindObjectOfType<Player>();
        //Debug.Log("SkillCDR EquipmentTrait OnEquip");
        if (player != null)
        {
            //Debug.Log("EquipmentTrait OnEquip Player is NOT null");
            foreach (BaseSkill skill in player.skillList)
            {
                //Debug.Log("Trait OnEquip checking skill");
                if (skill.skillData.skill == effectedSkill)
                {
                    skill.cooldownReduction += magnitude;
                    //Debug.Log("Skill CDR increased");
                }
            }
        }
    }

    public override void OnRemove()
    {
        Player player = Object.FindObjectOfType<Player>();
        //Debug.Log("EquipmentTrait OnRemove");
        if (player != null)
        {
            //Debug.Log("EquipmentTrait OnRemove Player is NOT null");
            foreach (BaseSkill skill in player.skillList)
            {
                //Debug.Log("Trait OnRemove checking skill");
                if (skill.skillData.skill == effectedSkill)
                {
                    skill.cooldownReduction -= magnitude;
                    //Debug.Log("Skill CDR decreased");
                }
            }
        }
    }
}
