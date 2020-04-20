using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelManager : MonoBehaviour
{
    TextSystem textSystem;
    Player player;
	PauseAbility pause = null;
	EnemyManager enemyManager;
	PlayerInGameUI playerUI;
	PauseMenuUI pauseMenu;
	public GameObject visualNovel;
	public bool didPause = false;
	public bool wentAcrossBridge = false;
	public bool doOnce = false;
	bool inventoryCheck = false;
	bool didheal = false;
	bool battleDone = false;

	// Start is called before the first frame update
	void Start()
    {
        textSystem = FindObjectOfType<TextSystem>();
        player = FindObjectOfType<Player>();
		pause = FindObjectOfType<PauseAbility>();
		playerUI = FindObjectOfType<PlayerInGameUI>();
		enemyManager = FindObjectOfType<EnemyManager>();
		pauseMenu = FindObjectOfType<PauseMenuUI>();

	}

    // Update is called once per frame
    void Update()
    {
        test();

		if (wentAcrossBridge == true)
		{
			if (playerUI.novelManager == true)
			{
				PopUpBox();
				playerUI.novelManager = false;
			}
		}

		
		if (playerUI.healthDown == true)
		{
			if (doOnce == false)
			{
				PopUpBox();
				doOnce = true;
				didheal = true;
			}
		}

		if (didheal == true)
		{
			if (playerUI.checkRewind == true)
			{
				PopUpBox();
				playerUI.checkRewind = false;
			}
		}


		if (didheal == true)
		{
			if (enemyManager.inBattle == false)
			{
				if (battleDone == false)
				{
					PopUpBox();
					battleDone = true;
				}
			}
		}

		if (battleDone == true)
		{

			if (inventoryCheck == false)
			{
				if (player.checkInventory == true)
				{
					PopUpBox();
					inventoryCheck = true;
				}
			}
		} 


	}

    void test()
    {
        if (player.triggerBox == true || Input.GetKeyDown(KeyCode.L))
        {
			
			PopUpBox();
			wentAcrossBridge = true;
        }
    }

	void PopUpBox()
	{
		visualNovel.SetActive(true);
		textSystem.GameStart = true;
		textSystem.novelActive = false;
		player.triggerBox = false;
	}
}
