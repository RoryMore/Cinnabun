//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class status : MonoBehaviour
//{
//    struct StaticEffect
//    {
//        string name;
//        float ActiveCooldown;// cooldown used when statics is active. -2 when in active;
//        float cooldown;// cooldown 
//        //effect;

//        void ReduceCooldown()
//        {
//            cooldown -= Time.deltaTime;
//        }
//    }

//    List<StaticEffect> ActiveEffect = new List<StaticEffect>();
//    List<StaticEffect> PassiveEffect = new List<StaticEffect>();

//    void FixedUpdate()
//    {
//        foreach (var StaticEffect in ActiveEffect)
//        {
//            ReduceCooldown();
//            if (cooldown<=0)
//            {
//                //reomve StaticEffect
//            }
//        }
//    }

//    void AddPassiveEffect(string name, float ActiveCooldown, float cooldown)
//    {

//    }
//}
