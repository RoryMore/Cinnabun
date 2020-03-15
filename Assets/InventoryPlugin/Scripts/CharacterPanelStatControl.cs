using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelStatControl : MonoBehaviour
{
    [Header("Editted Text Fields")]
    public Text strengthText;
    public Text agilityText;
    public Text constitutionText;
    public Text intellectText;
    public Text healthText;
    //public Text physicalDamageReductionText;
    //public Text magicalDamageReductionText;
    public Text physicalArmourText;
    public Text magicalArmourText;

    [Header("Literal Names of Entity Variables")]
    public string strength;
    public string agility;
    public string constitution;
    public string intellect;
    public string currentHealth;
    public string maxHealth;
    //public string physicalDamageReduction;
    //public string magicalDamageReduction;
    public string physicalArmour;
    public string magicalArmour;

    static string statStrength;
    static string statAgility;
    static string statConstitution;
    static string statIntellect;
    static string statCurrentHealth;
    static string statMaxHealth;
    //static string statPhysicalDamageReduction;
    //static string statMagicalDamageReduction;
    static string statPhysicalArmour;
    static string statMagicalArmour;

    [Header("Entity to Mirror Stat Values From")]
    public Entity characterEntity;
    static Entity statCharacterEntity;

    // Start is called before the first frame update
    void Start()
    {
        statStrength = strength;
        statAgility = agility;
        statConstitution = constitution;
        statIntellect = intellect;
        statCurrentHealth = currentHealth;
        statMaxHealth = maxHealth;
        //statPhysicalDamageReduction = physicalDamageReduction;
        //statMagicalDamageReduction = magicalDamageReduction;
        statPhysicalArmour = physicalArmour;
        statMagicalArmour = magicalArmour;

        characterEntity = FindObjectOfType<Player>();
        if (characterEntity != null)
        {
            statCharacterEntity = characterEntity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (characterEntity != null)
        {
            // Set Strength text field with strength
            UpdateTextFieldWithValue(strengthText, "str: ", strength);

            // Set Agility text field with agility
            UpdateTextFieldWithValue(agilityText, "agi: ", agility);

            // Set Constitution text field with constitution
            UpdateTextFieldWithValue(constitutionText, "con: ", constitution);

            // Set Intellect text field with intellect
            UpdateTextFieldWithValue(intellectText, "int: ", intellect);

            // Set Health text field with current & max health
            int realCurrentHealth = (int)characterEntity.GetType().GetField(currentHealth).GetValue(characterEntity);
            int realMaxHealth = (int)characterEntity.GetType().GetField(maxHealth).GetValue(characterEntity);
            healthText.text = realCurrentHealth + "/" + realMaxHealth;

            // Set Physical Damage Reduction text field with physicalDmgReduction
            //UpdateTextFieldWithValue(physicalDamageReductionText, "physical: ", physicalDamageReduction);

            // Set Magical Damage Reduction text field with magicalDmgReduction
            //UpdateTextFieldWithValue(magicalDamageReductionText, "magical: ", magicalDamageReduction);

            // Set Physical Armour text field with physicalArmour
            UpdateTextFieldWithValue(physicalArmourText, "phyzikal: ", physicalArmour);

            // Set Magical Armour text field with magicalArmour
            UpdateTextFieldWithValue(magicalArmourText, "majikal:  ", magicalArmour);
        }
    }

    void UpdateTextFieldWithValue(Text updatedField, string baseText, string statLiteralName)
    {
        int realValue = (int)characterEntity.GetType().GetField(statLiteralName).GetValue(characterEntity);
        updatedField.text = baseText + realValue;
    }

    public static void OnItemEquip(InventoryItem.ItemInfoBlock itemStats)
    {
        IncreaseStatValue(statStrength, itemStats.strength);
        IncreaseStatValue(statAgility, itemStats.agility);
        IncreaseStatValue(statConstitution, itemStats.constitution);
        statCharacterEntity.CalculateMaxHP();
        IncreaseStatValue(statIntellect, itemStats.intellect);
        IncreaseStatValue(statPhysicalArmour, itemStats.physicalArmour);
        IncreaseStatValue(statMagicalArmour, itemStats.magicalArmour);
    }

    public static void OnItemRemove(InventoryItem.ItemInfoBlock itemStats)
    {
        DecreaseStatValue(statStrength, itemStats.strength);
        DecreaseStatValue(statAgility, itemStats.agility);
        DecreaseStatValue(statConstitution, itemStats.constitution);
        statCharacterEntity.CalculateMaxHP();
        DecreaseStatValue(statIntellect, itemStats.intellect);
        DecreaseStatValue(statPhysicalArmour, itemStats.physicalArmour);
        DecreaseStatValue(statMagicalArmour, itemStats.magicalArmour);
    }

    static void IncreaseStatValue(string statToChange, int increaseBy)
    {
        int statValue = (int)statCharacterEntity.GetType().GetField(statToChange).GetValue(statCharacterEntity);
        statValue += increaseBy;
        statCharacterEntity.GetType().GetField(statToChange).SetValue(statCharacterEntity, statValue);
    }

    static void DecreaseStatValue(string statToChange, int decreaseBy)
    {
        int statValue = (int)statCharacterEntity.GetType().GetField(statToChange).GetValue(statCharacterEntity);
        statValue -= decreaseBy;
        statCharacterEntity.GetType().GetField(statToChange).SetValue(statCharacterEntity, statValue);
    }
}
