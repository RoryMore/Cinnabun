using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnControl : MonoBehaviour
{
    Player player;
    Transform respawnPoint;

    public InventoryBase inventoryBase;

    bool itemsCleared = false;

    WinLoseCanvasControl winLoseCanvas;

    EnemyManager enemyManager;

    bool moneyRewarded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        respawnPoint = GameObject.FindGameObjectWithTag("MainRespawn").transform;
        winLoseCanvas = FindObjectOfType<WinLoseCanvasControl>();
        enemyManager = FindObjectOfType<EnemyManager>();

        moneyRewarded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {

            if (winLoseCanvas.loseAlphaFull)
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
                            CharacterPanelStatControl.OnItemRemove(item);

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

                //player.transform.position = respawnPoint.position;
                //player.nav.Warp(respawnPoint.position);
                //player.Revive();
                //itemsCleared = false;

                int wavesCleared = 0;
                // Add an amount of upgrade money = wavesCompleted
                foreach (Encounter encounter in enemyManager.encounters)
                {
                    if (encounter.cleared)
                    {
                        wavesCleared++;
                    }
                }
                if (!moneyRewarded)
                {
                    CurrencyManager.AddUpgradeMoney(enemyManager.numOfClearedEncounters);
                    SaveManager.SaveUpgradeMoney();
                    moneyRewarded = true;
                }
                SceneManager.LoadSceneAsync(SaveManager.upgradeShopScene);
            }
        }
        //else
        //{
        //    if (winLoseCanvas != null)
        //    {
        //        if (winLoseCanvas.gameWon)
        //        {
        //            string currentScene = SceneManager.GetActiveScene().name;
        //            SceneManager.LoadSceneAsync(currentScene);
        //        }
        //    }
        //}
    }
}
