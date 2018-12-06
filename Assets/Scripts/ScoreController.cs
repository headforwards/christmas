using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    public TextMesh playerOneScore;
    public TextMesh playerTwoScore;
    public TextMesh playerThreeScore;
    public TextMesh playerFourScore;
    public TextMesh playerFiveScore;
    public TextMesh playerSixScore;

    void Start()
    {
        playerOneScore.text = "0";
        playerTwoScore.text = "0";
        playerThreeScore.text = "0";
        playerFourScore.text = "0";
        playerFiveScore.text = "0";
        playerSixScore.text = "0";

    }
    void OnEnable()
    {
        EventManager.StartListening("scoreupdate", UpdateScore);
    }

    public void UpdateScore(string playerName)
    {
        var playerNumber = playerName[playerName.Length - 1].ToString();

        switch (playerNumber)
        {
            case "1":
                playerOneScore.text = UpdatePlayerScore(playerOneScore);                
                break;
            case "2":
                playerTwoScore.text = UpdatePlayerScore(playerTwoScore);
                break;
            case "3":
                playerThreeScore.text = UpdatePlayerScore(playerThreeScore);
                break;
            case "4":
                playerFourScore.text = UpdatePlayerScore(playerFourScore);
                break;
            case "5":
                playerFiveScore.text = UpdatePlayerScore(playerFiveScore);
                break;
            case "6":
                playerSixScore.text = UpdatePlayerScore(playerSixScore);
                break;
            default:
                break;
        }

    }

    private string UpdatePlayerScore(TextMesh player)
    {
        int currentScore = 0;
        int updatedScore = 0 ;
        if (Int32.TryParse(player.text, out currentScore))
        {
            updatedScore = (currentScore + 1);
        }

        return updatedScore.ToString();
    }

    void OnDisable()
    {
        EventManager.StopListening("scoreUpdate", UpdateScore);
    }
}
