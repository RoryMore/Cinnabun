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
	public bool Trigger1 = false;
	public bool Trigger2 = false;
	public bool Trigger3 = false;
	public bool Trigger4 = false;
	public bool Trigger5 = false;
	public bool Trigger6 = false;
	public bool Trigger7 = false;
	public bool Trigger8 = false;

	bool on = false;

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
		//PlayerDead();
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




	void PopUpBox()
	{
		visualNovel.SetActive(true);
		textSystem.GameStart = true;
		textSystem.novelActive = false;
		player.triggerBox = false;
	}
}
