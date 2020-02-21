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

    [HideInInspector] public string itemName;

    public ItemSpawner.ItemRarity itemRarity;

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

    public InventoryItem.ItemStatBlock GetRandomItemStats()
    {
        InventoryItem.ItemStatBlock itemStatBlock = new InventoryItem.ItemStatBlock
        {
            strength = Random.Range(strengthRangeMin, strengthRangeMax),
            agility = Random.Range(agilityRangeMin, agilityRangeMax),
            constitution = Random.Range(constitutionRangeMin, constitutionRangeMax),
            intellect = Random.Range(intellectRangeMin, intellectRangeMax),
            physicalArmour = Random.Range(physicalArmourRangeMin, physicalArmourRangeMax),
            magicalArmour = Random.Range(magicalArmourRangeMin, magicalArmourRangeMax)
        };

        return itemStatBlock;
    }

    public InventoryItem.ItemStatBlock GetSetItemStats()
    {
        InventoryItem.ItemStatBlock itemStatBlock = new InventoryItem.ItemStatBlock
        {
            strength = strength,
            agility = agility,
            constitution = constitution,
            intellect = intellect,
            physicalArmour = physicalArmour,
            magicalArmour = magicalArmour
        };

        return itemStatBlock;
    }
}
