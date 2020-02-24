using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeSinceStartUp : MonoBehaviour
{
    [SerializeField] private Transform damageNumbers;
    private TextMeshPro textNumbers;
    float timeNumbers;
    // Start is called before the first frame update
    void Start()
    {
        textNumbers = transform.GetComponent<TextMeshPro>();
       
    }

    // Update is called once per frame
    void Update()
    {
        timeNumbers = Time.timeSinceLevelLoad;

        textNumbers.SetText(timeNumbers.ToString());
    }
}
