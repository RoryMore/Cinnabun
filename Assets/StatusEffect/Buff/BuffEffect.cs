using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffect: MonoBehaviour
{
    public Buff[] BuffEffects;
    private int checkLoctation;

    // Start is called before the first frame update
    public void applyEffects(Entity entity, Buff.EffectApplyType effectApply)
    {
        foreach (Buff item in BuffEffects)
        {
           
            //check to see if this is the right time to apply these condation to entity
            if (effectApply == item.EfectApplyWhen)
            {
                //
                if (CheckToContrion(entity, item))//enetity is already in that condition
                {
                    //add condition to entity basiced on if it is percent damage
                    //entity.currentBufConditions.Add(new Entity.ConditionBuf(item.Duration, item.BuffType, item.buffnumber, item.BuffStat.ToString()));
                    Debug.LogWarning(entity.name + " was hit with a buff to " + item.BuffType);
                    entity.AddCurrentBuf(item.Duration, item.BuffType, item.buffnumber, item.BuffStat.ToString());
                }
                else
                {
                    //set duration to need effect duration if lower
                    if (entity.currentBufConditions[checkLoctation].duration < item.Duration)//Duration of the contration vs 
                    {
                        var temp = entity.currentEffConditions[checkLoctation];
                        temp.duration = item.Duration;
                        entity.currentEffConditions[checkLoctation] = temp;
                    }
                }
            }
        }
    }

    //check to see if the entity alreadys has that condition 
    public bool CheckToContrion(Entity entity, Buff efect)
    {
        int StoreLoc = 0;
        if (entity)
        {
            if (entity.currentEffConditions.Count != 0)
            {
                foreach (var item in entity.currentBufConditions)
                {
                    if (item.conditionType == efect.BuffType)
                    {
                        checkLoctation = StoreLoc;
                        return false;
                    }
                    StoreLoc++;
                }
            }
        }
        return true;
    }
}
