using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEfect : MonoBehaviour
{
    public Effects[] StatusEffects;
    private int checkLoctation;

    public void applyEffects(Entity entity, Effects.EffectApplyType effectApply)
    {
        
        if (entity ==null)
        {
            return;
        }  
        foreach (Effects item in StatusEffects)
        {
            //check to see if this is the right time to apply these condation to entity
            if (effectApply == item.EfectApplyWhen)
            {
                if (CheckToContrion(entity, item))//enetity is already in that condition
                {
                    //add condition to entity basiced on if it is percent damage
                    if (item.BuffType == Effects.buffeType.Percent)
                        {
                        Debug.LogWarning("Percent");
                        entity.currentEffConditions.Add(new Entity.ConditionEff(item.Duration, item.Effect, item.Damage));
                    }
                        else
                        {
                        //entity.currentEffConditions.Add(new Entity.ConditionEff(item.Duration, item.Effect, item.Damage, item.TickDamage));
                        entity.AddCurrentEff(item.Duration, item.Effect, item.Damage);
                    }
                }
                else
                {
                    //set duration to need effect duration if lower
                  //  Debug.LogWarning("reset");
                    if (StatusEffects[checkLoctation].Duration< item.Duration)//Duration of the contration vs 
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
    public bool CheckToContrion(Entity entity, Effects efect)
{
        int Cyle=0;
        if (entity)
        {
            if (entity.currentEffConditions.Count != 0)
            {
                foreach (var item in entity.currentEffConditions)
                {
                    if (item.conditionType == efect.Effect)
                    {
                        checkLoctation = Cyle;
                        return false;
                    }
                    Cyle++;
                }
            }
        }
       
    return true; 
}
    //public void RemoveEffect(Entity entity, Effects effectApply)
    //{
    //    CheckToContrion(entity, effectApply);

    //    foreach (Entity.Condition item in entity.currentEffConditions)
    //    {
    //        if (effectApply.Effect == item.conditionType)
    //        {
    //            if (effectApply.permanent == item.permanent)
    //            {
    //                entity.currentConditions.Remove(item);
    //                return;
    //            }
    //        }
    //    }
    //Debug.LogError("Couldn't Remove Effect: "+ effectApply.Effect);
       
    //}
}
