using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEstSkill : EnemyScript
{
    public BaseSkill basicAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        myEncounter = transform.parent.parent.GetComponent<Encounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (basicAttack.isAllowedToCast)
        {
            basicAttack.TriggerSkill(myEncounter.playerInclusiveInitiativeList);
        }
    }
}
