using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CurrencyManager : MonoBehaviour
{
    //public string nonStat_gameSceneName;
    public static string gameSceneName;

    [Header("Currencies")]
    //int upgradeMoney;
    static int staticUpgradeMoney;

    //int gold;
    static int staticGold;

    bool upgradeMoneyRewarded;
    Player player;
    EnemyManager enemyManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameSceneName = SaveManager.gameScene;

        
        //staticUpgradeMoney = upgradeMoney;

        // If we are in the game scene, set our gold to 0 because we always start with 0 gold.
        if (SceneManager.GetActiveScene().name == gameSceneName)
        {
            //gold = 0;
            staticGold = 0;
            player = FindObjectOfType<Player>();
        }

        upgradeMoneyRewarded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (gold != staticGold)
        //{
        //    gold = staticGold;
        //}
        //Debug.Log("Gold: " + gold);
        //Debug.Log("UpgradeMoney: " + staticUpgradeMoney);
        if (!upgradeMoneyRewarded)
        {
            if (enemyManager != null)
            {
                if (player != null)
                {
                    if (player.isDead)
                    {
                        
                        
                        
                        upgradeMoneyRewarded = true;
                    }
                }
            }
        }
    }

    public static void AddUpgradeMoney(int value)
    {
        staticUpgradeMoney += value;
        //Debug.Log("CurrencyManager: Money Rewarded = " + value);
        staticUpgradeMoney = Mathf.Clamp(staticUpgradeMoney, 0, int.MaxValue);
        //Debug.Log("CurrencyManager: staticUpgradeMoney = " + staticUpgradeMoney);
    }

    public static void DeductUpgradeMoney(int value)
    {
        staticUpgradeMoney -= value;
        staticUpgradeMoney = Mathf.Clamp(staticUpgradeMoney, 0, staticUpgradeMoney);
    }

    public static int GetUpgradeMoney()
    {
        return staticUpgradeMoney;
    }

    public static void SetUpgradeMoney(int amount)
    {
        staticUpgradeMoney = amount;
        staticUpgradeMoney = Mathf.Clamp(staticUpgradeMoney, 0, int.MaxValue);
    }

    public static void AddGold(int value)
    {
        staticGold += value;
    }

    public static void DeductGold(int value)
    {
        staticGold -= value;
    }

    public static int GetGoldValue()
    {
        return staticGold;
    }
}
