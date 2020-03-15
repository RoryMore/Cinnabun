using UnityEngine;
using UnityEngine.UI;

public class NameText : MonoBehaviour
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
        textColor = textSystem.nameTextColour;
;
        wordSize = textSystem.nameTextSize;

    }

    void Update()
    {
        changeFontSize();
    }

    //updates the font to what the user wants
    void changeFontSize()
    {
        text.fontSize = wordSize;
        text.color = textColor;
        text.font = textSystem.nameFont;

    }
}