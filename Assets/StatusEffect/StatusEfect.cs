using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEfect : MonoBehaviour
{
    public Effects[] StatusEffects;
    private int checkLoctation;

    public void applyEffects(Entity entity, Effects.EffectApplyType effectApply)
    {
        Debug.LogWarning("called");
        if (entity ==null)
        {
            return;
        }
        foreach (Effects item in StatusEffects)
        {
            if (effectApply == item.EfectApplyWhen)
            {
                if (CheckToContrion(entity, item))//enetity is already in that condition
                {
                    //Debug.LogWarning(effectApply == item.EfectApplyWhen);
                    //check to see if this is the right time to apply these condation to entity


                    //set duration to need effect duration if lower
                    Debug.LogWarning("reset");
                    if (StatusEffects[checkLoctation].Duration < item.Duration)//Duration of the contration vs 
                    {
                        var temp = entity.currentEffConditions[checkLoctation];
                        temp.duration = item.Duration;
                        entity.currentEffConditions[checkLoctation] = temp;
                    }

                }
                else
                { //add condition to entity basiced on if it is percent damage

                    if (item.EffectType == Effects.EffecteType.Percent)
                    {
                        //Debug.LogWarning("Percent");
                        Debug.LogWarning(entity.name);
                        entity.currentEffConditions.Add(new Entity.ConditionEff(item.Duration, item.Effect, item.Damage));
                    }
                    else
                    {
                        //Debug.LogWarning("numbe");
                        //entity.currentEffConditions.Add(new Entity.ConditionEff(item.Duration, item.Effect, item.Damage, item.TickDamage));
                        Debug.LogWarning(entity.name + "and" + effectApply);
                        entity.AddCurrentEff(item.Duration, item.Effect, item.TickDamage);
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
                    //condition is on entity
                    if (item.conditionType == efect.Effect)
                    {
                       checkLoctation = Cyle;
                        return true;
                    }
                    Cyle++;
                }
            }
        }
       
    return false; 
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
