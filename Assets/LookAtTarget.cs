using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LookAtTarget : MonoBehaviour
{
    Camera camera;

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        transform.LookAt(camera.transform);
 
    }
}
