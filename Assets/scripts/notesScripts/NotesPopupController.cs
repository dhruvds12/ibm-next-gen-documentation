using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesPopupController : MonoBehaviour
{
    public GameObject notespanel; // Reference to the settings menu canvas

    void Start()
    {
        notespanel.SetActive(false); // Hide the settings menu canvas on start
    }

    // Method to show the settings menu
    public void ShowNotes()
    {
        notespanel.SetActive(true);
    }

    // Method to hide the settings menu
    public void HideNotes()
    {
        notespanel.SetActive(false);
    }
}
