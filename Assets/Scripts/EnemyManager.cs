using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public List<Encounter> encounters;
    public float maxEncounterDistance;
   
    public bool weWon;

    public bool isInBattle;

    public bool inBattle;

    public bool WaveActive;

    /*Each group of enemies is handled by their own personal "Encounter" manager. The enemy manager handles the
    Global functions of managing the encounters themselves, disabling them and enabling them as required*/

    //List all the types of enemies we want to be able to manage

    // Start is called before the first frame update
    void Start()
    {
        weWon = false;

        player = GameObject.Find("Player");
        foreach (Encounter encounter in encounters)
        {
            encounter.enemyManager = this;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActiveEncounters();
    }

    public void UpdateActiveEncounters()
    {
        if (WaveActive == false)
        {
            foreach (Encounter encounter in encounters)
            {
                //If it's not cleared and we don't already have a wave going...
                if (encounter.cleared == false && encounter.gameObject.activeInHierarchy == false)
                {
                    //UI for "Wave" + encounter list.Count + 1
                    inBattle = true;
                    WaveActive = true;
                    encounter.gameObject.SetActive(true);
                    encounter.SpawnEnemies();

                    Entity.SetCurrentEncounter(encounter);
                    break;
                }
            }
        }
    }

    public void CheckVictory()
    {
        int numOfClearedEncounters = 0;

        foreach (Encounter encounter in encounters)
        {
            if (encounter.cleared == true)
            {
                numOfClearedEncounters++;
            }
        }
        if (numOfClearedEncounters == encounters.Count)
        {
            //You Won!
            Debug.Log("You win!");
            weWon = true;
        }
    }
    public void PlayerDeath()
    {
        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Enemy in EnemyList)
        {
            //stop attacking
            Enemy.GetComponent<SimpleEnemy>();
            
        }
   
    }
    public void ResetWave()
    {
        
        foreach (Encounter encounter in encounters)
        {
            if (encounter.gameObject.activeSelf)
            {
                encounter.ClearInitiativeList();
            }
        }
        


        GameObject[] EnemyList = GameObject.FindGameObjectsWithTag("Enemy");
        //kill all enemys
        foreach (GameObject Enemy in EnemyList)
        {
            Destroy(Enemy);
        }


        //spawn new enemy
        foreach (Encounter encounter in encounters)
        {
            if (encounter.gameObject.activeSelf)
            {
                encounter.SpawnEnemies();
            }
        }
    }

}
