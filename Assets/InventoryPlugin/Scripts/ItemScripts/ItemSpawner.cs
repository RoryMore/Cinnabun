using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ItemSpawner : MonoBehaviour
{

    static bool firstSetDropped = false;
    static int firstSetIndex = 0;

    [Header("First Items")]
    [SerializeField]
    List<GameObject> firstArmourSet;
    static List<GameObject> firstArmourSetStatic;

    [Header("Rarity Tables")]
    [SerializeField] List<GameObject> commonItems;
    static List<GameObject> commonItemsStatic;
    [SerializeField] List<GameObject> uncommonItems;
    static List<GameObject> uncommonItemsStatic;
    [SerializeField] List<GameObject> rareItems;
    static List<GameObject> rareItemsStatic;
    [SerializeField] List<GameObject> ultraItems;
    static List<GameObject> ultraItemsStatic;

    [Header("Rarity Ratios | Must have a value <= the previous")]
    // Ratio Value  | Chance to Hit Rarity Table (Roughly rounded)
    [SerializeField] int noLootRatio;     // 50           | 45.45  %
    static int noLootRatioStatic;
    [SerializeField] int commonRatio;     // 35           | 31.8  %
    static int commonRatioStatic;
    [SerializeField] int uncommonRatio;   // 18           | 16.36%
    static int uncommonRatioStatic;
    [SerializeField] int rareRatio;       // 5            | 4.54  %
    static int rareRatioStatic;
    [SerializeField] int ultraRatio;      // 2            | 1.8
    static int ultraRatioStatic;

    // The chance of hitting a specific rarity table is: 100 / ((SumOfAllRatioCoefficients) / SpecificRatio) = ChanceForRarity
    // The chance of getting a specific item from a rarity is: 1 / NumberOfItemsInRarity * ChanceForRarity

    private void Awake()
    {
        noLootRatioStatic = noLootRatio;

        commonItemsStatic = commonItems;
        commonRatioStatic = commonRatio;

        uncommonItemsStatic = uncommonItems;
        uncommonRatioStatic = uncommonRatio;

        rareItemsStatic = rareItems;
        rareRatioStatic = rareRatio;

        ultraItemsStatic = ultraItems;
        ultraRatioStatic = ultraRatio;

        firstArmourSetStatic = firstArmourSet;
    }

    private void Start()
    {
        firstSetDropped = false;
        firstSetIndex = 0;
    }

    
    public static void SpawnItem(Vector3 position)
    {
        if (!firstSetDropped)
        {
            Vector3 spawnLocation = position;
            spawnLocation.x += Random.Range(-2.0f, 2.0f);
            spawnLocation.z += Random.Range(-2.0f, 2.0f);

            Instantiate(firstArmourSetStatic[firstSetIndex], spawnLocation, Quaternion.identity);

            firstSetIndex++;
            if (firstSetIndex >= 3)
            {
                firstSetDropped = true;
            }
        }
        else
        {
            int rarityTotal = noLootRatioStatic + commonRatioStatic + uncommonRatioStatic + rareRatioStatic + ultraRatioStatic;

            int randomNumber = Random.Range(0, (rarityTotal + 1));

            Vector3 spawnLocation = position;
            spawnLocation.x += Random.Range(-2.0f, 2.0f);
            spawnLocation.z += Random.Range(-2.0f, 2.0f);

            if ((randomNumber <= ultraRatioStatic) && (ultraRatioStatic != 0))  //randomNumber hit the ultra loot
            {
                Instantiate(ultraItemsStatic[Random.Range(0, ultraItemsStatic.Count)], spawnLocation, Quaternion.identity);
            }
            else if ((randomNumber <= rareRatioStatic) && (rareRatioStatic != 0)) //randomNumber hit the rare loot
            {
                Instantiate(rareItemsStatic[Random.Range(0, rareItemsStatic.Count)], spawnLocation, Quaternion.identity);
            }
            else if ((randomNumber <= uncommonRatioStatic) && (uncommonRatioStatic != 0)) //randomNumber hit the uncommon loot
            {
                Instantiate(uncommonItemsStatic[Random.Range(0, uncommonItemsStatic.Count)], spawnLocation, Quaternion.identity);
            }
            else if ((randomNumber <= commonRatioStatic) && (commonRatioStatic != 0))  // randomNumber hit the common loot
            {
                Instantiate(commonItemsStatic[Random.Range(0, commonItemsStatic.Count)], spawnLocation, Quaternion.identity);
            }
            // Rip no loot down here. Bad luck
        }
    }
}
