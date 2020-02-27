using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Basic Self-Rotation Var")]
    [SerializeField]
    Vector3 rotationSpeed;

    [Header("Rotation Around Point Variables")]
    [SerializeField]
    bool rotateAroundPoint;
    [SerializeField]
    float rotateAroundSpeed;
    [SerializeField]
    Vector3 rotateAroundAxis;
    [SerializeField]
    Transform rotatedAround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateAroundPoint)
        {
            transform.RotateAround(rotatedAround.position, rotateAroundAxis, rotateAroundSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}
