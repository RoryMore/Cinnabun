using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct ItemSpawnerStruct
    {
        [Header("Rarity Tables")]
        [SerializeField] List<GameObject> commonItems;
        [SerializeField] List<GameObject> uncommonItems;
        [SerializeField] List<GameObject> rareItems;
        [SerializeField] List<GameObject> ultraItems;

        [Header("Rarity Ratios | Must have a value <= the previous")]
        // Ratio Value  | Chance to Hit Rarity Table (Roughly rounded)
        [SerializeField] int noLootRatio;     // 50           | 46.3  %
        [SerializeField] int commonRatio;     // 35           | 32.4  %
        [SerializeField] int uncommonRatio;   // 18           | 16.667%
        [SerializeField] int rareRatio;       // 5            | 4.63  %
        [SerializeField] int ultraRatio;

        public void SpawnItem(Vector3 position)
        {
            int rarityTotal = noLootRatio + commonRatio + uncommonRatio + rareRatio + ultraRatio;

            int randomNumber = Random.Range(0, (rarityTotal + 1));

            Vector3 spawnLocation = position;
            spawnLocation.x += Random.Range(-2.0f, 2.0f);
            spawnLocation.z += Random.Range(-2.0f, 2.0f);

            if ((randomNumber <= ultraRatio) && (ultraRatio != 0))  //randomNumber hit the ultra loot
            {
                Instantiate(ultraItems[Random.Range(0, ultraItems.Count)], spawnLocation, Quaternion.identity);
            }
            else if ((randomNumber <= rareRatio) && (rareRatio != 0)) //randomNumber hit the rare loot
            {
                Instantiate(rareItems[Random.Range(0, rareItems.Count)], spawnLocation, Quaternion.identity);
            }
            else if ((randomNumber <= uncommonRatio) && (uncommonRatio != 0)) //randomNumber hit the uncommon loot
            {
                Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], spawnLocation, Quaternion.identity);
            }
            else if ((randomNumber <= commonRatio) && (commonRatio != 0))  // randomNumber hit the common loot
            {
                Instantiate(commonItems[Random.Range(0, commonItems.Count)], spawnLocation, Quaternion.identity);
            }
            // Rip no loot down here. Bad luck
        }
    }
    // The chance of hitting a specific rarity table is: 100 / ((SumOfAllRatioCoefficients) / SpecificRatio) = ChanceForRarity
    // The chance of getting a specific item from a rarity is: 1 / NumberOfItemsInRarity * ChanceForRarity
}
