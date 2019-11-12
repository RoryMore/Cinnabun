using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
    PauseAbility pauseAbility;
    Player player;

    public Button PauseButton;
    public Button PlayButton;

    public Button RewindButtonBackground;
    public Button RewindButton;

    public Button DelayedBlastButtonBackground;
    public Button DelayedBlastButton;

    public Button TeleportBackground;
    public Image TeleportButton;

    public Image Health;
    public Image TurnCounter;
    // Start is called before the first frame update
    void Start()
    {
        pauseAbility = FindObjectOfType<PauseAbility>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAbility.states == PauseAbility.GameStates.TIMESTOP)
        {
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
        }
        if (pauseAbility.states != PauseAbility.GameStates.TIMESTOP)
        {
            PauseButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);
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
         TurnCounter.fillAmount = 1.0f - ( (float)pauseAbility.timeStopCoolDown / 4.0f);


    }
}
