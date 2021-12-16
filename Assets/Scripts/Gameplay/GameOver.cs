using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public int enemyKilled;
    private int scoreVal;
    private float timeVal;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI totalTime;
    [SerializeField] private TextMeshProUGUI totalEnemy;
    [SerializeField] private UITimer timer;
    [SerializeField] private Transform playerPos;

    public void showGameOver()
    {
        Time.timeScale = 0f;
        Vector3 offset = playerPos.forward;
        offset *= 2f;
        this.gameObject.transform.position = playerPos.position + offset;

        transform.GetChild(0).gameObject.SetActive(true);

        timeVal = timer.GetTimer();
        int minutes = Mathf.FloorToInt(timeVal / 60F);
        int seconds = Mathf.FloorToInt(timeVal % 60F);
        int milliseconds = Mathf.FloorToInt((timeVal * 100F) % 100F);
        totalTime.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");

        totalEnemy.text = enemyKilled.ToString();

        scoreVal = (enemyKilled * 30) + (Mathf.FloorToInt(timeVal) * 10);
        score.text = scoreVal.ToString();

        if (scoreVal > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", scoreVal);
            highScore.text = "New high score!";
        }
        else
        {
            highScore.text = "High score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
