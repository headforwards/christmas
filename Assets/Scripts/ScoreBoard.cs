using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoard : MonoBehaviour {

	public Text ScoreText; 

	// Use this for initialization
	void Start () {
		UpdateScoreText("Waiting for players");
	}
	
	void playerFound(string player)
	{
		UpdateScoreText(string.Format("found: {0}",player));
	}

	void playerLost(string player)
	{
		UpdateScoreText(string.Format("lost: {0}",player));

	}

	void playerScored(string player)
	{
		UpdateScoreText(string.Format("scored: {0}",player));

	}

	void UpdateScoreText(string message)
	{
		if(ScoreText != null)
			ScoreText.text = message;
	}

	void OnEnable(){
        EventManager.StartListening(EventNames.PlayerFound, playerFound);
        EventManager.StartListening(EventNames.PlayerLost, playerLost);
        EventManager.StartListening(EventNames.UpdateScore, playerScored);
    }

    void OnDisable(){
        EventManager.StopListening(EventNames.PlayerFound, playerFound);
        EventManager.StopListening(EventNames.PlayerLost, playerLost);
        EventManager.StartListening(EventNames.UpdateScore, playerScored);
    }
}
