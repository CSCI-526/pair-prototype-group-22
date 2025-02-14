using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in Inspector
    public GameObject gameOverText;

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToShipBuilding()
    {
        SceneManager.LoadScene("BuildScene"); // Change to actual scene name
    }
}
