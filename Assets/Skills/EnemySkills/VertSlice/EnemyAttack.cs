using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Ability
{
    [Tooltip("The width a straight attack will have, damaging enemies in an area based on width and range")]
    public float attackWidth;     // effective width of the ability in which it can damage enemies 
    //making a 'hit rectangle' with range and attackWidth

    public enum AttackType
    {
        Forward,
        Cone
    };

    public AttackType attackType;

    [Tooltip("The half the angle that a conal attack will hit, damaging enemies in an area based on angle and range. [180deg is full circle]")]
    public float angle;

    ConeRangeIndicator coneRangeIndicator = null;
    RectangleRangeIndicator rectangleRangeIndicator = null;

    // Start is called before the first frame update
    void Start()
    {
        //line = GetComponent<LineRenderer>();




        // If there were to be diff equipped weapons
        //GameObject pRef = GameObject.FindGameObjectWithTag("Player");
        //magnitude = pRef.equippedWep.damage;
        //type = Type.WeaponAttack;
        //attackType = AttackType.Cone;

        //actionSpeed = 2.0f;
        //range = 5.0f;
        //magnitude = 20.0f;

        turnsBeenOnCooldown = cooldown;

        turnsBuffed = 0;
        isBuffActive = false;

        turnsDebuffed = 0;
        isDebuffActive = false;

        switch (attackType)
        {
            case AttackType.Cone:
                coneRangeIndicator = transform.GetComponent<ConeRangeIndicator>();
                if (coneRangeIndicator == null)
                {
                    Debug.LogAssertion("coneRangeIndicator failed to be set");
                }
                coneRangeIndicator.Init(angle);
                break;

            case AttackType.Forward:

                rectangleRangeIndicator = transform.GetComponent<RectangleRangeIndicator>();
                if (rectangleRangeIndicator == null)
                {
                    Debug.LogAssertion("rectangleRangeIndicator failed to be set");
                }
                rectangleRangeIndicator.Init();

                break;
            default:
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawRangeIndicator()
    {
        if (attackType == AttackType.Cone)
        {
            coneRangeIndicator.DrawIndicator(angle, 0.0f, range);
        }
        else if (attackType == AttackType.Forward)
        {
            rectangleRangeIndicator.DrawIndicator(attackWidth, 0.0f, range);
        }
    }

    public void DrawCastTimeRangeIndicator(float timeSpentCasting)
    {
        //float drawPercentage = (timeSpentCasting / actionSpeed) * range;
        if (attackType == AttackType.Cone)
        {
            float drawPercentage = (timeSpentCasting / actionSpeed) * range;

            coneRangeIndicator.DrawIndicator(angle, 0.0f, range);
            coneRangeIndicator.DrawCastTimeIndicator(angle, 0.0f, drawPercentage);
        }
        else if (attackType == AttackType.Forward)
        {
            float drawPercentage = (timeSpentCasting / actionSpeed);

            rectangleRangeIndicator.DrawIndicator(attackWidth, 0.0f, range);
            rectangleRangeIndicator.DrawCastTimeIndicator(attackWidth, 0.0f, drawPercentage, range);
        }
    }

    public bool ShouldEnemyInPositionBeDamaged(Vector3 position)
    {
        // Depending on our 'attackType' we calculate hits slightly differently
        if (attackType == AttackType.Cone)
        {

            float forwardAngle = 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x);

            float positionAngle = Vector3.Angle(position - transform.position, transform.forward);
            float distance = Vector3.Distance(position, transform.position);

            if (positionAngle <= angle)
            {
                if (distance <= range)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        else if (attackType == AttackType.Forward)
        {
            //Debug.Log("Player calculating if attack damages with Forward!");
            Vector3 meshYPosition = new Vector3(position.x, rectangleRangeIndicator.mesh.bounds.center.y, position.z);
            if (rectangleRangeIndicator.mesh.bounds.Contains(meshYPosition))
            {
                //Debug.Log("Straight Attack DAMAGED THEM!!!!!!!!!");
                return true;
            }
            else
            {
                return false;
            }
        }
        Debug.Log("PlayerAttackScript: This Return path shouldn't be reached. Didn't discern whether enemy should be damaged or not");
        return false;
    }

    /*void DrawLineAtAngleFor(float givenAngle, ref float seconds)
    {
        seconds -= Time.fixedDeltaTime;

        // Draw Line
        Vector3[] tempPositions = new Vector3[line.positionCount];
        tempPositions[0] = transform.position;
        float x = (range * 2) * Mathf.Cos(givenAngle) + transform.position.x;
        float z = (range * 2) * Mathf.Sin(givenAngle) + transform.position.z;
        float y = transform.position.y;
        tempPositions[1] = new Vector3(x, y, z);

        line.SetPositions(tempPositions);

        if (seconds > 0.0f)
        {
            DrawLineAtAngleFor(givenAngle, ref seconds);
        }
    }*/
}
