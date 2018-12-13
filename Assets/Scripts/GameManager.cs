using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    GameObject welcome;
    GameObject instructions;
    GameObject gameover;
    GameObject inprogress;
    PresentSpawner presentSpawner;

    int requestedGameLength;
    bool gameInProgress;
    List<int> players = new List<int>();
    List<int> playersReady = new List<int>();

    public int gameLength = 30;
    public float displayGameFinished = 20.0f;
    public TMP_Text timer;

    public TMP_Text startCountDown;
    public TMP_Text endCountDown;

    void Start()
    {
        inprogress = GameObject.Find("inprogress") as GameObject;
        welcome = GameObject.Find("welcome") as GameObject;
        instructions = GameObject.Find("instructions") as GameObject;
        gameover = GameObject.Find("gameover") as GameObject;
        presentSpawner = GameObject.FindObjectOfType(typeof(PresentSpawner)) as PresentSpawner;

        EventManager.TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers);

        requestedGameLength = gameLength;

        // StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.PlayerJoined, 2.0f));

        // StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.InProgress, 4.0f));
    }

    IEnumerator TriggerEvent(string eventName, string payload, float wait)
    {
        yield return new WaitForSeconds(wait);

        Debug.Log(string.Format("Triggering: {0} > {1}", eventName, payload));

        EventManager.TriggerEvent(eventName, payload);
    }

    void UpdateGameTimerText()
    {
        if (timer != null)
        {
            timer.text = string.Format("Time Left: {0} seconds", gameLength--);
        }

        if (endCountDown != null)
        {
            endCountDown.text = gameLength <= 4 ? (gameLength +1).ToString() : "";
        }
    }
    IEnumerator gameTimer()
    {
        UpdateGameTimerText();

        yield return new WaitForSeconds(1.0f);

        if (gameLength <= 0)
        {
            EventManager.TriggerEvent(EventNames.GameStateChanged, GameStates.GameFinished);
        }
        else
        {
            StartCoroutine(gameTimer());
        }
    }

    #region start game count down

    int startGameCountDown;

    IEnumerator startGame()
    {
        if (startCountDown != null)
        {
            switch (startGameCountDown)
            {
                case 4:
                    startCountDown.text = "Get Ready";
                    break;
                case 3:
                case 2:
                case 1:
                    startCountDown.text = startGameCountDown.ToString();
                    break;
                case 0:
                    startCountDown.text = "GO!!!";
                    EventManager.TriggerEvent(EventNames.StartPresents, "");
                    break;
            }

            yield return new WaitForSeconds(1.0f);

            startGameCountDown--;

            if (startGameCountDown >= 0)
                StartCoroutine(startGame());
            else
            {
                startCountDown.text = "";
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(gameTimer());
            }
        }
    }
    #endregion


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
                break;
            case GameStates.PlayerJoined:
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
                startGameCountDown = 4;
                UpdateGameTimerText();
                StartCoroutine(startGame());
                break;
            case GameStates.GameFinished:
                EventManager.TriggerEvent(EventNames.StopPresents, "");
                gameInProgress = false;
                inprogress.SetActive(false);
                gameover.SetActive(true);

                playersReady.Clear();
                players.Clear();

                gameLength = requestedGameLength;

                StartCoroutine(TriggerEvent(EventNames.GameStateChanged, GameStates.WaitingForPlayers, displayGameFinished));
                break;
        }
    }

    void playerReady(string playerId)
    {
        if (gameInProgress) return;
        int id = int.Parse(playerId);
        if (players.Contains(id) && !playersReady.Contains(id))
        {
            playersReady.Add(id);
        }

        if (players.All(m => playersReady.Contains(m)))
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
            playersReady.Remove(id);
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
