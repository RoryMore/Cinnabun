using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Weapon,
        Armour
    }

    public enum ArmourType
    {
        Light,
        Medium,
        Heavy
    }

    public enum WeaponType
    {
        Sword,
        Staff,
        Bow
    }

    public enum EquipmentSlot
    {
        Head,
        Shoulders,
        Chest,
        Legs,
        Feet,
        Hands,
        Weapon,
        Ring1,
        Ring2,
        Neck
    }

    public struct IntRange
    {
        public int min;
        public int max;
    }

    [Header("Item Types")]
    [HideInInspector] public ItemType itemType;
    [HideInInspector] public ArmourType armourType;
    [HideInInspector] public WeaponType weaponType;
    [HideInInspector] public EquipmentSlot equipmentSlot;

    [Header("Stat Bonuses")]
    [HideInInspector] public bool applyRandomStats;
    [HideInInspector] public int strength;
    [HideInInspector] public int agility;
    [HideInInspector] public int constitution;
    [HideInInspector] public int intellect;
    [HideInInspector] public int physicalArmour;
    [HideInInspector] public int magicalArmour;

    [Header("Random Range")]
    //[HideInInspector] public IntRange strengthRange;
    //[HideInInspector] public IntRange agilityRange;
    //[HideInInspector] public IntRange constitutionRange;
    //[HideInInspector] public IntRange intellectRange;
    //[HideInInspector] public IntRange physicalArmourRange;
    //[HideInInspector] public IntRange magicalArmourRange;

    [HideInInspector] public int strengthRangeMin;
    [HideInInspector] public int strengthRangeMax;
    [HideInInspector] public int agilityRangeMin;
    [HideInInspector] public int agilityRangeMax;
    [HideInInspector] public int constitutionRangeMin;
    [HideInInspector] public int constitutionRangeMax;
    [HideInInspector] public int intellectRangeMin;
    [HideInInspector] public int intellectRangeMax;
    [HideInInspector] public int physicalArmourRangeMin;
    [HideInInspector] public int physicalArmourRangeMax;
    [HideInInspector] public int magicalArmourRangeMin;
    [HideInInspector] public int magicalArmourRangeMax;

    [Header("Image")]
    [HideInInspector] public Sprite inventorySprite;
    [HideInInspector] public Sprite equippedSprite;

    // TODO: Add editor functionality to define how much inventorySpace we take up
    [Header("Inventory Space")]
    [HideInInspector] public int inventorySpaceX;
    [HideInInspector] public int inventorySpaceY;

    public enum ItemRarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        ULTRA
    }

    public ItemRarity rarity;
    [SerializeField]
    //ItemName itemName;
    string itemName;

    // TODO: Add a section for Tooltip on mouseover.

    // Possible room to add Trait section. List of Traits. Required for Production game
    // Trait randomness/chance needs to be an option as well

    public void RandomiseStatValues()
    {
        strength = Random.Range(strengthRangeMin, strengthRangeMax);
        agility = Random.Range(agilityRangeMin, agilityRangeMax);
        constitution = Random.Range(constitutionRangeMin, constitutionRangeMax);
        intellect = Random.Range(intellectRangeMin, intellectRangeMax);
        physicalArmour = Random.Range(physicalArmourRangeMin, physicalArmourRangeMax);
        magicalArmour = Random.Range(magicalArmourRangeMin, magicalArmourRangeMax);
    }

    public InventoryItem.ItemInfoBlock GetRandomItemStats(float statScalar)
    {
        InventoryItem.ItemInfoBlock itemStatBlock = new InventoryItem.ItemInfoBlock
        {
            strength = Random.Range(strengthRangeMin + Mathf.RoundToInt(statScalar * (strengthRangeMax * 0.75f)), strengthRangeMax + Mathf.RoundToInt(statScalar * (strengthRangeMax * 0.75f))),
            agility = Random.Range(agilityRangeMin + Mathf.RoundToInt(statScalar * agilityRangeMax), agilityRangeMax + Mathf.RoundToInt(statScalar * agilityRangeMax)),
            constitution = Random.Range(constitutionRangeMin + Mathf.RoundToInt(statScalar * constitutionRangeMax), constitutionRangeMax + Mathf.RoundToInt(statScalar * constitutionRangeMax)),
            intellect = Random.Range(intellectRangeMin + Mathf.RoundToInt(statScalar * intellectRangeMax), intellectRangeMax + Mathf.RoundToInt(statScalar * intellectRangeMax)),
            physicalArmour = Random.Range(physicalArmourRangeMin + Mathf.RoundToInt(statScalar * physicalArmourRangeMax), physicalArmourRangeMax + Mathf.RoundToInt(statScalar * physicalArmourRangeMax)),
            magicalArmour = Random.Range(magicalArmourRangeMin + Mathf.RoundToInt(statScalar * magicalArmourRangeMax), magicalArmourRangeMax + Mathf.RoundToInt(statScalar * magicalArmourRangeMax)),

            itemName = itemName,
            rarity = rarity,
            equipmentTrait = new EquipmentTrait()
        };
        itemStatBlock.equipmentTrait = itemStatBlock.equipmentTrait.GetRandomTraitType(itemType);
        //itemStatBlock.equipmentTrait = itemStatBlock.equipmentTrait.GetSpecificTrait(EquipmentTrait.TraitType.SkillPercentDmg);
        itemStatBlock.equipmentTrait.Initialise(rarity);

        return itemStatBlock;
    }

    public InventoryItem.ItemInfoBlock GetSetItemStats()
    {
        InventoryItem.ItemInfoBlock itemStatBlock = new InventoryItem.ItemInfoBlock
        {
            strength = strength,
            agility = agility,
            constitution = constitution,
            intellect = intellect,
            physicalArmour = physicalArmour,
            magicalArmour = magicalArmour,

            itemName = itemName,
            rarity = rarity,
            equipmentTrait = new EquipmentTrait()
        };
        itemStatBlock.equipmentTrait = itemStatBlock.equipmentTrait.GetRandomTraitType(itemType);
        //itemStatBlock.equipmentTrait = itemStatBlock.equipmentTrait.GetSpecificTrait(EquipmentTrait.TraitType.SkillWUR);
        itemStatBlock.equipmentTrait.Initialise(rarity);

        return itemStatBlock;
    }
}
