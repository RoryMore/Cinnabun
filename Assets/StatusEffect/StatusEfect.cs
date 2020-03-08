using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEfect : MonoBehaviour
{
    public Effects[] StatusEffects;
    private int checkLoctation;

    public void applyEffects(Entity entity, Effects.EffectApplyType effectApply)
    {
        foreach (Effects item in StatusEffects)
        {
            //check to see if this is the right time to apply these condation to entity
            if (effectApply == item.EfectApplyWhen)
            {
               //
                if (CheckToContrion(entity, item))//enetity is already in that condition
                {
                    //add condition to entity basiced on if it is percent damage
                        if (item.BuffType == Effects.buffeType.Percent)
                        {
                        entity.currentConditions.Add(new Entity.Condition(item.Duration, item.Effect, item.Damage, 0, item.TickDamage));
                        }
                        else
                        {
                        entity.currentConditions.Add(new Entity.Condition(item.Duration, item.Effect, 0, item.Damage, item.TickDamage));
                        }             
                }
                else
                {
                    //set duration to need effect duration if lower
                    if (StatusEffects[checkLoctation].Duration< item.Duration)//Duration of the contration vs 
                    {
                        var temp = entity.currentConditions[checkLoctation];
                        temp.duration = item.Duration;
                        entity.currentConditions[checkLoctation] = temp;
                    }
                }
            }
        }
    }

    //check to see if the entity alreadys has that condition 
    public bool CheckToContrion(Entity entity, Effects efect)
{
        int Cyle=0;
    foreach (var item in entity.currentConditions)
    {
        if (item.conditionType == efect.Effect)
        {
                checkLoctation = Cyle;
            return true;
        }
            Cyle++;
    }
    return false; 
}
    public void RemoveEffect(Entity entity, Effects effectApply)
    {
        CheckToContrion(entity, effectApply);

        foreach (Entity.Condition item in entity.currentConditions)
        {
            if (effectApply.Effect == item.conditionType)
            {
                if (effectApply.permanent == item.permanent)
                {
                    entity.currentConditions.Remove(item);
                    return;
                }
            }
        }

    Debug.LogError("Couldn't Remove Effect: "+ effectApply.Effect);
       
    }
}
