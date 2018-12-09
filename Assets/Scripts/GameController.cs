using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		for(int i = 0; i < 6;i++){
			StartCoroutine(TriggerEvent(EventNames.PlayerWaving, i.ToString(), 2.0f));
			if(i % 2 == 0)
				StartCoroutine(TriggerEvent(EventNames.PlayerLost, i.ToString(),2.0f * i));
		}
		

	}
	
	IEnumerator TriggerEvent(string eventName, string payload, float wait)
	{
		yield return new WaitForSeconds(wait);

		EventManager.TriggerEvent(eventName, payload);
	}

}
