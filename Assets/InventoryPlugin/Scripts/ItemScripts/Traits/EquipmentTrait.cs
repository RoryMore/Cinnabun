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

    public enum Trait
    {
        None,
        SkillCDR
    }

    protected Trait trait;

    protected string description;

    protected float magnitude;

    public Trait Trait1 { get => trait;}
    public string Description { get => description; }

    public virtual void Initialise() { }

    public virtual void OnEquip() { }

    public virtual void OnRemove() { }

    public EquipmentTrait GetRandomTraitType()
    {
        Trait[] traitRange = new Trait[] { Trait.None, Trait.SkillCDR };
        Trait randomTraitType = traitRange[Random.Range(0, traitRange.Length)];

        switch(randomTraitType)
        {
            case Trait.None:
                {
                    Debug.Log("RandomTrait Type set as None");
                    break;
                }
            case Trait.SkillCDR:
                {
                    Debug.Log("RandomTrait Type set as SkillCDRTrait");
                    return new SkillCDRTrait();
                }
        }
        return this;
    }
}
