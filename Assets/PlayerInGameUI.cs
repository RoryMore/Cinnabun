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
    public Button TeleportButton;
    // Start is called before the first frame update
    void Start()
    {
        pauseAbility = FindObjectOfType<PauseAbility>();
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

     
    }

    public void OnPaused()
    {
        pauseAbility.ButtonPaused();
    }

    public void OnPlay()
    {
        pauseAbility.ButtonPlay();
    }
}
