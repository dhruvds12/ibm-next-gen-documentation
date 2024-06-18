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

    private ApiManager apiManager;

    void Start()
    {
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
            if (user.userId == "8e005a538ebadc51ac94cf080ca3d5ba") // Skip the current user
                continue;

            GameObject userButton = Instantiate(userButtonPrefab, userListContent);
            TextMeshProUGUI buttonText = userButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = user.username;
            userButton.GetComponent<Button>().onClick.AddListener(() => ShareNoteWithUser(user.userId));
        }
    }

    void ShareNoteWithUser(string userId)
    {
        StartCoroutine(apiManager.ShareNote("8e005a538ebadc51ac94cf080ca3d5ba", "image1", "general_note", userId));
        userListPopup.SetActive(false); // Close the popup after sharing
    }
}
