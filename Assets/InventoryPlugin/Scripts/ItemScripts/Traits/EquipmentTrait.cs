using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentTrait
{
    // The EquipmentTrait class will hold all relevant data and methods related to Traits.
    // Every Item will have a property of type EquipmentTrait which can be used to access its Trait properties.

    // Different Classes will be made for each different Trait, inheriting from this Class.
    // Do the Trait initialisation outside of the Item Class, instead where the item gets created.

    public enum TraitType
    {
        None,
        SkillCDR,
        SkillWUR
    }

    protected TraitType trait;

    protected string description;

    protected float magnitude;

    public TraitType Trait { get => trait;}
    public string Description { get => description; }

    public virtual void Initialise(ItemData.ItemRarity rarity) { }

    public virtual void OnEquip() { }

    public virtual void OnRemove() { }

    public EquipmentTrait GetRandomTraitType(ItemData.ItemType itemType)
    {
        List<TraitType> traitRange = new List<TraitType>();
        switch (itemType)
        {
            case ItemData.ItemType.Weapon:
                {
                    traitRange.AddRange(new TraitType[] { TraitType.None, TraitType.SkillCDR, TraitType.SkillWUR });
                   // traitRange = new Trait[] { Trait.None, Trait.SkillCDR };
                    break;
                }
            case ItemData.ItemType.Armour:
                {
                    traitRange.AddRange(new TraitType[] { TraitType.None, TraitType.SkillCDR, TraitType.SkillWUR });
                    break;
                }
        }
        TraitType randomTraitType = traitRange[Random.Range(0, traitRange.Count - 1)];

        switch(randomTraitType)
        {
            case TraitType.None:
                {
                    description = "this item seems to be mundane";
                    Debug.Log("RandomTrait Type set as None");
                    break;
                }
            case TraitType.SkillCDR:
                {
                    Debug.Log("RandomTrait Type set as SkillCDRTrait");
                    return new SkillCDRTrait();
                }
            case TraitType.SkillWUR:
                {
                    Debug.Log("RandomTrait Type set as SkillCTRTrait");
                    return new SkillWURTrait();
                }
        }
        return this;
    }

    public EquipmentTrait GetSpecificTrait(TraitType traitType)
    {
        switch (traitType)
        {
            case TraitType.SkillCDR:
                {
                    return new SkillCDRTrait();
                }
            case TraitType.SkillWUR:
                {
                    return new SkillWURTrait();
                }
        }
        return this;
    }
}
