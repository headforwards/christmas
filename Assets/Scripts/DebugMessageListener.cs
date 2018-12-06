using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMessageListener : MonoBehaviour {


	public Text debugMessage;

	void Start () 
	{
		OnDebugMessage("Ready");
	}

	void OnDebugMessage(string message)
	{
		if(debugMessage != null)
		{
			debugMessage.text = message;
		}
	}

	void OnEnable(){
        EventManager.StartListening(EventNames.DebugMessage, OnDebugMessage);
    }

    void OnDisable(){
        EventManager.StopListening(EventNames.DebugMessage, OnDebugMessage);
    }
}
