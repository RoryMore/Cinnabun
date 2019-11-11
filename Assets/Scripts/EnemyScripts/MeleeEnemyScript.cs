//using system.collections;
//using system.collections.generic;
//using unityengine;
//using unityengine.ai;

//public class meleeenemyscript : enemyscript
//{

//    public float meleeattackrange;
//    public int meleedamage;

//    playerattack ourattack;
//    bool isattacking = false;

//    void awake()
//    {
//        anim = getcomponent<animator>();
//        //enemyaudio = getcomponent<audiosource>();
//        hitparticles = getcomponent<particlesystem>();

//        nav = getcomponent<navmeshagent>();


//        player = gameobject.find("player").getcomponent<playerscript>();

//        turnmanger = gameobject.find("turnmanager").getcomponent<turnmanagescript>();
//        enemymanager = gameobject.find("enemymanager").getcomponent<enemymanager>();

//        enemycooldown = 6.0f;
//        //enemycooldown = 2.0f + random.range(1.0f, 4.0f);
//        initiativespeed = 1.5f;
//        currenthealth = startinghealth;


//        ourattack = getcomponent<playerattack>();

//    }


//    void update()
//    {
//        if (isdead != true)
//        {
//            if (turnmanger.state == turnmanagescript.battlestate.battle || turnmanger.state == turnmanagescript.battlestate.action)
//            {
//                movement();
//                meleeattack();
//                turn();
//            }
//            if (input.getkeydown("g") == true)
//            {

//                takedamage(10, transform.position);
//            }
//        }

//    }


//    public void movement()
//    {
//        if (currenthealth > 0 && player.currenthealth > 0)
//        {

//            //if we're close enough to smack, stop moving
//            if (vector3.distance(transform.position, player.gameobject.transform.position) < meleeattackrange)
//            {
//                nav.setdestination(transform.position);
//                //transform.lookat(player.transform);
//                facetarget(player.transform);
//                anim.setbool("iswalking", false);

//            }
//            else
//            {
//                //gameobject.find("player")
//                nav.setdestination(player.transform.position);
//                anim.setbool("iswalking", true);

//            }
//            //assuming we arent, get closer
//        }
//        else
//        {
//            nav.enabled = false;
//            anim.setbool("iswalking", false);
//        }
//    }

//    //temporary function for when jasmine finishes her turn counter
//    public void turn()
//    {
//        enemycooldown -= 1f * time.deltatime;
//        //debug.log("enemy cooldown counter: " + enemycooldown);

//    }

//    public void meleeattack()
//    {

//        float distance = vector3.distance(transform.position, player.transform.position);

//        //we are ready to make our attack, and we are in range. attack!
//        if (distance <= meleeattackrange && enemycooldown <= 0.0f)
//        {

//        }
//        if (isattacking == true)
//        {
//            anim.setbool("isattacking", true);
//            timespentdoingaction += time.fixeddeltatime;

//            ourattack.drawcasttimerangeindicator(timespentdoingaction);

//            if (timespentdoingaction >= ourattack.actionspeed)
//            {
//                if (ourattack.shouldenemyinpositionbedamaged(player.transform.position) == true)
//                {
//                    player.takedamage(meleedamage);
//                }

//                //play animation
//                enemycooldown = 6.0f;
//                timespentdoingaction = 0.0f;
//                anim.setbool("isattacking", false);
//                //anim.setbool("isattacking", false);
//            }
//        }
//        //debug.log("attack!");
//        //if its the melee enemy turn but we are out of range, we go into defence stance!
//        else if (meleeattackrange <= distance && enemycooldown <= 0.0f)
//        {
//            holdturn();
//            //debug.log("she's too far!");
//        }
//        else if (meleeattackrange <= distance && 0.0f <= enemycooldown)
//        {
//            //debug.log("");
//        }
//    }

//}