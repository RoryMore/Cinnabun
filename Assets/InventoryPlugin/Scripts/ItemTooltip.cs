using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField]
    RarityColour rarityColours;

    [Header("Tooltip Text Fields")]
    [SerializeField]
    Text itemName;
    [SerializeField]
    Text itemStrength;
    [SerializeField]
    Text itemAgility;
    [SerializeField]
    Text itemConstitution;
    [SerializeField]
    Text itemIntellect;
    [SerializeField]
    Text itemPhysicalArm;
    [SerializeField]
    Text itemMagicalArm;

    InventoryItem.ItemInfoBlock hoveredItemInfo;
    InventoryItem.ItemInfoBlock equippedItemInfo;

    Player player;
    bool hoveredItemEquipped;

    // Start is called before the first frame update
    void Start()
    {
        hoveredItemInfo = new InventoryItem.ItemInfoBlock();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        hoveredItemEquipped = false;
    }

    // Update is called once per frame
    void Update()
    {

        itemName.text = hoveredItemInfo.itemName;
        switch(hoveredItemInfo.rarity)
        {
            case ItemData.ItemRarity.COMMON:
                {
                    itemName.color = rarityColours.commonColour;
                    break;
                }

            case ItemData.ItemRarity.UNCOMMON:
                {
                    itemName.color = rarityColours.uncommonColour;
                    break;
                }

            case ItemData.ItemRarity.RARE:
                {
                    itemName.color = rarityColours.rareColour;
                    break;
                }

            case ItemData.ItemRarity.ULTRA:
                {
                    itemName.color = rarityColours.ultraColour;
                    break;
                }
        }

        SetStatText(itemStrength, hoveredItemInfo.strength, equippedItemInfo.strength, player.strength, "strength: ");
        SetStatText(itemAgility, hoveredItemInfo.agility, equippedItemInfo.agility, player.agility, "agility: ");
        SetStatText(itemConstitution, hoveredItemInfo.constitution, equippedItemInfo.constitution, player.constitution, "con: ");
        SetStatText(itemIntellect, hoveredItemInfo.intellect, equippedItemInfo.intellect, player.intellect, "int: ");
        SetStatText(itemPhysicalArm, hoveredItemInfo.physicalArmour, equippedItemInfo.physicalArmour, player.physicalArmour, "phyzikal armour: ");
        SetStatText(itemMagicalArm, hoveredItemInfo.magicalArmour, equippedItemInfo.magicalArmour, player.magicalArmour, "majikal armour: ");
    }

    void SetStatText(Text field, int itemStat, int equippedStat, int samePlayerStat, string statField)
    {
        int statDifference = 0, itemDifference = 0;
        if (equippedStat != 0)
        {
            itemDifference = itemStat - equippedStat;
        }
        else
        {
            itemDifference = itemStat;
        }
        if (hoveredItemEquipped)
        {
            statDifference = -itemStat;
        }
        else
        {
            statDifference = itemDifference;
        }
        if (Mathf.Sign(statDifference) > 0)
        {
            field.text = statField + itemStat.ToString() + " (<color=lime>+" + statDifference.ToString() + "</color>)";
        }
        else
        {
            field.text = statField + itemStat.ToString() + " (<color=red>" + statDifference.ToString() + "</color>)";
        }
    }

    public void SetEquippedItemInfo(InventoryItem.ItemInfoBlock equippedInfo)
    {
        equippedItemInfo = equippedInfo;
    }

    public void SetHoveredItem(InventoryItem.ItemInfoBlock itemInfo, bool isEquipped)
    {
        hoveredItemInfo = itemInfo;
        hoveredItemEquipped = isEquipped;
    }
}
