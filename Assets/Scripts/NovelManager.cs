using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NovelManager : MonoBehaviour
{
    TextSystem textSystem;
    Player player;
	PauseAbility pause = null;
	EnemyManager enemyManager;
	PlayerInGameUI playerUI;
	PauseMenuUI pauseMenu;
	public GameObject visualNovel;



	//Triggers
	[HideInInspector]
	public bool Trigger1 = false;
	[HideInInspector]
	public bool Trigger2 = false;
	[HideInInspector]
	public bool Trigger3 = false;
	[HideInInspector]
	public bool Trigger4 = false;
	[HideInInspector]
	public bool Trigger5 = false;
	[HideInInspector]
	public bool Trigger6 = false;
	[HideInInspector]
	public bool Trigger7 = false;
	[HideInInspector]
	public bool Trigger8 = false;

	bool waveSpawn = false;
	bool on = false;

	bool story1 = false;

	int wavesCleared;

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
        TriggerBox();
		AcrossBridge();
		DidGetHit();
		NoMoreBattle();
		UsedRewind();
		UsedInventory();
		teleport();
		tutorialDone();
		waveCheck();
		Story();
	}



	void AcrossBridge()
	{
		if (Trigger1 == true)
		{
			if (playerUI.novelManager == true)
			{
				PopUpBox();
				playerUI.novelManager = false;
				Trigger7 = true;
			}
		}
	}

    void TriggerBox()
    {
        if (player.triggerBox == true || Input.GetKeyDown(KeyCode.L))
        {
			
			PopUpBox();
			Trigger1 = true;
        }
    }

	void DidGetHit()
	{
		if (Trigger7 == true)
		{
			TurnWalkOff();
			
			if (playerUI.healthDown == true)
			{
				if (Trigger2 == false)
				{
					//textSystem.novelActive = true;
					on = true;
					//TurnWalkOn();
					PopUpBox();
					Trigger2 = true;
					Trigger3 = true;
					//TurnWalkOn();
				
				}
			}
		}

	}

	void TurnWalkOff()
	{
		
		if (on == false)
		{
			textSystem.novelActive = false;
			
		}
	}

	void TurnWalkOn()
	{
		bool off = false;
		if (off == false)
		{
			textSystem.novelActive = true;
			off = true;
		}

	}

	void NoMoreBattle()
	{
		if (Trigger4 == true)
		{
			if (enemyManager.inBattle == false)
			{
				if (Trigger5 == false)
				{
					PopUpBox();
					Trigger5 = true;
				}
			}
		}
	}

	void UsedRewind()
	{
		if (Trigger3 == true )
		{
			if (playerUI.checkRewind == true)
			{
				PopUpBox();
				playerUI.checkRewind = false;
				Trigger4 = true;
			}
		}

	}

	void UsedInventory()
	{
		if (Trigger5 == true)
		{
			if (Trigger6 == false)
			{
				if (player.checkInventory == true)
				{
					PopUpBox();
					Trigger6 = true;
				}
			}
		}

	}

	void teleport()
	{
		if (Trigger6 == true)
		{
			if (waveSpawn == true)
			{
				PopUpBox();
			}
		}
	}

	void Story()
	{
		if (wavesCleared == 2)
		{
			if (story1 == false)
			{
				PopUpBox();

				story1 = true;
			}
		}
	}

	void tutorialDone()
	{
		if (player.tutorialDone == true)
		{
			SceneManager.LoadScene(0);
		}
	}



	void PopUpBox()
	{
		visualNovel.SetActive(true);
		textSystem.GameStart = true;
		textSystem.novelActive = false;
		player.triggerBox = false;
	}

	void waveCheck()
	{
		wavesCleared = enemyManager.numOfClearedEncounters + 1;
	}
}
