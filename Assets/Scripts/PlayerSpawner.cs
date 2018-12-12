using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

    void OnEnable()
    {
        EventManager.StartListening(EventNames.PlayerWaving, AddPlayer);
        EventManager.StartListening(EventNames.PlayerLost, RemovePlayer);
        EventManager.StartListening(EventNames.GameStateChanged, gameStateChanged);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventNames.PlayerWaving, AddPlayer);
        EventManager.StartListening(EventNames.PlayerLost, RemovePlayer);
        EventManager.StartListening(EventNames.GameStateChanged, gameStateChanged);
    }

    void gameStateChanged(string gameState)
    {

        if (gameState == GameStates.GameFinished)
		{
			foreach(var player in players.Values){
				Destroy(player);
			}
			
			players.Clear();
		}

    }

    void AddPlayer(string playerId)
    {
        var index = Int32.Parse(playerId);

        if (!players.ContainsKey(index))
        {
            players.Add(index, null);

            KinectManager manager = KinectManager.Instance;

            long id = manager.GetUserIdByIndex(index);

            var pos = manager.GetUserPosition(id);

            var spawnPosition = gameObject.transform.position;

            spawnPosition.x = pos.x * 2;

            var playerObject = Instantiate(
                   Resources.Load(string.Format("players/player{0}", playerId)),
                   spawnPosition,
                   Quaternion.identity) as GameObject;

            players[index] = playerObject;

            EventManager.TriggerEvent(EventNames.DebugMessage, string.Format("Added player {0}", playerId));
            EventManager.TriggerEvent(EventNames.GameStateChanged, GameStates.PlayerJoined);

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

        if (players.ContainsKey(index))
        {
            Destroy(players[index]);

            players.Remove(index);

            RefreshAvatars();

            EventManager.TriggerEvent(EventNames.DebugMessage, string.Format("Removed player {0}", playerId));
        }
    }
}
