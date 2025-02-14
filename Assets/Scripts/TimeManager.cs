using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class TimeManager : MonoBehaviour
{
    public float levelTime = 240f; // Set the level time (e.g., 60 seconds)
    private float currentTime;
    public Slider timerBar; // Assign in Inspector
    public GameOverUI gameOverUI;

    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;

        currentTime = levelTime;
        if (timerBar != null)
        {
            timerBar.maxValue = levelTime;
            timerBar.value = levelTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            currentTime -= Time.deltaTime;

            if (timerBar != null)
                timerBar.value = currentTime;

            if (currentTime <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        gameOver = true;
        gameOverUI.ShowGameOverScreen(); // Show Game Over UI
    }

    public void setGameOverTrue()
    {
        gameOver = true;
    }

}
