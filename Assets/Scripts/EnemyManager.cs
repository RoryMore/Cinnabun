using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public List<Encounter> encounters;
    public float maxEncounterDistance;

    public bool weWon;

    public bool inBattle;

    public bool WaveActive;

    float waveCooldownTimer;
    public float timeBetweenWaves;

    /*Each group of enemies is handled by their own personal "Encounter" manager. The enemy manager handles the
    Global functions of managing the encounters themselves, disabling them and enabling them as required*/

    //List all the types of enemies we want to be able to manage

    // Start is called before the first frame update
    void Start()
    {
        weWon = false;
        waveCooldownTimer = 0.0f;

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
        if (WaveActive == false)
        {
            waveCooldownTimer -= Time.deltaTime;
        }
    }

    public void UpdateActiveEncounters()
    {
        if (WaveActive == false && waveCooldownTimer <= 0.0f)
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

    public void SetTimeToNextWave(float timer)
    {
        waveCooldownTimer = timer;
    }
}
