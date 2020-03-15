using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractedPickup : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    [Tooltip("How close the Target has to be for the Attraction to take effect")]
    float attractionDistance;
    [SerializeField]
    float minDistance;

    [SerializeField]
    float maxTravelSpeed;
    [SerializeField]
    float acceleration;
    float currentSpeed;

    bool isAttracted;

    [Tooltip("If the target is within the minDistance")]
    public bool targetInRange;

    // Start is called before the first frame update
    void Start()
    {
        targetInRange = false;
        isAttracted = false;

        currentSpeed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttracted)
        {
            isAttracted = CheckTargetInAttractionDistance();
        }
        else
        {
            if (Vector3.Distance(target.position, gameObject.transform.position) <= minDistance)
            {
                targetInRange = true;
            }
            else
            {
                targetInRange = false;

                if (currentSpeed < maxTravelSpeed)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }

                transform.position = Vector3.Slerp(transform.position, target.position, currentSpeed * Time.deltaTime);
            }
        }
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    bool CheckTargetInAttractionDistance()
    {
        if (Vector3.Distance(target.position, gameObject.transform.position) <= attractionDistance)
        {
            return true;
        }
        return false;
    }
}
