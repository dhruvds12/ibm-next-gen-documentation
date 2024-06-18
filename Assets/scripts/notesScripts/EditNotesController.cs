using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EditNotesController : MonoBehaviour
{
    public GameObject notesText; // Reference to the TextMeshProUGUI object
    public GameObject notesInputField; // Reference to the TMP_InputField object
    public Button editButton; // Reference to the Edit button
    public ScrollRect notesScrollRect; // Reference to the ScrollRect
    public TMP_Dropdown notesDropdown; // Reference to the TMP_Dropdown object

    public RectTransform scrollViewRectTransform; // Reference to the ScrollView RectTransform
    public float editModeHeight = 1250f; // Height of ScrollView in edit mode
    public float viewModeHeight = 2000f; // Height of ScrollView in view mode
    public float editModePosY = -700f; // Y position of ScrollView in edit mode
    public float viewModePosY = -1000f; // Y position of ScrollView in view mode

    private ApiManager apiManager; // Reference to ApiManager

    private bool isEditMode = false;

    private const string userId = "5678"; // Hardcoded userId
    private const string noteKey = "general_note"; // Hardcoded noteKey
    private const string imageName = "image1"; // Hardcoded imageName

    private Dictionary<string, string> allNotes = new Dictionary<string, string>();

    void Start()
    {
        // Initially set the input field to inactive
        notesInputField.SetActive(false);

        // Add listener to the edit button
        editButton.onClick.AddListener(ToggleEditMode);

        // Add listener to the dropdown
        notesDropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });

        // Set initial ScrollRect content to NotesText
        notesScrollRect.content = notesText.GetComponent<RectTransform>();

        // Set initial height and position to view mode settings
        SetScrollViewPosition(viewModePosY);
        SetScrollViewHeight(viewModeHeight);

        // Load saved notes from server
        apiManager = FindObjectOfType<ApiManager>();
        if (apiManager != null)
        {
            StartCoroutine(apiManager.GetNote(userId, imageName, noteKey, OnNoteLoaded));
            // Fetch shared notes
            StartCoroutine(apiManager.GetSharedNotes(userId, imageName, noteKey, OnSharedNotesLoaded));
        }
    }

    void ToggleEditMode()
    {
        isEditMode = !isEditMode;

        if (isEditMode)
        {
            // Switch to edit mode
            notesText.SetActive(false);
            notesInputField.SetActive(true);

            // Copy text from TextMeshProUGUI to TMP_InputField
            TMP_InputField inputField = notesInputField.GetComponent<TMP_InputField>();
            TextMeshProUGUI text = notesText.GetComponent<TextMeshProUGUI>();
            inputField.text = text.text;

            // Change ScrollRect content to NotesInputField
            notesScrollRect.content = notesInputField.GetComponent<RectTransform>();

            // Change button text to "Save"
            editButton.GetComponentInChildren<TextMeshProUGUI>().text = "Save";

            // Set height and position to edit mode settings
            SetScrollViewPosition(editModePosY);
            SetScrollViewHeight(editModeHeight);
        }
        else
        {
            // Switch to view mode
            notesText.SetActive(true);
            notesInputField.SetActive(false);

            // Copy text from TMP_InputField to TextMeshProUGUI
            TMP_InputField inputField = notesInputField.GetComponent<TMP_InputField>();
            TextMeshProUGUI text = notesText.GetComponent<TextMeshProUGUI>();
            text.text = inputField.text;

            // Change ScrollRect content to NotesText
            notesScrollRect.content = notesText.GetComponent<RectTransform>();

            // Change button text to "Edit"
            editButton.GetComponentInChildren<TextMeshProUGUI>().text = "Edit";

            // Save the text to server
            if (apiManager != null)
            {
                StartCoroutine(apiManager.PostNote(userId, imageName, noteKey, inputField.text));
            }

            // Set height and position to view mode settings
            SetScrollViewPosition(viewModePosY);
            SetScrollViewHeight(viewModeHeight);
        }
    }

    void SetScrollViewHeight(float height)
    {
        // Adjust the height of the ScrollView
        Vector2 sizeDelta = scrollViewRectTransform.sizeDelta;
        sizeDelta.y = height;
        scrollViewRectTransform.sizeDelta = sizeDelta;
    }

    void SetScrollViewPosition(float posY)
    {
        // Adjust the Y position of the ScrollView
        Vector2 anchoredPosition = scrollViewRectTransform.anchoredPosition;
        anchoredPosition.y = posY;
        scrollViewRectTransform.anchoredPosition = anchoredPosition;
    }

    void OnNoteLoaded(string noteContent)
    {
        allNotes["My Notes"] = noteContent;
        UpdateDropdownOptions();
        SetNotesText(noteContent);
        editButton.gameObject.SetActive(true); // Show the edit button when "My Notes" is loaded
    }

    void OnSharedNotesLoaded(List<string> sharedUsers)
    {
        Debug.Log("OnSharedNotesLoaded called");
        if (sharedUsers == null || sharedUsers.Count == 0)
        {
            Debug.LogError("No shared notes found");
            return;
        }

        foreach (var sharedUserId in sharedUsers)
        {
            StartCoroutine(apiManager.GetNote(sharedUserId, imageName, noteKey, noteContent =>
            {
                string noteKey = $"{sharedUserId}'s Notes";
                allNotes[noteKey] = noteContent;
                UpdateDropdownOptions();
            }));
        }
    }

    void UpdateDropdownOptions()
    {
        notesDropdown.options.Clear();

        foreach (var noteKey in allNotes.Keys)
        {
            notesDropdown.options.Add(new TMP_Dropdown.OptionData(noteKey));
        }

        notesDropdown.value = 0; // Default to the first note
        notesDropdown.RefreshShownValue();
        OnDropdownValueChanged(); // Ensure the default selection is displayed
    }

    void OnDropdownValueChanged()
    {
        string selectedNoteKey = notesDropdown.options[notesDropdown.value].text;
        if (allNotes.TryGetValue(selectedNoteKey, out string noteContent))
        {
            SetNotesText(noteContent);
            editButton.gameObject.SetActive(selectedNoteKey == "My Notes"); // Show/hide edit button based on selection
        }
        else
        {
            Debug.LogError("Selected note not found in allNotes dictionary");
        }
    }

    void SetNotesText(string noteContent)
    {
        TextMeshProUGUI text = notesText.GetComponent<TextMeshProUGUI>();
        text.text = noteContent;
    }
}
