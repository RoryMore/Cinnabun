using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UpgradeSaveManager : MonoBehaviour
{
    // TODECIDE: Rename this class and make it a general save manager that contains Keybindings as well

    // Inspector values of this list are default values.
    // Defaults will be used on first time playing. Future times, access the save file
    public List<CharacterUpgrade> characterUpgrades;

    // CurrencyManager functionality might be merged into this
    public string gameSceneName;
    [SerializeField]
    int upgradeMoney;
    int gold;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log(Application.persistentDataPath);
        LoadUpgradeSave();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private UpgradeSave CreateUpgradeSave()
    {
        UpgradeSave save = new UpgradeSave();

        foreach (CharacterUpgrade upgrade in characterUpgrades)
        {
            switch (upgrade.upgradeType)
            {
                case CharacterUpgrade.UpgradeType.TeleportRange:
                    {
                        save.teleportUpgrade = upgrade;
                        break;
                    }
                case CharacterUpgrade.UpgradeType.PlayerMoveSpeed:
                    {
                        save.playerMovespeed = upgrade;
                        break;
                    }
                case CharacterUpgrade.UpgradeType.BloodOrbEffectiveness:
                    {
                        save.bloodOrbEffectiveness = upgrade;
                        break;
                    }
                case CharacterUpgrade.UpgradeType.BlastExplosionRadius:
                    {
                        save.blastExplosionRadius = upgrade;
                        break;
                    }
            }
        }
        save.upgradeMoney = upgradeMoney;

        return save;
    }

    public void SaveUpgrades()
    {
        BinaryFormatter bf = new BinaryFormatter();
        
        //Debug.Log("Game Saved: New File Created");
        UpgradeSave save = CreateUpgradeSave();

        // Save file does not exist. We are creating one and saving it
        FileStream file = File.Create(Application.persistentDataPath + "/upgradeSave.ugs");
        bf.Serialize(file, save);
        file.Close();
    }

    public bool LoadUpgradeSave()
    {
        if (File.Exists(Application.persistentDataPath + "/upgradeSave.ugs"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/upgradeSave.ugs", FileMode.Open);

            UpgradeSave save = (UpgradeSave)bf.Deserialize(file);
            

            for (int i = 0; i < characterUpgrades.Count-1; i++)
            {
                switch (characterUpgrades[i].upgradeType)
                {
                    case CharacterUpgrade.UpgradeType.TeleportRange:
                        {
                            characterUpgrades[i] = save.teleportUpgrade;
                            break;
                        }
                    case CharacterUpgrade.UpgradeType.PlayerMoveSpeed:
                        {
                            characterUpgrades[i] = save.playerMovespeed;
                            break;
                        }
                    case CharacterUpgrade.UpgradeType.BloodOrbEffectiveness:
                        {
                            characterUpgrades[i] = save.bloodOrbEffectiveness;
                            break;
                        }
                    case CharacterUpgrade.UpgradeType.BlastExplosionRadius:
                        {
                            characterUpgrades[i] = save.blastExplosionRadius;
                            break;
                        }
                }
            }
            upgradeMoney = save.upgradeMoney;
            //Debug.Log("Game Loaded from existing file");
            file.Close();
            return true;
        }
        // Load failed
        //Debug.Log("Game failed to load any existing file");
        return false;
    }
}
