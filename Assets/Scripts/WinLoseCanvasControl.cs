using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseCanvasControl : MonoBehaviour
{
    [SerializeField]
    Image blackFade;
    [SerializeField]
    Image loseBGImage;
    [SerializeField]
    Image loseTextImage;
    [SerializeField]
    Text winText;

    Player player;
    EnemyManager enemyManager;

    public bool gameLost;
    public bool gameWon;

    float winAlpha;
    float loseAlpha;

    public bool loseAlphaFull;
    public bool winAlphaFull;

    // Start is called before the first frame update
    void Start()
    {
        winAlpha = 0.0f;
        loseAlpha = 0.0f;

        Color alphaColour = winText.color;
        alphaColour.a = 0.0f;
        winText.color = alphaColour;

        alphaColour = blackFade.color;
        alphaColour.a = 0.0f;
        blackFade.color = alphaColour;

        alphaColour = loseBGImage.color;
        alphaColour.a = 0.0f;
        loseBGImage.color = alphaColour;

        alphaColour = loseTextImage.color;
        alphaColour.a = 0.0f;
        loseTextImage.color = alphaColour;
    }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();

        gameLost = false;
        gameWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWinConditions();

        if (gameWon)
        {
            if (winAlpha < 2.0f)
            {
                winAlpha += Time.deltaTime;

                if (winAlpha < 1.0f)
                {
                    Color alphaColour = winText.color;
                    alphaColour.a = winAlpha;
                    winText.color = alphaColour;

                    alphaColour = blackFade.color;
                    alphaColour.a = winAlpha;
                    blackFade.color = alphaColour;
                }
            }
            else
            {
                winAlphaFull = true;
            }
        }
        else if (gameLost)
        {
            if (loseAlpha < 2.0f)
            {
                loseAlpha += Time.deltaTime;

                if (loseAlpha < 1.0f)
                {
                    Color alphaColour = blackFade.color;
                    alphaColour.a = loseAlpha;
                    blackFade.color = alphaColour;

                    alphaColour = loseBGImage.color;
                    alphaColour.a = loseAlpha;
                    loseBGImage.color = alphaColour;

                    alphaColour = loseTextImage.color;
                    alphaColour.a = loseAlpha;
                    loseTextImage.color = alphaColour;
                }
            }
            else
            {
                loseAlphaFull = true;
            }
        }
        else
        {
            if (loseAlpha > 0.0f)
            {
                loseAlpha -= Time.deltaTime;

                Color alphaColour = blackFade.color;
                alphaColour.a = loseAlpha;
                blackFade.color = alphaColour;

                alphaColour = loseBGImage.color;
                alphaColour.a = loseAlpha;
                loseBGImage.color = alphaColour;

                alphaColour = loseTextImage.color;
                alphaColour.a = loseAlpha;
                loseTextImage.color = alphaColour;
            }
            else
            {
                loseAlphaFull = false;
            }
            if (winAlpha > 0.0f)
            {
                winAlpha -= Time.deltaTime;

                Color alphaColour = winText.color;
                alphaColour.a = winAlpha;
                winText.color = alphaColour;

                alphaColour = blackFade.color;
                alphaColour.a = winAlpha;
                blackFade.color = alphaColour;
            }
            else
            {
                winAlphaFull = false;
            }
        }
    }

    void UpdateWinConditions()
    {
        gameLost = player.isDead;
        // Set gameWon based on enemyManager bool
        gameWon = enemyManager.weWon;
    }
}
