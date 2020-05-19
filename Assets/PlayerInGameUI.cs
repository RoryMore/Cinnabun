﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInGameUI : MonoBehaviour
{
    PauseAbility pauseAbility;

	NovelManager novelM;

    Player player;

    public Button PauseButton;
    public Button PlayButton;

    public Button RewindButtonBackground;
    public Image RewindButton;

    public Button DelayedBlastButtonBackground;
    public Image DelayedBlastButton;

    public Button TeleportBackground;
    public Image TeleportButton;

    public Image Health;
    public Image TurnCounter;

    public Button WeaponAttackButtonBackground;
    public Image MeleeAttack;

    public Image VHSimage;

    public Text timeSinceStartUp;

	public bool healthDown = false;

	public bool novelManager = false;

	public bool checkRewind = false;
	bool rewindOnce = false;
	bool doOnce = false;
	Scene currentScene = SceneManager.GetActiveScene();
	string sceneName;

	// Start is called before the first frame update
	void Start()
    {
        pauseAbility = FindObjectOfType<PauseAbility>();
      
        player = FindObjectOfType<Player>();

		novelM = FindObjectOfType<NovelManager>();

		Scene currentScene = SceneManager.GetActiveScene();
		sceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update()
    {
		CHeckHealthForNovel();
		tutorial();

        if (pauseAbility.states == PauseAbility.GameStates.TIMESTOP)
        {
			   PauseButton.gameObject.SetActive(false);
				PlayButton.gameObject.SetActive(true);
				RewindButtonBackground.interactable = true;
				DelayedBlastButtonBackground.interactable = true;
				TeleportBackground.interactable = true;
				WeaponAttackButtonBackground.interactable = true;
				VHSimage.gameObject.SetActive(true);
				timeSinceStartUp.gameObject.SetActive(true);


        }
        if (pauseAbility.states != PauseAbility.GameStates.TIMESTOP)
        {
            PauseButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
            RewindButtonBackground.interactable = false;
            DelayedBlastButtonBackground.interactable = false;
            TeleportBackground.interactable = false;
            WeaponAttackButtonBackground.interactable = false;
            VHSimage.gameObject.SetActive(false);
            timeSinceStartUp.gameObject.SetActive(false);
        }


        foreach (BaseSkill skill in player.skillList)
        {
            //if (skill.timeBeenOnCooldown < skill.cooldown)
            if (!skill.isAllowedToCast)
            {
                switch (skill.skillData.skill)
                {
                    case SkillData.SkillList.DELAYEDBLAST:
                        {
                            DelayedBlastButton.gameObject.SetActive(true);
                            DelayedBlastButton.fillAmount = 1.0f - (skill.timeBeenOnCooldown / skill.skillData.cooldown);
                            break;
                        }
                    case SkillData.SkillList.REWIND:
                        {
							if (rewindOnce == false)
							{
								checkRewind = true;
								rewindOnce = true;
							}
							RewindButton.gameObject.SetActive(true);
                            RewindButton.fillAmount = 1.0f - (skill.timeBeenOnCooldown / skill.skillData.cooldown);
                            break;
                        }
                    case SkillData.SkillList.TELEPORT:
                        {
                            TeleportButton.gameObject.SetActive(true);
                             TeleportButton.fillAmount = 1.0f - (skill.timeBeenOnCooldown / skill.skillData.cooldown);
                            break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                switch (skill.skillData.skill)
                {
                    case SkillData.SkillList.DELAYEDBLAST:
                        {

                            DelayedBlastButton.gameObject.SetActive(false);
                            break;
                        }
                    case SkillData.SkillList.REWIND:
                        {
							
                            RewindButton.gameObject.SetActive(false);
                            break;
                        }
                    case SkillData.SkillList.TELEPORT:
                        {
                            TeleportButton.gameObject.SetActive(false);
                            break;
                        }
                    default:
                        break;
                }
            }
            
        }

        //if (player.weaponAttack.timeBeenOnCooldown < player.weaponAttack.skillData.cooldown)
        if (!player.weaponAttack.isAllowedToCast)
        {
			if (doOnce == false)
			{
				novelManager = true;
				doOnce = true;
			}
            MeleeAttack.gameObject.SetActive(true);
            MeleeAttack.fillAmount = 1.0f - (player.weaponAttack.timeBeenOnCooldown / player.weaponAttack.skillData.cooldown);
        }
        else
        {
            MeleeAttack.gameObject.SetActive(false);
        }

        UpdateHealth();
        UpdateTurnCounter();
    }

    public void OnPaused()
    {
        pauseAbility.ButtonPaused();
    }

    public void OnPlay()
    {
        pauseAbility.ButtonPlay();
    }

    void UpdateHealth()
    {
        Health.fillAmount = (float)player.currentHP/(float)player.maxHP ;
    }

    void UpdateTurnCounter()
    {
        TurnCounter.fillAmount = 1.0f - ( (float)pauseAbility.timeStopCoolDown /2.0f);

    }

	void CHeckHealthForNovel()
	{
		if (player.currentHP < player.maxHP)
		{
			healthDown = true;
		}
	}

	void tutorial()
	{
		if (sceneName == "JasmineScene")
		{

			if (novelM != null)
			{
	
				if (novelM.Trigger1 == true)
				{
					player.attackSkill = true;
				}

				if (novelM.Trigger3 == true)
				{
					player.rewindSkill = true;
				}

				if (novelM.Trigger4 == true)
				{
					player.bombSkill = true;
				}
			}
		}
		if (sceneName != "JasmineScene")
		{
			player.attackSkill = true;
			player.bombSkill = true;
			player.rewindSkill = true;
			player.telepotSkill = true;
		}
	}
  
}
