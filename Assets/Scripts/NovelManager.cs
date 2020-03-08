using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelManager : MonoBehaviour
{
    TextSystem textSystem;
    Player player;
    public GameObject visualNovel;
  
    // Start is called before the first frame update
    void Start()
    {
        textSystem = FindObjectOfType<TextSystem>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        test();

  
    }

    void test()
    {
        if (player.triggerBox == true)
        {
           
            visualNovel.SetActive(true);
            textSystem.GameStart = true;
            textSystem.novelActive = false;
            player.triggerBox = false;
        }
    }
}
