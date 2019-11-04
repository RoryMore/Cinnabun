using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    Image fadeImage;
    Color fadeColour;

    static bool doFade;    // True fades in. False fades out
    public static bool fadeInComplete;
    public static bool fadeOutComplete;

    [Tooltip("How long the fade will complete, in seconds")]
    public float timeToCompleteFadeIn;
    [Tooltip("How long the fade will complete, in seconds")]
    public float timeToCompleteFadeOut;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        fadeColour = fadeImage.color;
        fadeColour.a = 0.0f;

        fadeImage.color = fadeColour;
        
        doFade = false;

        fadeInComplete = false;
        fadeOutComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        ControlActiveFade(doFade);
    }

    void ControlActiveFade(bool fade)
    {
        if (fade)
        {
            if (fadeColour.a < 1.25f)
            {
                fadeColour.a += (Time.deltaTime / timeToCompleteFadeIn);
            }
            else if (fadeColour.a >= 1.0f)
            {
                fadeInComplete = true;
            }
        }
        else
        {
            if (fadeColour.a > 0.0f)
            {
                fadeColour.a -= (Time.deltaTime / timeToCompleteFadeOut);
            }
            else
            {
                fadeOutComplete = true;
            }
        }
        fadeImage.color = fadeColour;
    }

    public static void FadeIn()
    {
        if (!doFade)
        {
            doFade = true;
            fadeInComplete = false;
        }
    }

    public static void FadeOut()
    {
        if (doFade)
        {
            doFade = false;
            fadeOutComplete = false;
        }
    }
}
