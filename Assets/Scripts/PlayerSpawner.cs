using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	// Use this for initialization
	public int playerIndex;
	bool playerSpawned;
	void OnEnable(){
        EventManager.StartListening(EventNames.PlayerWaving, AddPlayer);
    }

    void OnDisable(){
        EventManager.StopListening(EventNames.PlayerWaving, AddPlayer);
    }

	void AddPlayer(string playerId)
	{
		var index = Int32.Parse(playerId);
		if(index==playerIndex && !playerSpawned)
		{
			KinectManager manager = KinectManager.Instance;
			playerSpawned = true;
			Instantiate(Resources.Load(string.Format("player{0}",playerId)), gameObject.transform.position, Quaternion.identity);
			EventManager.TriggerEvent(EventNames.DebugMessage,string.Format("Add player {0}",playerId));
			
			manager.refreshAvatarControllers();
			manager.RefreshAvatarUserIds();
		}
		
	}

	IEnumerator refreshControllers()
	{
		yield return new WaitForSeconds(0.5f);

		KinectManager manager = KinectManager.Instance;
		manager.refreshAvatarControllers();
		manager.RefreshAvatarUserIds();
	}
}
