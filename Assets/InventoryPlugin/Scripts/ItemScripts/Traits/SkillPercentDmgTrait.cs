using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPercentDmgTrait : EquipmentTrait
{
    public override void Initialise(ItemData.ItemRarity rarity)
    {
        trait = TraitType.SkillPercentDmg;

        magnitude = Random.Range(0.025f, 0.3f);
        // Round magnitude to 2 decimal places
        magnitude = Mathf.Round(magnitude * 100.0f) / 100.0f;

        description = "main Attack <color=orange>dmg -" + (magnitude * 150.0f) + "%</color>, but <color=cyan>+" + (magnitude * 100.0f) + "%</color> of target <color=maroon>maximum HP</color> as extra dmg";
    }

    public override void OnEquip()
    {
        Player player = Object.FindObjectOfType<Player>();
        //Debug.Log("SkillCDR EquipmentTrait OnEquip");
        if (player != null)
        {
            player.weaponAttack.traitPercentDmg += magnitude;
        }
    }

    public override void OnRemove()
    {
        Player player = Object.FindObjectOfType<Player>();
        //Debug.Log("SkillCDR EquipmentTrait OnEquip");
        if (player != null)
        {
            player.weaponAttack.traitPercentDmg -= magnitude;
        }
    }
}
