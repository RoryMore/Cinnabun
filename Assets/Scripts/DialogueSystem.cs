using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DialogueSystem : MonoBehaviour
{

    public static DialogueSystem instance;
    TextSystem textSystem;

    public ELEMENTS elements;

	public string speakerNameHold;


    // public Text test;



    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        textSystem = TextSystem.instance;
    }

    // Update is called once per frame
    void Update()
    {
        Catch();
    }
    //starts the coroutine to write the text.
    public void Say(string speech, string speaker = "")
    {
        StopSpeaking();
        speaking = StartCoroutine(Speaking(speech));
    }

    [HideInInspector] public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWatingForUserInput = false;
    [HideInInspector] public float waitfor;

    Coroutine speaking = null;

    //checks if the coroutine is still wrting out the text or not
    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
            
        }
        speaking = null;     
    }

    //This get the text pannel and writes the text to it 
    IEnumerator Speaking(string targetSpeech)
    {
        speechPanel.SetActive(true);
        speechText.text = "";
        speakerNameText.text = DetermineSpeaker();
        isWatingForUserInput = false;

        while (speechText.text != targetSpeech)
        {
            speechText.text += targetSpeech[speechText.text.Length];
            yield return new WaitForSeconds(waitfor);

        }

        textSystem.index++;
        isWatingForUserInput = true;
        while (isWatingForUserInput)
        yield return new WaitForEndOfFrame();

        StopSpeaking();    
    }

    //checks if the text is currently being written out then if it isnt will write out all the text at once
    public void SkipTextScroll(string targetSpeech, string speaker = "")
    {
        if (speaking != null)
        {
            speechText.text = targetSpeech;
            StopCoroutine(speaking);
            isWatingForUserInput = true;
        }
    }

    //Determins who is speaking
    string DetermineSpeaker()
    {
       speakerNameHold = textSystem.text[textSystem.index].CharacterName;

        return speakerNameHold;
    }

    //make sure the text does not skip it self.
    void Catch()
    {
        if (isWatingForUserInput == true)
        {
            textSystem.userInput = false;
        }
    }

    //gets the text object elements
    [System.Serializable]
    public class ELEMENTS
    {
        public GameObject speechPanel;
        public Text speakerNameText;
        public Text speechText;

    }
    public GameObject speechPanel { get { return elements.speechPanel; } }
    public Text speakerNameText { get { return elements.speakerNameText; } }
    public Text speechText { get { return elements.speechText; } }
}
