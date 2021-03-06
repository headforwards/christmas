﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using TMPro;

public class ScoreBoard : MonoBehaviour
{

    class Player
    {
        public Player(int index)
        {
            Index = index;
        }
        public int Score { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return string.Format("Player {0}: {1}", Index + 1, Score);
        }
    }

    public TMP_Text ScoreBoardText;
    public TMP_Text ScorePlayerSummary;
    public TMP_Text TeamScore;
    public TMP_Text CurrentHighScore;
    public TMP_Text NewHighScore;

    List<Player> players = new List<Player>();
    private int highScore;

    void Start()
    {
        UpdateScoreText("Waiting for players");
    }

    int PlayerIndex(string kinectPlayerIndex)
    {
        int playerIndex = 0;
        int.TryParse(kinectPlayerIndex, out playerIndex);
        return playerIndex;
    }

    void playerWaving(string player)
    {
        var playerIndex = PlayerIndex(player);

        if (!players.Any(p => p.Index == playerIndex))
        {
            players.Add(new Player(playerIndex));
            UpdateScores();
        }
    }

    void playerLost(string player)
    {
        var playerIndex = PlayerIndex(player);

        players.RemoveAll(p => p.Index == playerIndex);
        UpdateScores();
    }

    void playerScored(string playerName)
    {
        if (!inProgress)
            return;

        var playerId = playerName.ToLower().Replace("player", "");

        int idx = 0;

        if (!int.TryParse(playerId[0].ToString(), out idx))
            return;

        var player = players.FirstOrDefault(p => p.Index == idx);

        if (player != null)
            player.Score++;

        UpdateScores();
    }

    void UpdateScores()
    {
        if (!players.Any())
        {
            UpdateScoreText("No players :(");
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (var player in players
            .OrderBy(p => p.Index))
        {
            sb.AppendLine(player.ToString());
        }

        UpdateScoreText(sb.ToString());
    }

    void UpdateScoreText(string message)
    {
        if (!inProgress)
            return;

        if (ScoreBoardText != null)
            ScoreBoardText.text = message;
        if (ScorePlayerSummary != null)
            ScorePlayerSummary.text = message;

        if (TeamScore != null)
        {
            var score = players.Sum(p => p.Score);

            TeamScore.text = string.Format("Your Team Scored: {0}", score);

            if (score > highScore)
            {
                NewHighScore.text = "Your Team Got The High Score!!!";
                CurrentHighScore.text = string.Empty;
            }
            else
            {
                NewHighScore.text = string.Empty;
                CurrentHighScore.text = string.Format("Current Highest Score Is: {0}", highScore);
            }

            highScore = System.Math.Max(score, highScore);
        }
    }


    bool inProgress = false;
    void gameStateChanged(string gameState)
    {
        inProgress = gameState == GameStates.InProgress;
        if (gameState == GameStates.GameFinished)
        {
            players.Clear();
        }
        UpdateScores();
    }

    void OnEnable()
    {
        EventManager.StartListening(EventNames.PlayerWaving, playerWaving);
        EventManager.StartListening(EventNames.PlayerLost, playerLost);
        EventManager.StartListening(EventNames.UpdateScore, playerScored);
        EventManager.StartListening(EventNames.GameStateChanged, gameStateChanged);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventNames.PlayerFound, playerWaving);
        EventManager.StopListening(EventNames.PlayerLost, playerLost);
        EventManager.StopListening(EventNames.UpdateScore, playerScored);
        EventManager.StopListening(EventNames.GameStateChanged, gameStateChanged);
    }
}
