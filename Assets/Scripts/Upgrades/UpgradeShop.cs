using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShop : MonoBehaviour
{
    [System.Serializable]
    public struct FunctionalUpgradeUI
    {
        public Image progressFillImage;
        public Text progressText;

        public Image buyButtonImage;
        public Sprite buyButtonSprite;
        public Sprite noBuyButtonSprite;
    }

    [System.Serializable]
    public struct Upgrade
    {
        public CharacterUpgrade upgrade;
        public FunctionalUpgradeUI ui;
        public string tooltipDescription;
    }

    [Header("Teleport Skill")]
    public Upgrade teleportRange;

    [Header("Blast Skill")]
    public Upgrade blastExplosionRadius;
    public Upgrade blastExplosionDmgMultiplier;

    [Header("Health Pickup")]
    public Upgrade bloodOrbEffectiveness;

    [Header("Player Stats")]
    public Upgrade playerBaseMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessUpgradeUI(teleportRange);
        ProcessUpgradeUI(blastExplosionRadius);
        ProcessUpgradeUI(blastExplosionDmgMultiplier);
        ProcessUpgradeUI(bloodOrbEffectiveness);
        ProcessUpgradeUI(playerBaseMovementSpeed);
    }

    void ProcessUpgradeUI(Upgrade upgrade)
    {
        // Update Purchase Progress bar
        upgrade.ui.progressFillImage.fillAmount = upgrade.upgrade.progressToUpgrade / upgrade.upgrade.GetUpgradeCost();

        // Update Purchase Progress text
        upgrade.ui.progressText.text = upgrade.upgrade.progressToUpgrade.ToString() + "/" + upgrade.upgrade.GetUpgradeCost();

        // Update Buy Button with appropriate graphic if a purchase can be made here
        if (upgrade.upgrade.CanBuyUpgrade() && (CurrencyManager.GetUpgradeMoney() > 0))
        {
            upgrade.ui.buyButtonImage.sprite = upgrade.ui.buyButtonSprite;

            //Debug.Log("Original Tooltip: " + upgrade.tooltipDescription);
            for (int i = 0; i < upgrade.tooltipDescription.Length; i++)
            {
                if (upgrade.tooltipDescription[i] == '+')
                {
                    upgrade.tooltipDescription = upgrade.tooltipDescription.Insert(i+1, upgrade.upgrade.upgradeMagnitude.ToString());
                    //Debug.Log("Edited Tooltip: " + upgrade.tooltipDescription);
                    break;
                }
            }
        }
        else
        {
            upgrade.ui.buyButtonImage.sprite = upgrade.ui.noBuyButtonSprite;
        }
    }

    public void BuyTeleportRange()
    {
        teleportRange.upgrade.IncrementUpgradeProgress();
    }

    public void BuyBlastExplRadius()
    {
        blastExplosionRadius.upgrade.IncrementUpgradeProgress();
    }

    public void BuyBlastExplDmgMultipier()
    {
        blastExplosionDmgMultiplier.upgrade.IncrementUpgradeProgress();
    }

    public void BuyBloodOrbEffectiveness()
    {
        bloodOrbEffectiveness.upgrade.IncrementUpgradeProgress();
    }

    public void BuyBaseMovementSpeed()
    {
        playerBaseMovementSpeed.upgrade.IncrementUpgradeProgress();
    }
}
