using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelManager : MonoBehaviour
{
    TextSystem textSystem;
    public GameObject visualNovel;
    // Start is called before the first frame update
    void Start()
    {
        textSystem = FindObjectOfType<TextSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        test();
    }

    void test()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            visualNovel.SetActive(true);
            textSystem.GameStart = true;
        }
    }
}
