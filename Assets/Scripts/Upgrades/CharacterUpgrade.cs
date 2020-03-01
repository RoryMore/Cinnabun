using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterUpgrade", order = 1)]
[System.Serializable]
public class CharacterUpgrade : ScriptableObject
{
    [Tooltip("How many times this upgrade can be bought")]
    public int maxUpgrades;
    [Tooltip("The number of upgrades the player has purchased")]
    public int upgradeCount;
    [Tooltip("UpgradeMagnitude is only relevant for things like skill range upgrades")]
    public float upgradeMagnitude;

    [Header("Upgrade Costs/Buying")]
    [Tooltip("The current amount of points put in this upgrade")]
    public int progressToUpgrade;
    [Tooltip("How much the upgrade will cost")]
    [SerializeField]
    int initialUpgradeCost;
    [Tooltip("How much upgrade cost will be incremented per existing upgrade")]
    [SerializeField]
    int upgradeCostIncrements;

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
            EditorUtility.SetDirty(this);
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
            EditorUtility.SetDirty(this);
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

    public void ResetUpgrades()
    {
        upgradeCount = 0;
        EditorUtility.SetDirty(this);
    }
}
