using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeSinceStartUp : MonoBehaviour
{

    PauseAbility pause;
    private Text textNumbers;
    float timeNumbers;
    Color textColor;
    // Start is called before the first frame update
    void Start()
    {
        textNumbers = transform.GetComponent<Text>();
        pause = FindObjectOfType<PauseAbility>();
       // textColor = textNumbers;
    }

    // Update is called once per frame
    void Update()
    {

         
        
      

        timeNumbers = Time.timeSinceLevelLoad;
        textNumbers.text = timeNumbers.ToString();

         if (pause.takeingTurn == true)
          {
            textColor.a = 1.0f;
        }
       else if (pause.takeingTurn == false)
     {
            textColor.a = 0.0f;
        }

        textNumbers.color = textColor;
    }
}
