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
        GameObject player = GameObject.Find("Player");
        player.GetComponent<Transform>().position = new Vector3(0, 0, -5.0f);
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
    }

    public void GoToShipBuilding()
    {
        SceneManager.LoadScene("BuildScene"); // Change to actual scene name
        GameObject player = GameObject.Find("Player");
        Destroy(player);
    }
}
