using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    GameObject welcome;
    GameObject instructions;
    GameObject gameover;

    GameObject inprogress;
    PresentSpawner presentSpawner;

    bool gameInProgress;

    List<int> players = new List<int>();
    List<int> playersWithArmsRaised = new List<int>();
    // Use this for initialization

    public int gameLength = 30;

    public Text timer;

    IEnumerator countDownTimer()
    {
        if (timer != null)
        {
            timer.text = string.Format("Time Left: {0} seconds", gameLength--);
        }
        yield return new WaitForSeconds(1.0f);
        if (gameLength <= 0)
        {
            EventManager.TriggerEvent(EventNames.GameStateChanged, GameStates.GameFinished);
        }
        else
        {
            StartCoroutine(countDownTimer());
        }
    }

    void Start()
    {
        inprogress = GameObject.Find("inprogress") as GameObject;
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

        // StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers, delay));
        // StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.PlayerJoined, delay += 2.0f));
        // StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.InProgress, delay += 2.0f));
        // StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.GameFinished, delay += 4.0f));
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
                gameInProgress = false;
                gameover.SetActive(false);
                welcome.SetActive(true);
                inprogress.SetActive(false);
                instructions.SetActive(false);
                presentSpawner.minInterval = 1.0f;
                presentSpawner.maxInterval = 2.0f;
                break;
            case GameStates.PlayerJoined:
                Debug.Log(gameInProgress);
                if (!gameInProgress)
                {
                    welcome.SetActive(false);
                    instructions.SetActive(true);
                }

                break;
            case GameStates.InProgress:
                gameInProgress = true;
                instructions.SetActive(false);
                inprogress.SetActive(true);
                presentSpawner.minInterval = 0.1f;
                presentSpawner.maxInterval = 0.5f;
                StartCoroutine(countDownTimer());
                break;
            case GameStates.GameFinished:
                gameInProgress = false;
                inprogress.SetActive(false);
                gameover.SetActive(true);
                presentSpawner.minInterval = 10.0f;
                presentSpawner.maxInterval = 10.0f;
                StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers, 20.0f));
                break;
        }
    }

    void playerReady(string playerId)
    {
        if (gameInProgress) return;
        int id = int.Parse(playerId);
        if (!playersWithArmsRaised.Contains(id))
        {
            playersWithArmsRaised.Add(id);
        }

        if (players.All(m => playersWithArmsRaised.Contains(m)))
        {
            EventManager.TriggerEvent(EventNames.GameStateChanged, GameStates.InProgress);
        }
    }

    void playerWaving(string playerId)
    {
        int id = int.Parse(playerId);
        if (!players.Contains(id))
        {
            players.Add(id);
        }
    }

    void playerLost(string playerId)
    {
        int id = int.Parse(playerId);
        if (players.Contains(id))
        {
            players.Remove(id);
            playersWithArmsRaised.Remove(id);
        }
    }

    void OnEnable()
    {
        EventManager.StartListening(EventNames.PlayerWaving, playerWaving);
        EventManager.StartListening(EventNames.PlayerLost, playerLost);
        EventManager.StartListening(EventNames.GameStateChanged, gameStateChanged);
        EventManager.StartListening(EventNames.PlayerReady, playerReady);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventNames.PlayerWaving, playerWaving);
        EventManager.StopListening(EventNames.PlayerLost, playerLost);
        EventManager.StopListening(EventNames.GameStateChanged, gameStateChanged);
        EventManager.StopListening(EventNames.PlayerReady, playerReady);
    }

}
