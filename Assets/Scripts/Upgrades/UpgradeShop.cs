using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
        public Text currentPurchasesbyMaxText;
    }

    [System.Serializable]
    public struct ShopUpgrade
    {
        public FunctionalUpgradeUI ui;
        [TextArea]
        public string tooltipDescription;
    }

    // Enum list for different skill categories. With the upgrades intended to be implemented this will come in handy to keep the shop interface clean rather than trying to shove all upgrades in one screen
    public enum SelectedShopTab
    {
        SKILLS,
        STATS,
        ITEMS
    }

    [Header("Teleport Skill")]
    public ShopUpgrade teleportRange;
    bool teleportButtonPressed = false;

    [Header("Blast Skill")]
    public ShopUpgrade blastExplosionRadius;
    bool explosionRadiusButtonPressed = false;
    public ShopUpgrade blastExplosionDmgMultiplier;

    [Header("Extra Action")]
    public ShopUpgrade extraActionUpgrade;
    bool extraActionButtonPressed = false;

    [Header("Health Pickup")]
    public ShopUpgrade bloodOrbEffectiveness;
    bool bloodOrbEffectivenessButtonPressed = false;

    [Header("Player Stats")]
    public ShopUpgrade playerBaseMovementSpeed;
    bool playerMovespeedButtonPressed = false;

    public ShopUpgrade bonusAgilityCrit;
    bool bonusAgilityCritButtonPressed = false;

    [Header("Upgrade Money Counter")]
    public Text upgradeMoneyCounter;

    [Header("Button Delay")]
    [SerializeField]
    float buyDelay;
    float currentDelayPass;

    bool upgradesSaved = false;
    SelectedShopTab selectedShopTab;

    [Header("Shop Tab Objects")]
    public GameObject skillTabObject;
    public GameObject statTabObject;
    public GameObject itemTabObject;

    
    EventSystem eventSystem;
    PointerEventData pointerEventData;
    GraphicRaycaster raycaster;

    [Header("Upgrade Description Tooltip")]
    [SerializeField]
    Canvas shopCanvas;

    [SerializeField]
    RectTransform tooltipPosition;
    [SerializeField]
    Text tooltipDescription;
    [SerializeField]
    Text tooltipCurrentBonus;
    [SerializeField]
    Text tooltipNextBonus;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        raycaster = shopCanvas.GetComponent<GraphicRaycaster>();

        selectedShopTab = SelectedShopTab.SKILLS;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentDelayPass = buyDelay;
        tooltipPosition.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Switch cases ready to implement shop tabs
        switch (selectedShopTab)
        {
            case SelectedShopTab.SKILLS:
                {
                    skillTabObject.SetActive(true);
                    statTabObject.SetActive(false);
                    itemTabObject.SetActive(false);
                    break;
                }
            case SelectedShopTab.STATS:
                {
                    skillTabObject.SetActive(false);
                    statTabObject.SetActive(true);
                    itemTabObject.SetActive(false);
                    break;
                }
            case SelectedShopTab.ITEMS:
                {
                    skillTabObject.SetActive(false);
                    statTabObject.SetActive(false);
                    itemTabObject.SetActive(true);
                    break;
                }
        }
        ProcessPressedButton(teleportButtonPressed, SaveManager.GetUpgradeList().teleportRange);
        ProcessPressedButton(playerMovespeedButtonPressed, SaveManager.GetUpgradeList().playerMovespeed);
        ProcessPressedButton(bloodOrbEffectivenessButtonPressed, SaveManager.GetUpgradeList().bloodOrbEffectiveness);
        ProcessPressedButton(explosionRadiusButtonPressed, SaveManager.GetUpgradeList().blastExplosionRadius);
        ProcessPressedButton(extraActionButtonPressed, SaveManager.GetUpgradeList().extraPauseAction);
        ProcessPressedButton(bonusAgilityCritButtonPressed, SaveManager.GetUpgradeList().bonusAgilityCrit);

        ProcessUpgradeUI(teleportRange, SaveManager.GetUpgradeList().teleportRange);
        ProcessUpgradeUI(blastExplosionRadius, SaveManager.GetUpgradeList().blastExplosionRadius);
        //ProcessUpgradeUI(blastExplosionDmgMultiplier);
        ProcessUpgradeUI(bloodOrbEffectiveness, SaveManager.GetUpgradeList().bloodOrbEffectiveness);
        ProcessUpgradeUI(playerBaseMovementSpeed, SaveManager.GetUpgradeList().playerMovespeed);
        ProcessUpgradeUI(extraActionUpgrade, SaveManager.GetUpgradeList().extraPauseAction);
        ProcessUpgradeUI(bonusAgilityCrit, SaveManager.GetUpgradeList().bonusAgilityCrit);

        upgradeMoneyCounter.text = CurrencyManager.GetUpgradeMoney().ToString();

        ProcessButtonRaycast();
    }

    void ProcessPressedButton(bool buttonPressed, CharacterUpgrade characterUpgrade)
    {
        if (buttonPressed)
        {
            currentDelayPass += Time.deltaTime;
            if (currentDelayPass >= buyDelay)
            {
                characterUpgrade.IncrementUpgradeProgress();
                currentDelayPass = 0.0f;
            }
        }
        else
        {
            if (!upgradesSaved)
            {
                SaveManager.SaveUpgradeMoney();
                SaveManager.SaveUpgrades();
                upgradesSaved = true;
                currentDelayPass = buyDelay;
            }
        }
    }

    void ProcessButtonRaycast()
    {
        tooltipPosition.gameObject.SetActive(false);
        List<RaycastResult> raycastResults = GetNewPointerEventRaycast();
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.name.Contains("Teleport"))
            {
                tooltipPosition.gameObject.SetActive(true);

                UpdateTooltip(teleportRange, SaveManager.GetUpgradeList().teleportRange, "range");
            }
            else if (result.gameObject.name.Contains("PlayerMovespeed"))
            {
                tooltipPosition.gameObject.SetActive(true);

                UpdateTooltip(playerBaseMovementSpeed, SaveManager.GetUpgradeList().playerMovespeed, "move speed");
            }
            else if (result.gameObject.name.Contains("BlastRadius"))
            {
                tooltipPosition.gameObject.SetActive(true);

                UpdateTooltip(blastExplosionRadius, SaveManager.GetUpgradeList().blastExplosionRadius, "range");
            }
            else if (result.gameObject.name.Contains("BloodOrb"))
            {
                tooltipPosition.gameObject.SetActive(true);

                // Position the Description
                Vector3 descriptionPosition = bloodOrbEffectiveness.ui.progressFillImage.transform.position;
                descriptionPosition.y += 30;
                tooltipPosition.position = descriptionPosition;

                tooltipDescription.text = bloodOrbEffectiveness.tooltipDescription;
                tooltipCurrentBonus.text = "current bonus: <color=lime>+" + SaveManager.GetUpgradeList().bloodOrbEffectiveness.GetUpgradedMagnitude().ToString() + "%</color> bonus healing";

                if (SaveManager.GetUpgradeList().bloodOrbEffectiveness.CanBuyUpgrade())
                {
                    float nextBonus = SaveManager.GetUpgradeList().bloodOrbEffectiveness.GetUpgradedMagnitude() + SaveManager.GetUpgradeList().bloodOrbEffectiveness.upgradeMagnitude;
                    tooltipNextBonus.text = "next bonus: <color=lime>+" + nextBonus.ToString() + "%</color> bonus healing";
                }
                else
                {
                    tooltipNextBonus.text = "next bonus: <color=white>MAXED</color>";
                }
            }
            else if (result.gameObject.name.Contains("ExtraAction"))
            {
                tooltipPosition.gameObject.SetActive(true);

                UpdateTooltip(extraActionUpgrade, SaveManager.GetUpgradeList().extraPauseAction, "action");
            }
            else if (result.gameObject.name.Contains("BonusAgilityCrit"))
            {
                tooltipPosition.gameObject.SetActive(true);

                // Position the Description
                Vector3 descriptionPosition = bonusAgilityCrit.ui.progressFillImage.transform.position;
                descriptionPosition.y += 30;
                tooltipPosition.position = descriptionPosition;

                tooltipDescription.text = bonusAgilityCrit.tooltipDescription;
                float critPercent = SaveManager.GetUpgradeList().bonusAgilityCrit.GetUpgradedMagnitude() * 100.0f;
                tooltipCurrentBonus.text = "current bonus: <color=lime>+" + critPercent.ToString() + "%</color> critical chance";

                if (SaveManager.GetUpgradeList().bonusAgilityCrit.CanBuyUpgrade())
                {
                    float nextBonus = (SaveManager.GetUpgradeList().bonusAgilityCrit.GetUpgradedMagnitude() + SaveManager.GetUpgradeList().bonusAgilityCrit.upgradeMagnitude) * 100.0f;
                    tooltipNextBonus.text = "next bonus: <color=lime>+" + nextBonus.ToString() + "%</color> critical chance";
                }
                else
                {
                    tooltipNextBonus.text = "next bonus: <color=white>MAXED</color>";
                }
            }
        }
    }

    void UpdateTooltip(ShopUpgrade shopUpgrade, CharacterUpgrade characterUpgrade, string bonusSuffix)
    {
        // Position the Description
        Vector3 descriptionPosition = shopUpgrade.ui.progressFillImage.transform.position;
        descriptionPosition.y += 30;
        tooltipPosition.position = descriptionPosition;

        tooltipDescription.text = shopUpgrade.tooltipDescription;
        tooltipCurrentBonus.text = "current bonus: <color=lime>+" + characterUpgrade.GetUpgradedMagnitude().ToString() + "</color> " + bonusSuffix;

        if (characterUpgrade.CanBuyUpgrade())
        {
            float nextBonus = characterUpgrade.GetUpgradedMagnitude() + characterUpgrade.upgradeMagnitude;
            tooltipNextBonus.text = "next bonus: <color=lime>+" + nextBonus.ToString() + "</color> " + bonusSuffix;
        }
        else
        {
            tooltipNextBonus.text = "next bonus: <color=white>MAXED</color>";
        }
    }

    void ProcessUpgradeUI(ShopUpgrade upgradeUiElements, CharacterUpgrade characterUpgrade)
    {
        // Update Purchase Progress bar
        if (characterUpgrade.upgradeCount == characterUpgrade.maxUpgrades)
        {
            upgradeUiElements.ui.progressFillImage.fillAmount = 1.0f;

            upgradeUiElements.ui.progressText.text = "Maxed";
        }
        else
        {
            upgradeUiElements.ui.progressFillImage.fillAmount = (float)((float)characterUpgrade.progressToUpgrade / (float)characterUpgrade.GetUpgradeCost());  // Says casts are redundant. They aren't. Trust me
                                                                                                                                                    // Update Purchase Progress text
            upgradeUiElements.ui.progressText.text = characterUpgrade.progressToUpgrade.ToString() + "/" + characterUpgrade.GetUpgradeCost();
        }
        upgradeUiElements.ui.currentPurchasesbyMaxText.text = characterUpgrade.upgradeCount.ToString() + "/" + characterUpgrade.maxUpgrades.ToString();
        
        // Update Buy Button with appropriate graphic if a purchase can be made here
        if (characterUpgrade.CanBuyUpgrade() && (CurrencyManager.GetUpgradeMoney() > 0))
        {
            upgradeUiElements.ui.buyButtonImage.sprite = upgradeUiElements.ui.buyButtonSprite;

            //Debug.Log("Original Tooltip: " + upgrade.tooltipDescription);
            for (int i = 0; i < upgradeUiElements.tooltipDescription.Length; i++)
            {
                if (upgradeUiElements.tooltipDescription[i] == '+')
                {
                    upgradeUiElements.tooltipDescription = upgradeUiElements.tooltipDescription.Insert(i+1, characterUpgrade.upgradeMagnitude.ToString());
                    //Debug.Log("Edited Tooltip: " + upgrade.tooltipDescription);
                    break;
                }
            }
        }
        else
        {
            upgradeUiElements.ui.buyButtonImage.sprite = upgradeUiElements.ui.noBuyButtonSprite;
        }
    }

    public void TeleportButtonDown()
    {
        teleportButtonPressed = true;
        upgradesSaved = false;
    }
    public void TeleportButtonUp()
    {
        teleportButtonPressed = false;
    }

    public void BlastExplRadiusButtonDown()
    {
        explosionRadiusButtonPressed = true;
        upgradesSaved = false;
    }

    public void BlastExplRadiusButtonUp()
    {
        explosionRadiusButtonPressed = false;
    }

    public void BlastExplDmgMultipierButtonDown()
    {
        upgradesSaved = false;
    }
    
    public void BlastExplDmgMultiplierButtonUp()
    {

    }

    public void BloodOrbEffectivenessButtonDown()
    {
        bloodOrbEffectivenessButtonPressed = true;
        upgradesSaved = false;
    }

    public void BloodOrbEffectivenessButtonUp()
    {
        bloodOrbEffectivenessButtonPressed = false;
    }

    public void PlayerMovespeedButtonDown()
    {
        playerMovespeedButtonPressed = true;
        upgradesSaved = false;
    }

    public void PlayerMovespeedButtonUp()
    {
        playerMovespeedButtonPressed = false;
    }

    public void ExtraActionButtonDown()
    {
        extraActionButtonPressed = true;
        upgradesSaved = false;
    }
    public void ExtraActionButtonUp()
    {
        extraActionButtonPressed = false;
    }

    public void BonusAgilityCritButtonDown()
    {
        bonusAgilityCritButtonPressed = true;
        upgradesSaved = false;
    }
    public void BonusAgilityCritButtonUp()
    {
        bonusAgilityCritButtonPressed = false;
    }

    public void SkillTabClicked()
    {
        selectedShopTab = SelectedShopTab.SKILLS;
    }

    public void StatsTabClicked()
    {
        selectedShopTab = SelectedShopTab.STATS;
    }

    public void ItemsTabClicked()
    {
        selectedShopTab = SelectedShopTab.ITEMS;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(CurrencyManager.gameSceneName);
    }

    public void LoadMainMenu()
    {
        // Level index 0 is likely the main menu
        SceneManager.LoadSceneAsync(0);
    }

    List<RaycastResult> GetNewPointerEventRaycast()
    {
        pointerEventData = new PointerEventData(eventSystem);

        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);

        return results;
    }
}
