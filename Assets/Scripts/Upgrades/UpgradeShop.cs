using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        [TextArea]
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

    [Header("Upgrade Money Counter")]
    public Text upgradeMoneyCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessUpgradeUI(teleportRange);
        //ProcessUpgradeUI(blastExplosionRadius);
        //ProcessUpgradeUI(blastExplosionDmgMultiplier);
        //ProcessUpgradeUI(bloodOrbEffectiveness);
        //ProcessUpgradeUI(playerBaseMovementSpeed);

        upgradeMoneyCounter.text = CurrencyManager.GetUpgradeMoney().ToString();
    }

    void ProcessUpgradeUI(Upgrade upgrade)
    {
        // Update Purchase Progress bar
        if (upgrade.upgrade.upgradeCount == upgrade.upgrade.maxUpgrades)
        {
            upgrade.ui.progressFillImage.fillAmount = 1.0f;

            upgrade.ui.progressText.text = "Maxed";
        }
        else
        {
            upgrade.ui.progressFillImage.fillAmount = (float)((float)upgrade.upgrade.progressToUpgrade / (float)upgrade.upgrade.GetUpgradeCost());  // Says casts are redundant. They aren't. Trust me
                                                                                                                                                    // Update Purchase Progress text
            upgrade.ui.progressText.text = upgrade.upgrade.progressToUpgrade.ToString() + "/" + upgrade.upgrade.GetUpgradeCost();
        }
        
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
        teleportRange.upgrade.SetDirty();
    }

    public void BuyBlastExplRadius()
    {
        blastExplosionRadius.upgrade.IncrementUpgradeProgress();
        blastExplosionRadius.upgrade.SetDirty();
    }

    public void BuyBlastExplDmgMultipier()
    {
        blastExplosionDmgMultiplier.upgrade.IncrementUpgradeProgress();
        blastExplosionDmgMultiplier.upgrade.SetDirty();
    }

    public void BuyBloodOrbEffectiveness()
    {
        bloodOrbEffectiveness.upgrade.IncrementUpgradeProgress();
        bloodOrbEffectiveness.upgrade.SetDirty();
    }

    public void BuyBaseMovementSpeed()
    {
        playerBaseMovementSpeed.upgrade.IncrementUpgradeProgress();
        playerBaseMovementSpeed.upgrade.SetDirty();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(CurrencyManager.gameSceneName);
    }
}
