using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] UI;
    bool gameStart;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < UI.Length; i++)
        {
            UI[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart == true)
        {
            for (int i = 0; i < UI.Length; i++)
            {
                UI[i].SetActive(true);
            }
            gameStart = false;
        }
    }
}
