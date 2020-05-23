using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TextSystem : MonoBehaviour
{
    public static TextSystem instance;
    SpeechText speechText;
    DialogueSystem dialogue;
	EnemyManager enemyManager;
	NovelManager novelManager;
	GameObject[] playerUI;

	[Header("Text Settings")]
    public Font font;
    [Tooltip("The time before the user can skip the text.")]
    [Range(0.2f, 1f)] public float WaitBeforeSkip;
    [Tooltip("The size the speaker text will appear.")]
    [Range(1f, 30f)] public int textSize;
    [Tooltip("How far apart the lines will appear from each other.")]
    [Range(1f, 10f)] public float lineSpacing;
    [Tooltip("How far fast the text will appear.")]
    [Range(0.01f, 1f)] public float textSpeed;
    [Tooltip("The sound for when a button is hit.")]
    public AudioSource textSound;
    public Color textColour;


    [Header("Name Text Settings")]
    public Font nameFont;
    [Tooltip("The size of the speakers name")]
    [Range(1f, 30f)] public int nameTextSize;
    public Color nameTextColour;

	[Header("Scene Management")]
	[Space(10)]
    [Tooltip("If checked at the end of all the text take user to next scene")]
    public bool GoToNextScene;
    [Tooltip("Which scene to go to")]
    public int sceneNumber;

	[System.Serializable]
	public struct Text
        {
        public string CharacterName;
		public string BackgroundName;
        [System.Serializable]
        public enum Emotion
        {
            Happy,
            Sad,
            Nutrual,
            Angry
        };
		public Emotion emotion;
		[Space(10)]
        [TextArea(10, 20)]
        public string[] text;
        }

	//[Header("Script Settings")]
	[Space(10)]
	public Text[] text;
	public int wordIndex = 0;
	public bool novelActive;
	[System.Serializable]
	public struct Characters
	{
		public string characterName;
		public GameObject Happy;
		public GameObject Sad;
		public GameObject Nutral;
		public GameObject Angry;
	}
	//[Header("Characters")]
	[Space(10)]
	public Characters[] characters;

	[System.Serializable]
	public struct BackgroundImage
	{
		public string backgroundName;
		public GameObject backgroundImage;
		//public int textLineNumber;
	}
	//[Header("Background")]
	[Space(10)]
	public BackgroundImage[] background;

	public bool GameStart = true;
	float waitTime = 0.0f;
	[HideInInspector] public string backgroundName;
	[HideInInspector] public int index = 0;
	public bool hideNovel = false;
	public bool userInput = false;

	public GameObject visualNovel;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		dialogue = DialogueSystem.instance;
		speechText = GetComponent<SpeechText>();
		novelManager = GetComponent<NovelManager>();
		playerUI = GameObject.FindGameObjectsWithTag("PlayerUI");
		enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
		dialogue.waitfor = textSpeed;
	}

	void Update()
    {
		
		gameStart();
		//sayPaused(text[wordIndex].text[index]);
		
		

		if (dialogue != null)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0))) && userInput == false)
            {

                if (!dialogue.isSpeaking || dialogue.isWatingForUserInput)
                {
                    if (index >= text[wordIndex].text.Length)
                    {
						enemyManager.enemyMangerCurrentEncounter.SetActiveBehavior(true);
						hideNovel = true;
						Debug.Log("Text,Done");
						novelActive = true;
						wordIndex++;
						index = 0;
						foreach (GameObject g in playerUI)
						{
							g.SetActive(true);
						}
					
						LoadScene(sceneNumber);
						visualNovel.SetActive(false);						
					
						
						return;
                    }
                   
                    textSound.Play();

                    Say(text[wordIndex].text[index]);
				
					checkIfNull();
				getBackGroundName();
					checkBackground();

                }
                else
                {
                    enemyManager.enemyMangerCurrentEncounter.SetActiveBehavior(false);
                }

            }

            if (index <= text.Length)
            {
                stopSay(text[wordIndex].text[index]);
			
            }
        }
        Delay();
	
	}

    //Gets the Index for which line should be said
    void Say(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";
        dialogue.Say(speech, speaker);
    }
    //Gets the Index for which line should be said and skips it to so its all written out
    void stopSay(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";
        SkipText(speech, speaker);
    }


	//If the user wants to skip the text it checks for either mouse or space bar input and the writes out the rest of the passage.
	void SkipText(string speech, string speaker)
	{
		if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0))) && !dialogue.isWatingForUserInput)
		{
			if (userInput == true)
			{
				dialogue.SkipTextScroll(speech, speaker);
				index++;
				userInput = false;
				waitTime = 0;
				textSound.Play();
			}

		}
	
	}

	//This delay makes it so that if space or mouse is clicked it does not skip multiple diolouges 
	void Delay()
    {
        if (dialogue.isWatingForUserInput == false)
        {
            if (waitTime != WaitBeforeSkip)
            {
                waitTime += Time.deltaTime;
            }

            if (waitTime >= WaitBeforeSkip)
            {
                userInput = true;
            }
        }
        else
        {
            userInput = false;
        }
    }

    //Just checks when the game starts
    void gameStart()
    {
        if (GameStart == true)
        {
			//getBackGroundName();
			//enemyManager.enemyMangerCurrentEncounter.SetActiveBehavior();
			Say(text[wordIndex].text[index]);
			//checkIfNull();
			//checkBackground();
			GameStart = false;
        }
    }

    //gets the user imput for the back ground names
	string getBackGroundName()
	{

	    backgroundName = text[index].BackgroundName;
		return backgroundName;
	}

    //Goes through the game object list and checks if they are null or not and if they are null just ignore them.This also activates the image if the drop down is set to that emotions.
    void checkIfNull()
	{
		if (text != null)
		{

			for (int i = 0; i < characters.Length; i++)
			{

				if (dialogue.speakerNameHold == characters[i].characterName)
				{
					switch (text[index].emotion)
					{
						case Text.Emotion.Happy:
							{
								nullCheck(i);
								characters[i].Happy.SetActive(true);
								break;
							}
						case Text.Emotion.Sad:
							{
								nullCheck(i);
								characters[i].Sad.SetActive(true);
								break;
							}
						case Text.Emotion.Angry:
							{
								nullCheck(i);
								characters[i].Angry.SetActive(true);
								break;
							}
						case Text.Emotion.Nutrual:
							{
								nullCheck(i);
								characters[i].Nutral.SetActive(true);
								break;
							}
						default:
							{
								break;
							}
					}

				}
				else if (dialogue.speakerNameHold != characters[i].characterName)
				{
					nullCheck(i);
				}
			}

		}


	}
    //Goes through the game object list and checks if they are null or not and if they are null just ignore them. This also makes images inactive if the text is not that emotion.

    void nullCheck(int i)
	{
		if (characters[i].Sad != null)
		{
			characters[i].Sad.SetActive(false);
		}

		if (characters[i].Happy != null)
		{
			characters[i].Happy.SetActive(false);
		}

		if (characters[i].Angry != null)
		{
			characters[i].Angry.SetActive(false);
		}

		if (characters[i].Nutral != null)
		{
			characters[i].Nutral.SetActive(false);
		}
	}

  //This checks if the background image is null and if it is the intended background image to use or not.
	void checkBackground()
	{
		if (text != null)
		{
			for (int i = 0; i < background.Length; i++)
			{
				if (background[i].backgroundImage != null)
				{


					if (backgroundName == background[i].backgroundName)
					{
						background[i].backgroundImage.SetActive(true);
					}
					else if (backgroundName != background[i].backgroundName)
					{
						background[i].backgroundImage.SetActive(false);
					}
				}
			}
		}
	}

    //this checks if the user wants to load the scene or not then goes to that scene
	void LoadScene(int sceneNumber)
	{
	//	if (novelManager.Trigger6 == true)
	//	{
			if (GoToNextScene == true)
			{
				SceneManager.LoadScene(sceneNumber);
			}
		//}
			
	}


}


