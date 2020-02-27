using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{

    public Entity enemy1;
    public Entity enemy2;


    public List<Entity> masterInitiativeList; //Unchanging list of encounter made at its initilization
    public List<Entity> initiativeList; //List that updates and changes as enemies die. Used for enemy manager, not for skills
    public List<Entity> playerInclusiveInitiativeList; //Same as master but includes player for skill use, for enemy skills
    public List<Entity> healList;

    //NIK___List of skill which each enemy is going to use

    public List<EnemyScript> enemies;

    public List<GameObject> spawnPoints;

    public EnemyManager enemyManager;

    public bool cleared = false;

    // Inventory to add item to
    [Header("Temporary Inventory stuff")]
    
    public List<Item> items;

    [SerializeField]
    InventoryBase inventory;


    // Start is called before the first frame update
    void Start()
    {
        cleared = false;
    }

    public void SpawnEnemies()
    {
        


        //Spawn enemies
        //WARNING: If more spawn points are open than prefabs to fill them, this function breaks
        foreach (GameObject location in spawnPoints)
        {

            if (location.name.Contains("Enemy1"))
            {
                initiativeList.Add(Instantiate(enemy1, location.transform));

            }
            else if (location.name.Contains("Enemy2"))
            {
                initiativeList.Add(Instantiate(enemy2, location.transform));
            }
            else if (location.name.Contains("Enemy3"))
            {

            }
        }

        masterInitiativeList.AddRange(initiativeList);

        //Set up player inclusive
        playerInclusiveInitiativeList.AddRange(masterInitiativeList);
        playerInclusiveInitiativeList.Add(GameObject.Find("Player").GetComponent<Entity>());
    }

    void Awake()
    {
        //inventory = FindObjectOfType<InventoryBase>();

        if (inventory != null)
        {
            Debug.Log("Inventory set properly");
        }
    }

    // Update is called once per frame
    void Update()
    {
        KillCode();

        if (initiativeList.Count == 0)
        {
            if (!cleared)
            {
                Debug.Log("The encounter has been cleared!");
                Cleared();
            }

        }
    }

    public void Cleared()
    {
        //The encounter has been defeated!
        cleared = true;
        enemyManager.WaveActive = false;
        enemyManager.inBattle = false;
        enemyManager.SetTimeToNextWave(enemyManager.timeBetweenWaves);
        enemyManager.CheckVictory();
        GiveItem();
        //enemyManager.player.GetComponent<Entity>().currentHP += 25;
        
    }

    public void EnemyGotHurt(Entity enemy)
    {
        healList.Add(enemy);
    }

    public void EnemyNoLongerNeedHealed(Entity enemy)
    {
        healList.Remove(enemy);
    }

    public void GiveItem()
    {
        //Give the player an item to use
        Debug.Log("Player has obtained an Item!... but not really");

        if (inventory != null)
        {
            Debug.Log("Item given to player for real");
            int choice = (int)Random.Range(0, 4);
            {
                inventory.AddItem(items[choice]);
            }
            
        }
    }

    public void KillCode()
    {
        if (Input.GetKeyDown("k"))
        {
            if (isActiveAndEnabled)
            {
                foreach (Entity enemy in initiativeList)
                {
                    enemy.TakeDamage(enemy.maxHP);
                }
            }

        }
    }


}
