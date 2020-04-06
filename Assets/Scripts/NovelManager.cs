using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelManager : MonoBehaviour
{
    TextSystem textSystem;
    Player player;
	PauseAbility pause = null;
	Encounter encounter;
	PlayerInGameUI playerUI;
	public GameObject visualNovel;
	public bool didPause = false;
	public bool wentAcrossBridge = false;
	public bool doOnce = false;
	bool checkHealthDown = false;
	bool didheal = false;

	// Start is called before the first frame update
	void Start()
    {
        textSystem = FindObjectOfType<TextSystem>();
        player = FindObjectOfType<Player>();
		pause = FindObjectOfType<PauseAbility>();
		playerUI = FindObjectOfType<PlayerInGameUI>();
		encounter = FindObjectOfType<Encounter>();

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

		if (encounter.cleared == true)
		{
			PopUpBox();
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
