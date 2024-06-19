using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShareNoteController : MonoBehaviour
{
    public GameObject userListPopup; // Reference to the UserListPopup
    public Button shareButton; // Reference to the ShareButton
    public Transform userListContent; // Reference to the Scroll View Content
    public GameObject userButtonPrefab; // Reference to the Button prefab for users

    private string userId; // logged in userId
    private ApiManager apiManager;

    void Start()
    {
        userId = PlayerPrefs.GetString("userId", null); // Initialize userId in Start
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID not found. Please log in.");
            return; // Prevent further execution if userId is not set
        }

        shareButton.onClick.AddListener(OpenUserListPopup);
        apiManager = FindObjectOfType<ApiManager>();
        userListPopup.SetActive(false);
    }

    void OpenUserListPopup()
    {
        userListPopup.SetActive(true);
        StartCoroutine(apiManager.GetUsers(OnUsersLoaded));
    }

    void OnUsersLoaded(List<ApiManager.User> users)
    {
        // Clear existing user buttons
        foreach (Transform child in userListContent)
        {
            Destroy(child.gameObject);
        }

        // Populate the user list with buttons
        foreach (var user in users)
        {
            if (user.userId == userId) // Skip the current user
                continue;

            GameObject userButton = Instantiate(userButtonPrefab, userListContent);
            TextMeshProUGUI buttonText = userButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = user.username;
            userButton.GetComponent<Button>().onClick.AddListener(() => ShareNoteWithUser(user.userId));
        }
    }

    void ShareNoteWithUser(string targetUserId)
    {
        StartCoroutine(apiManager.ShareNote(userId, "image1", "general_note", targetUserId));
        //userListPopup.SetActive(false); // Close the popup after sharing
    }
}
