using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBoardController : MonoBehaviour
{

    public TextMesh leaderBoard;

    public List<Player> playerList = new List<Player>();

    void Start()
    {
        playerList = new List<Player>()
        {
            new Player("Player1",0),
            new Player("Player2",0),
            new Player("Player3",0),
            new Player("Player4",0),
            new Player("Player5",0),
            new Player("Player6",0)
        };

        //BuildLeaderBoard(playerList.OrderByDescending(x => x.Score).ToList());
        playerList.Single(x => x.Name == "Player5").Score = playerList.Single(x => x.Name == "Player5").Score + 10;
        BuildLeaderBoard(playerList.OrderByDescending(x => x.Score).ToList());
        EventManager.StartListening("scoreBoardUpdate", UpdateLeaderBoard);

    }

    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Player(string name, int score)
        {
            Name = name;
            Score = score;
        }
        public void UpdateScore()
        {
            Score = Score + 1;
        }
        public string GetNameAndScore()
        {
            return Name + ": " + Score.ToString();
        }
    }

    //void OnEnable()
    //{

    //}
    //void OnDisable()
    //{
    //    EventManager.StopListening("scoreUpdate", UpdateLeaderBoard);
    //}

    public void UpdateLeaderBoard(string playerName)
    {
        Debug.Log("playerName: " + playerName);

        playerList.Single(x => x.Name == playerName).Score = playerList.Single(x => x.Name == playerName).Score + 1;

        var orderedPlayerList = playerList.OrderByDescending(x => x.Score).ToList();

        BuildLeaderBoard(orderedPlayerList);
    }

    private void BuildLeaderBoard(List<Player> orderedPlayerList)
    {
        string scoreBoard = "LeaderBoard:";
        foreach (var player in orderedPlayerList)
        {
            scoreBoard += "\n" + player.GetNameAndScore();
        }
        leaderBoard.text = scoreBoard;
        Debug.Log("CurrentLeader " + orderedPlayerList.First().GetNameAndScore());
    }
}
