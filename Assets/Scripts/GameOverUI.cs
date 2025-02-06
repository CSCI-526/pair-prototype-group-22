using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in Inspector

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToShipBuilding()
    {
        SceneManager.LoadScene("ShipBuildingScene"); // Change to actual scene name
    }
}
