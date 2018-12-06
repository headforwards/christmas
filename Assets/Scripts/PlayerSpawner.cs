using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	[Tooltip("Zero based index for player")]
	public int playerIndex;

	GameObject playerObject;

	void OnEnable(){
        EventManager.StartListening(EventNames.PlayerWaving, AddPlayer);
		EventManager.StartListening(EventNames.PlayerLost, RemovePlayer);
    }

    void OnDisable(){
        EventManager.StopListening(EventNames.PlayerWaving, AddPlayer);
		EventManager.StartListening(EventNames.PlayerLost, RemovePlayer);
    }

	void AddPlayer(string playerId)
	{
		var index = Int32.Parse(playerId);

		if(index==playerIndex && playerObject == null)
		{
			playerObject = Instantiate(
				Resources.Load(string.Format("players/player{0}",playerId)), 
				gameObject.transform.position, 
				Quaternion.identity) as GameObject;

			EventManager.TriggerEvent(EventNames.DebugMessage,string.Format("Added player {0}",playerId));
			
			RefreshAvatars();
		}
	}

	void RefreshAvatars()
	{
		KinectManager manager = KinectManager.Instance;
		manager.refreshAvatarControllers();
		manager.RefreshAvatarUserIds();
	}

	void RemovePlayer(string playerId)
	{
		var index = Int32.Parse(playerId);

		if(index==playerIndex && playerObject != null)
		{
			Destroy(playerObject);
			
			RefreshAvatars();

			EventManager.TriggerEvent(EventNames.DebugMessage,string.Format("Removed player {0}",playerId));
		}
	}
}
