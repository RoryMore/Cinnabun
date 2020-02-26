using UnityEngine;
using UnityEngine.UI;

public class SpeechText : MonoBehaviour
{
    TextSystem textSystem;
    Text text;
    RectTransform recTransform;
    Color textColor;
    int wordSize;
    float lineSpacing;

    void Start()
    {
      
        //Fetch the Text and RectTransform components from the GameObject
        text = GetComponent<Text>();
        recTransform = GetComponent<RectTransform>();
        textSystem = TextSystem.instance;

        wordSize = textSystem.textSize;
        lineSpacing = textSystem.lineSpacing;
        textColor = textSystem.textColour;




    }

    void Update()
    {
        changeFontSize();
    }
    //changes the font to what the user wants
    void changeFontSize()
    {
        text.fontSize = wordSize;
        text.lineSpacing = lineSpacing;
        text.color = textColor;
        text.font = textSystem.font;

    }
}