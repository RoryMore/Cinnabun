using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public enum ItemRarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        ULTRA
    }

    [Header("Rarity Tables")]
    public List<GameObject> commonItems;
    public List<GameObject> uncommonItems;
    public List<GameObject> rareItems;
    public List<GameObject> ultraItems;

    [Header("Rarity Ratios | Must have a value smaller than the previous")]
                                // Ratio Value  | Chance to Hit Rarity Table (Roughly rounded)
    public int noLootRatio;     // 50           | 46.3  %
    public int commonRatio;     // 35           | 32.4  %
    public int uncommonRatio;   // 18           | 16.667%
    public int rareRatio;       // 5            | 4.63  %
    public int ultraRatio;

    // The chance of hitting a specific rarity table is: 100 / ((SumOfAllRatioCoefficients) / SpecificRatio) = ChanceForRarity
    // The chance of getting a specific item from a rarity is: 1 / NumberOfItemsInRarity * ChanceForRarity

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(Vector3 position)
    {
        int rarityTotal = noLootRatio + commonRatio + uncommonRatio + rareRatio + ultraRatio;

        int randomNumber = Random.Range(0, (rarityTotal + 1));

        if ((randomNumber <= ultraRatio) && (ultraRatio != 0))  //randomNumber hit the ultra loot
        {
            Instantiate(ultraItems[Random.Range(0, ultraItems.Count)], position, Quaternion.identity);
        }
        else if ((randomNumber <= rareRatio) && (rareRatio != 0)) //randomNumber hit the rare loot
        {
            Instantiate(rareItems[Random.Range(0, rareItems.Count)], position, Quaternion.identity);
        }
        else if ((randomNumber <= uncommonRatio) && (uncommonRatio != 0)) //randomNumber hit the uncommon loot
        {
            Instantiate(uncommonItems[Random.Range(0, uncommonItems.Count)], position, Quaternion.identity);
        }
        else if ((randomNumber <= commonRatio) && (commonRatio != 0))  // randomNumber hit the common loot
        {
            Instantiate(commonItems[Random.Range(0, commonItems.Count)], position, Quaternion.identity);
        }
        // Rip no loot down here. Bad luck
    }
}
