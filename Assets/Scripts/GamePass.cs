using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePass : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gamePassText;
    public GameObject timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameOverPanel.SetActive(true);
            gamePassText.SetActive(true);

            // stop timer here
            timer.GetComponent<TimeManager>().setGameOverTrue();
        }
    }
}
