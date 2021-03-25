using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TimeText;
    public TMPro.TextMeshProUGUI ScoreText;
    public TMPro.TextMeshProUGUI ScoreToReachText;

    private float time = 60f;
    private float TimeThreshold = 0f;
    //private int scoreToReach;

    private Board board;
    private void Start()
    {
        board = FindObjectOfType<Board>();
    }

    private void Update()
    {    
        TimeThreshold += Time.deltaTime;
        if (TimeThreshold >= 1)
        {
            time--;
            TimeThreshold = 0f;
            TimeText.text = "Time: " + time;
        }
        if(time <=0)
        {
            loseCondition();
        }
        ScoreText.text = "Score: "+ board.score;
    }
    public void winCondition()
    {
        Time.timeScale = 0;
        Debug.Log("You win!");
    }
    public void loseCondition()
    {
        Time.timeScale = 0;
        Debug.Log("You lose!");
    }
    public void EasySelected()
    {
        Debug.Log("Easy difficulty selected");
        board.scoreToReach = 50000;
        ScoreToReachText.text = "Score to reach: " + board.scoreToReach;
        board.maxCandy = 3;
        board.Setup();
    }
    public void MediumSelected()
    {
        Debug.Log("Medium difficulty selected");
        board.scoreToReach = 25000;
        ScoreToReachText.text = "Score to reach: " + board.scoreToReach;
        board.maxCandy = 4;
        board.Setup();
    }
    public void HardSelected()
    {
        Debug.Log("Hard difficulty selected");
        board.scoreToReach = 10000;
        ScoreToReachText.text = "Score to reach: " + board.scoreToReach;
        board.maxCandy = 5;
        board.Setup();
    }


}
