using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnControl : MonoBehaviour
{
    Player player;
    Transform respawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        respawnPoint = GameObject.FindGameObjectWithTag("MainRespawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead)
        {
            FadeUI.FadeIn();

            if (FadeUI.fadeInComplete)
            {
                player.transform.position = respawnPoint.position;
                player.Revive();
            }
        }
        else
        {
            FadeUI.FadeOut();
        }
    }
}
