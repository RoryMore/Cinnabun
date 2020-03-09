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
    
    [Header("Wave Variables")]
    public WaveType waveType;
    public bool milestone = false;

    //Endless Mode Variables

    [Tooltip("For ENDLESS waves, these variables determine countdowns")]
    public float countdownToNextRespawn; //Countdown between waves of enemies
    public float waveOverTimer; //How long you have to survive

    [SerializeField]
    private float respawnTicker;
    [SerializeField]
    private float waveOverTicker;

    

    // Inventory to add item to
    [Header("LootSystem")]
    public ItemSpawner.ItemSpawnerStruct itemSpawner;

    public enum WaveType
    {
        SLAUGHTER, //Kill all enemies who spawn to progress
        ENDLESS, //Enemies keep coming until a timer ends
        MINIBOSS, // One large enemy randomly spawns at one of the spawnpoints
        SHOP // Wave with no enemies


    }


    // Start is called before the first frame update
    void Start()
    {
        cleared = false;
    }

    public void Initialise()
    {
        switch(waveType)
        {
            //Kill a set number of People
            case (WaveType.SLAUGHTER):
                SpawnEnemies();
                if (milestone)
                {
                    SpawnEnemies();
                }

                break;

            case (WaveType.ENDLESS):
                SpawnEnemies();

                respawnTicker = countdownToNextRespawn;
                waveOverTicker = waveOverTimer;

                if (milestone)
                {
                    respawnTicker = countdownToNextRespawn / 2;
                    countdownToNextRespawn = countdownToNextRespawn / 2;
                }

                break;

            case (WaveType.MINIBOSS):
                //We don't need the normal spawn enemies function since we only have one enemy 
                SpawnBoss();

                if (milestone)
                {
                    //Need to rewrite from using Init 0 if I want it to spawn double boss instead of stronger boss
                    //SpawnBoss();
                }

                break;

        }
        
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

    public void SpawnBoss()
    {
        int randomNum = Random.Range(0, spawnPoints.Count);


        if (spawnPoints[randomNum].name.Contains("Enemy1"))
        {
            initiativeList.Add(Instantiate(enemy1, spawnPoints[randomNum].transform));
            initiativeList[0].gameObject.transform.localScale = initiativeList[0].gameObject.transform.localScale * 2;

        }
        else if (spawnPoints[randomNum].name.Contains("Enemy2"))
        {
            initiativeList.Add(Instantiate(enemy2, spawnPoints[randomNum].transform));
            initiativeList[0].gameObject.transform.localScale = initiativeList[0].gameObject.transform.localScale * 2;
        }
        else if (spawnPoints[randomNum].name.Contains("Enemy3"))
        {

        }

        masterInitiativeList.AddRange(initiativeList);

        //Set up player inclusive
        playerInclusiveInitiativeList.AddRange(masterInitiativeList);
        playerInclusiveInitiativeList.Add(GameObject.Find("Player").GetComponent<Entity>());
    }

    // Update is called once per frame
    void Update()
    {
        KillCode();

        //Each wave has its own update function to determine if you have completed it
        switch(waveType)
        {
            /// Kill all enemies
            case WaveType.SLAUGHTER:
            if (initiativeList.Count == 0)
            {
                if (!cleared)
                {
                    Debug.Log("The Slaughter encounter has been cleared!");
                    Cleared();
                }

            }
            break;

            /// Kill an endless supply of enemies until the end
            case WaveType.ENDLESS:
                waveOverTicker -= Time.deltaTime;
                respawnTicker -= Time.deltaTime;

                if (respawnTicker <= 0)
                {
                    SpawnEnemies();
                    respawnTicker = countdownToNextRespawn;
                }
               
                if (waveOverTicker <= 0 )
                {
                    if (!cleared)
                    {
                        Debug.Log("The Endless encounter has been cleared!");
                        if (isActiveAndEnabled)
                        {
                            foreach (Entity enemy in masterInitiativeList)
                            {

                                enemy.TakeDamage(enemy.maxHP);
                            }
                        }
                        Cleared();
                    }
                }
            break;

            /// Kill one big boi
            case WaveType.MINIBOSS:
            if (initiativeList.Count == 0)
            {
                if (!cleared)
                {
                    Debug.Log("The Slaughter encounter has been cleared!");
                    Cleared();
                }

            }
            break;



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
        
    }

    public void EnemyGotHurt(Entity enemy)
    {
        healList.Add(enemy);
    }

    public void EnemyNoLongerNeedHealed(Entity enemy)
    {
        healList.Remove(enemy);
    }

    public void KillCode()
    {
        if (Input.GetKeyDown("k"))
        {
            if (isActiveAndEnabled)
            {
                foreach (Entity enemy in masterInitiativeList)
                {
                    
                    enemy.TakeDamage(enemy.maxHP);
                }
            }

        }
    }

    public void SetActiveBehavior()
    {

        foreach (SimpleEnemy enemy in masterInitiativeList)
        {
            enemy.SwitchActiveBehavior();
        }

        
    }


}
