using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditNotesController : MonoBehaviour
{
    public GameObject notesText; // Reference to the TextMeshProUGUI object
    public GameObject notesInputField; // Reference to the TMP_InputField object
    public Button editButton; // Reference to the Edit button
    public ScrollRect notesScrollRect; // Reference to the ScrollRect

    public RectTransform scrollViewRectTransform; // Reference to the ScrollView RectTransform
    public float editModeHeight = 1250f; // Height of ScrollView in edit mode
    public float viewModeHeight = 2000f; // Height of ScrollView in view mode
    public float editModePosY = -700f; // Y position of ScrollView in edit mode
    public float viewModePosY = 1000; // Y position of ScrollView in view mode

    private bool isEditMode = false;

    void Start()
    {
        // Initially set the input field to inactive
        notesInputField.SetActive(false);

        // Add listener to the edit button
        editButton.onClick.AddListener(ToggleEditMode);

        // Set initial ScrollRect content to NotesText
        notesScrollRect.content = notesText.GetComponent<RectTransform>();

        // Set initial height and position to view mode settings
        SetScrollViewAnchors(new Vector2(0.5f, 0.5f)); // Center middle for view mode
        SetScrollViewPosition(viewModePosY);
        SetScrollViewHeight(viewModeHeight);

        // Load saved notes
        LoadNotes();
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

            // Set height, position, and anchors to edit mode settings
            SetScrollViewAnchors(new Vector2(0.5f, 1f)); // Center top for edit mode
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

            // Optionally, save the text to PlayerPrefs or another local storage
            PlayerPrefs.SetString("UserNotes", inputField.text);
            PlayerPrefs.Save();

            // Set height, position, and anchors to view mode settings
            SetScrollViewAnchors(new Vector2(0.5f, 0.5f)); // Center middle for view mode
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

    void SetScrollViewAnchors(Vector2 anchor)
    {
        // Adjust the anchors of the ScrollView
        scrollViewRectTransform.anchorMin = anchor;
        scrollViewRectTransform.anchorMax = anchor;
    }

    void LoadNotes()
    {
        // Optionally, load the text from PlayerPrefs or another local storage
        if (PlayerPrefs.HasKey("UserNotes"))
        {
            TextMeshProUGUI text = notesText.GetComponent<TextMeshProUGUI>();
            text.text = PlayerPrefs.GetString("UserNotes");
        }
    }
}
