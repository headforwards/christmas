using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    GameObject scoreboard;
    GameObject welcome;
    GameObject instructions;
    GameObject gameover;
    PresentSpawner presentSpawner;

    // Use this for initialization
    void Start()
    {
        scoreboard = GameObject.Find("scoreBoard") as GameObject;
        welcome = GameObject.Find("welcome") as GameObject;
        instructions = GameObject.Find("instructions") as GameObject;
        gameover = GameObject.Find("gameover") as GameObject;
        presentSpawner = GameObject.FindObjectOfType(typeof(PresentSpawner)) as PresentSpawner;

        EventManager.TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers);

        // for (int i = 0; i < 6; i++)
        // {
        //     StartCoroutine(TriggerEvent(EventNames.PlayerWaving, i.ToString(), 2.0f));
        //     if (i % 2 == 0)
        //         StartCoroutine(TriggerEvent(EventNames.PlayerLost, i.ToString(), 2.0f * i));
        // }

        // StartCoroutine(TriggerEvent(EventNames.PlayerWaving, "0", 2.0f));

        float delay = 4.0f;

        StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers, delay));
        StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.PlayerJoined, delay += 2.0f));
        StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.InProgress, delay += 2.0f));
        StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.GameFinished, delay += 4.0f));
    }

    IEnumerator TriggerEvent(string eventName, string payload, float wait)
    {
        yield return new WaitForSeconds(wait);

        EventManager.TriggerEvent(eventName, payload);
    }

    void gameStateChanged(string gameState)
    {
        switch (gameState)
        {
            case GameStates.WaitingForPlayers:
                gameover.SetActive(false);
                welcome.SetActive(true);
                scoreboard.SetActive(false);
                instructions.SetActive(false);
                presentSpawner.minInterval = 1.0f;
                presentSpawner.maxInterval = 2.0f;
                break;
            case GameStates.PlayerJoined:
                welcome.SetActive(false);
                instructions.SetActive(true);
                break;
            case GameStates.InProgress:
                instructions.SetActive(false);
                scoreboard.SetActive(true);
                presentSpawner.minInterval = 0.1f;
                presentSpawner.maxInterval = 0.5f;
                break;
            case GameStates.GameFinished:
                scoreboard.SetActive(false);
                gameover.SetActive(true);
                presentSpawner.minInterval = 10.0f;
                presentSpawner.maxInterval = 10.0f;
                StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers, 20.0f));
                break;
        }
    }

    void OnEnable()
    {
        EventManager.StartListening(EventNames.GameStateChanged, gameStateChanged);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventNames.GameStateChanged, gameStateChanged);
    }

}
