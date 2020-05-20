using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPoint : MonoBehaviour
{

    public Encounter encounter;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player In Area");
            encounter.playerInArea = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player left Area");
            encounter.playerInArea = false;

        }
    }
}
