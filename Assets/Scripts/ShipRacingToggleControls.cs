using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipRacingToggleControls : MonoBehaviour
{
    public GameObject controlsPanel;  // Reference to the panel
    public Button toggleButton;       // Reference to the button
    public TMP_Text buttonText;

    private bool isShowing = false;
    // Start is called before the first frame update
    void Start()
    {
        // Ensure the panel starts hidden
        controlsPanel.SetActive(isShowing);

        // Assign button click event
        toggleButton.onClick.AddListener(ToggleControls);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleControls()
    {
        isShowing = !isShowing;
        controlsPanel.SetActive(isShowing);
        buttonText.text = isShowing ? "Hide Controls" : "Show Controls"; // Update button text
    }
}
