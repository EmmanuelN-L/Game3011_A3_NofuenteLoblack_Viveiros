using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [Header("UI Text")]
    public TMPro.TextMeshProUGUI TimeText;
    public TMPro.TextMeshProUGUI ScoreText;
    public TMPro.TextMeshProUGUI ScoreToReachText;
    public TMPro.TextMeshProUGUI ResultsText;
    [Header("UI Panels")]
    public GameObject GameUIPanel;
    public GameObject DifficultyPanel;
    public GameObject ResultsPanel;

    private float time = 60f;
    private float TimeThreshold = 0f;
    bool isGameActive = false;

    private Board board;
    private void Start()
    {
        board = FindObjectOfType<Board>();
    }

    private void Update()
    {  
        if (!isGameActive) return;

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
        ResultsPanel.SetActive(true);
        ResultsText.text = "Congratulations, You Win!";
        isGameActive = false;
    }
    public void loseCondition()
    {
        Time.timeScale = 0;
        Debug.Log("You lose!");
        ResultsPanel.SetActive(true);
        ResultsText.text = "Try again you almost had it!";
        isGameActive = false;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Main");
    }

    public void EasySelected()
    {
        Debug.Log("Easy difficulty selected");
        DifficultyPanel.SetActive(false);
        board.scoreToReach = 50000;
        ScoreToReachText.text = "Score to reach: " + board.scoreToReach;
        board.maxCandy = 3;
        board.Setup();
        isGameActive = true;
    }
    public void MediumSelected()
    {
        Debug.Log("Medium difficulty selected");
        DifficultyPanel.SetActive(false);
        board.scoreToReach = 25000;
        ScoreToReachText.text = "Score to reach: " + board.scoreToReach;
        board.maxCandy = 4;
        board.Setup();
        isGameActive = true;
    }
    public void HardSelected()
    {
        Debug.Log("Hard difficulty selected");
        DifficultyPanel.SetActive(false);
        board.scoreToReach = 10000;
        ScoreToReachText.text = "Score to reach: " + board.scoreToReach;
        board.maxCandy = 5;
        board.Setup();
        isGameActive = true;
    }


}
