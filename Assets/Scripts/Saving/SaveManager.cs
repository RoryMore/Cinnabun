using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    

    [System.Serializable]
    public struct UpgradeList
    {
        public CharacterUpgrade teleportRange;
        public CharacterUpgrade playerMovespeed;
        public CharacterUpgrade bloodOrbEffectiveness;
        public CharacterUpgrade blastExplosionRadius;
        public CharacterUpgrade blastExplosionDamage;
        public CharacterUpgrade extraPauseAction;
        public CharacterUpgrade bonusAgilityCrit;
        public CharacterUpgrade armourEffectiveness;
    }

    [Header("Save File Names")]
    [SerializeField]
    [Tooltip("Must be prefixed with a '/'")]
    string upgradeSaveFileName;
    static string staticUpgradeSaveFileName;
    [SerializeField]
    [Tooltip("Must be prefixed with a '/'")]
    string upgradeMoneyFileName;
    static string staticUpgradeMoneyFileName;
    [SerializeField]
    [Tooltip("Must be prefixed with a '/'")]
    string settingsFileName;
    static string staticSettingsFileName;

    [Space]
    [Space]
    public string upgradeShopSceneName;
    public static string upgradeShopScene;
    public string gameSceneName;
    public static string gameScene;
    // Inspector values of this list are default values.
    // Defaults will be used on first time playing. Future times, access the save file
    [Tooltip("The values in this List will be the default starting values for the upgrades")]
    [SerializeField]
    UpgradeList defaultCharacterUpgrades;
    static UpgradeList characterUpgrades;

    [Tooltip("Setting Values assigned here will be default settings")]
    [SerializeField]
    SavedSettings defaultSettings;
    static SavedSettings savedSettings;
    static SavedSettings staticDefaultSettings;

    static int savedUpgradeMoney;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //Debug.Log(Application.persistentDataPath);
        staticUpgradeSaveFileName = upgradeSaveFileName;
        staticUpgradeMoneyFileName = upgradeMoneyFileName;
        staticSettingsFileName = settingsFileName;

        characterUpgrades = defaultCharacterUpgrades;
        savedSettings = defaultSettings;
        staticDefaultSettings = defaultSettings;

        upgradeShopScene = upgradeShopSceneName;
        gameScene = gameSceneName;

        if (LoadUpgradeSave())
        {
            Debug.Log("SaveManager: Upgrades Loaded");
        }
        else
        {
            Debug.Log("SaveManager: No UpgradeSave file to load");
        }
        if (LoadUpgradeMoney())
        {
            Debug.Log("SaveManager: UpgradeMoney Loaded");
        }
        else
        {
            Debug.Log("SaveManager: No UpgradeMoney file to load");
        }
        if (LoadSettingsSave())
        {
            Debug.Log("SaveManager: Settings Loaded");
        }
        else
        {
            Debug.Log("SaveManager: No Saved Settings file to load");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Period))
        //{
        //    CurrencyManager.AddUpgradeMoney(1);
        //}
        //else if (Input.GetKeyDown(KeyCode.Comma))
        //{
        //    CurrencyManager.DeductUpgradeMoney(1);
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    SaveUpgradeMoney();
        //}
    }

    public static void SaveUpgradeMoney()
    {
        BinaryFormatter bf = new BinaryFormatter();

        savedUpgradeMoney = CurrencyManager.GetUpgradeMoney();

        FileStream file = File.Create(Application.persistentDataPath + staticUpgradeMoneyFileName);
        bf.Serialize(file, savedUpgradeMoney);
        file.Close();
    }

    private bool LoadUpgradeMoney()
    {
        if (File.Exists(Application.persistentDataPath + upgradeMoneyFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + upgradeMoneyFileName, FileMode.Open);

            savedUpgradeMoney = (int)bf.Deserialize(file);
            CurrencyManager.SetUpgradeMoney(savedUpgradeMoney);

            file.Close();
            return true;
        }
        return false;
    }


    private static UpgradeSave CreateUpgradeSave()
    {
        UpgradeSave save = new UpgradeSave();

        save.teleportUpgrade = characterUpgrades.teleportRange;
        save.playerMovespeed = characterUpgrades.playerMovespeed;
        save.bloodOrbEffectiveness = characterUpgrades.bloodOrbEffectiveness;
        save.blastExplosionRadius = characterUpgrades.blastExplosionRadius;
        save.blastExplosionDamage = characterUpgrades.blastExplosionDamage;
        save.extraPauseAction = characterUpgrades.extraPauseAction;
        save.bonusAgilityCrit = characterUpgrades.bonusAgilityCrit;
        save.armourEffectiveness = characterUpgrades.armourEffectiveness;

        return save;
    }

    public static void SaveUpgrades()
    {
        BinaryFormatter bf = new BinaryFormatter();
        
        //Debug.Log("SaveManager: Upgrades saved");
        UpgradeSave save = CreateUpgradeSave();

        // Save file does not exist. We are creating one and saving it
        FileStream file = File.Create(Application.persistentDataPath + staticUpgradeSaveFileName);
        bf.Serialize(file, save);
        file.Close();
    }

    private bool LoadUpgradeSave()
    {
        if (File.Exists(Application.persistentDataPath + upgradeSaveFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + upgradeSaveFileName, FileMode.Open);

            UpgradeSave save = (UpgradeSave)bf.Deserialize(file);

            characterUpgrades.teleportRange = save.teleportUpgrade;
            characterUpgrades.playerMovespeed = save.playerMovespeed;
            characterUpgrades.bloodOrbEffectiveness = save.bloodOrbEffectiveness;
            characterUpgrades.blastExplosionRadius = save.blastExplosionRadius;
            characterUpgrades.blastExplosionDamage = save.blastExplosionDamage;
            characterUpgrades.extraPauseAction = save.extraPauseAction;
            characterUpgrades.bonusAgilityCrit = save.bonusAgilityCrit;
            characterUpgrades.armourEffectiveness = save.armourEffectiveness;

            //Debug.Log("Game Loaded from existing file");
            file.Close();
            return true;
        }
        // Load failed
        //Debug.Log("Game failed to load any existing file");
        return false;
    }


    private static SavedSettings CreateSettingsSave()
    {
        SavedSettings save = new SavedSettings();
        save.cameraMoveSensitivity = savedSettings.cameraMoveSensitivity;
        save.keybindings = savedSettings.keybindings;
        save.musicVolume = savedSettings.musicVolume;
        save.sfxVolume = savedSettings.sfxVolume;

        return save;
    }

    public static void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();

        SavedSettings save = CreateSettingsSave();

        FileStream file = File.Create(Application.persistentDataPath + staticSettingsFileName);
        bf.Serialize(file, save);
        file.Close();
    }

    private bool LoadSettingsSave()
    {
        if (File.Exists(Application.persistentDataPath + staticSettingsFileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + staticSettingsFileName, FileMode.Open);

            SavedSettings save = (SavedSettings)bf.Deserialize(file);

            savedSettings.cameraMoveSensitivity = save.cameraMoveSensitivity;
            savedSettings.keybindings = save.keybindings;
            savedSettings.musicVolume = save.musicVolume;
            savedSettings.sfxVolume = save.sfxVolume;

            file.Close();
            return true;
        }
        return false;
    }

    // Public Static Getters for accessing the settings/upgrades outside of this class
    public static SavedSettings GetSettings()
    {
        return savedSettings;
    }
    public static UpgradeList GetUpgradeList()
    {
        return characterUpgrades;
    }

    public static SavedSettings GetDefaultSettings()
    {
        return staticDefaultSettings;
    }
}
