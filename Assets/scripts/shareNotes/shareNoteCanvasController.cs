using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shareNoteCanvasController : MonoBehaviour
{
    public Canvas shareNoteCanvas; // Reference to the settings menu canvas

    void Start()
    {
        shareNoteCanvas.enabled = false; // Hide the settings menu canvas on start
    }

    // Method to show the settings menu
    public void ShowshareNoteCanvas()
    {
        shareNoteCanvas.enabled = true;
    }

    // Method to hide the settings menu
    public void HideshareNoteCanvas()
    {
        shareNoteCanvas.enabled = false;
    }
}
