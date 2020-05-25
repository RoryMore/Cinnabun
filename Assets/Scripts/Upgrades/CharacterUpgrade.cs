using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class CharacterUpgrade
{
    //public enum UpgradeType
    //{
    //    TeleportRange,
    //    PlayerMoveSpeed,
    //    BloodOrbEffectiveness,
    //    BlastExplosionRadius
    //}

    //public UpgradeType upgradeType;

    [Header("Number of Upgrades & Magnitude")]
    public int maxUpgrades;
    public int upgradeCount;
    public float upgradeMagnitude;

    [Header("Costs and Progress")]
    public int progressToUpgrade;
    [SerializeField]
    int initialUpgradeCost;
    [SerializeField]
    int upgradeCostIncrements;

    public CharacterUpgrade()
    {
        //upgradeType = type;

        maxUpgrades = 0;
        upgradeCount = 0;
        upgradeMagnitude = 0;

        progressToUpgrade = 0;
        initialUpgradeCost = 0;
        upgradeCostIncrements = 0;
    }

    public bool CanBuyUpgrade()
    {
        if (upgradeCount < maxUpgrades)
        {
            return true;
        }
        return false;
    }

    void IncrementUpgradeCount()
    {
        if (upgradeCount < maxUpgrades)
        {
            upgradeCount++;
            progressToUpgrade = 0;
        }
    }

    public void IncrementUpgradeProgress()
    {
        if (progressToUpgrade < GetUpgradeCost() && (CurrencyManager.GetUpgradeMoney() > 0))
        {
            progressToUpgrade++;
            CurrencyManager.DeductUpgradeMoney(1);

            if (progressToUpgrade >= GetUpgradeCost())
            {
                IncrementUpgradeCount();
            }
        }
    }

    public void DecrementUpgradeProgress()
    {
        if (progressToUpgrade > 0)
        {
            progressToUpgrade--;
            CurrencyManager.AddUpgradeMoney(1);
        }
    }

    public int GetUpgradeCost()
    {
        if (upgradeCount < maxUpgrades)
        {
            return initialUpgradeCost + (upgradeCount * upgradeCostIncrements);
        }
        else
        {
            return 0;
        }
    }

    public float GetUpgradedMagnitude()
    {
        return upgradeMagnitude * upgradeCount;
    }
}
