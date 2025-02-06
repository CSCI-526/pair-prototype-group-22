using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class TimeManager : MonoBehaviour
{
    public float levelTime = 120f; // Set the level time (e.g., 60 seconds)
    private float currentTime;
    public Slider timerBar; // Assign in Inspector
    public GameOverUI gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
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
        currentTime -= Time.deltaTime;
        
        if (timerBar != null)
            timerBar.value = currentTime;

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Time's up! Restart or go back to ship building.");
        gameOverUI.ShowGameOverScreen(); // Show Game Over UI
    }


}
