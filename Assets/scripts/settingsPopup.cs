using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsPopup : MonoBehaviour
{
    public Canvas settingsMenuCanvas; // Reference to the settings menu canvas

    void Start()
    {
        settingsMenuCanvas.enabled = false; // Hide the settings menu canvas on start
    }

    // Method to show the settings menu
    public void ShowSettingsMenu()
    {
        settingsMenuCanvas.enabled = true;
    }

    // Method to hide the settings menu
    public void HideSettingsMenu()
    {
        settingsMenuCanvas.enabled = false;
    }
}
