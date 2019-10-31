using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public List<Encounter> encounters;
    public float maxEncounterDistance;

    public bool inBattle;

    /*Each group of enemies is handled by their own personal "Encounter" manager. The enemy manager handles the
    Global functions of managing the encounters themselves, disabling them and enabling them as required*/
   

    //List all the types of enemies we want to be able to manage

    
    // Start is called before the first frame update
    void Start()
    {
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
        foreach (Encounter encounter in encounters)
        {
            float encounterDistance = Vector3.Distance(encounter.gameObject.transform.position, player.transform.position);
            if (encounterDistance > maxEncounterDistance) //Magic number, it really only needs to be the distance that covers the maximum zoo
            {
                if (encounter.gameObject.activeInHierarchy == true)
                {
                    encounter.gameObject.SetActive(false);
                    inBattle = false;
                }
            }
            else
            {
                if (encounter.gameObject.activeInHierarchy == false)
                {
                    encounter.gameObject.SetActive(true);
                    inBattle = true;
                    player.GetComponent<Player>().SetCurrentEncounter(encounter);
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
        }

    }

}
