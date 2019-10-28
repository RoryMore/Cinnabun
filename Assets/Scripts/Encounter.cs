using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{

    public Entity enemy1;

    public List<Entity> masterInitiativeList;
    public List<Entity> initiativeList;
    public List<Entity> healList;

    public List<GameObject> spawnPoints;

    public EnemyManager enemyManager;

    public bool cleared = false;

    


    // Start is called before the first frame update
    void Start()
    {
        //Spawn enemies
        foreach (GameObject location in spawnPoints)
        {

            if (location.name.Contains("Enemy1"))
            {
                initiativeList.Add(Instantiate(enemy1, location.transform));
                
            }
            else if (location.name.Contains("Enemy2"))
            {

            }
            else if (location.name.Contains("Enemy3"))
            {

            }
        }

        masterInitiativeList.AddRange(initiativeList);
        
    }

    void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {
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
        enemyManager.CheckVictory();
        GiveItem();
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
    }


}
