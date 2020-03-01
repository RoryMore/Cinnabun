using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]
    string gameSceneName;

    [Header("Currencies")]
    public UpgradeMoney upgradeMoney;
    static UpgradeMoney staticUpgradeMoney;

    public int gold;
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
        staticUpgradeMoney = upgradeMoney;
        // If we are in the game scene, set our gold to 0 because we always start with 0 gold.
        if (SceneManager.GetActiveScene().name == gameSceneName)
        {
            gold = 0;
            player = FindObjectOfType<Player>();
            enemyManager = FindObjectOfType<EnemyManager>();
        }
        staticGold = 0;

        upgradeMoneyRewarded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gold != staticGold)
        {
            gold = staticGold;
        }
        Debug.Log("Gold: " + gold);
        Debug.Log("UpgradeMoney: " + upgradeMoney.GetAmount());
        if (!upgradeMoneyRewarded)
        {
            if (enemyManager != null)
            {
                if (player != null)
                {
                    if (player.isDead)
                    {
                        int wavesCleared = 0;
                        // Add an amount of upgrade money = wavesCompleted
                        foreach (Encounter encounter in enemyManager.encounters)
                        {
                            if (encounter.cleared)
                            {
                                wavesCleared++;
                            }
                        }
                        upgradeMoney.AddMoney(wavesCleared);
                        upgradeMoneyRewarded = true;
                    }
                }
            }
        }
    }

    public static void AddUpgradeMoney(int value)
    {
        staticUpgradeMoney.AddMoney(value);
    }

    public static void DeductUpgradeMoney(int value)
    {
        staticUpgradeMoney.DeductMoney(value);
    }

    public static int GetUpgradeMoney()
    {
        return staticUpgradeMoney.GetAmount();
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
