using UnityEngine;


public class UI_FeedbackLink : MonoBehaviour 
{

	public string Url;

    public void Open()
    {
        Application.OpenURL(Url);
    }
    
}

