using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnControl : MonoBehaviour
{
    Player player;
    Transform respawnPoint;

    public InventoryBase inventoryBase;

    bool itemsCleared = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        respawnPoint = GameObject.FindGameObjectWithTag("MainRespawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            FadeUI.FadeIn();

            if (FadeUI.fadeInComplete)
            {
                if (!itemsCleared)
                {
                    itemsCleared = true;

                    foreach (InventoryItem item in inventoryBase.playerOwnedItems)
                    {
                        if (item.isMouseItem)
                        {
                            continue;
                        }
                        if (item.isEquipped)
                        {
                            CharacterPanelStatControl.OnItemRemove(item.item);

                            item.usedEquipSlot.equippedItem = null;
                            item.usedEquipSlot.isUsed = false;
                        }
                        foreach (InventorySlot slot in item.slotsUsed)
                        {
                            slot.isUsed = false;
                            slot.storedItem = null;
                        }
                        Destroy(item.gameObject);
                    }
                    inventoryBase.playerOwnedItems.Clear();
                }

                player.transform.position = respawnPoint.position;
                player.Revive();
                itemsCleared = false;
            }
        }
        else
        {
            FadeUI.FadeOut();
        }
    }
}
