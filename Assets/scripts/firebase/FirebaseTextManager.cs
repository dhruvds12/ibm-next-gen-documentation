using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;

public class FirebaseTextManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI displayText;

    private DatabaseReference databaseReference;
    private FirebaseApp app;
    public bool isFirebaseInitialized = false; // Add a flag to check if Firebase is initialized

    void Start()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;

                // Set the Database URL
                FirebaseDatabase.GetInstance(app).SetPersistenceEnabled(true);
                FirebaseDatabase database = FirebaseDatabase.GetInstance(app, "https://ibm-ar-cb075-default-rtdb.europe-west1.firebasedatabase.app/");

                databaseReference = database.RootReference;
                isFirebaseInitialized = true; // Set the flag to true

                // Load text on start
                LoadText();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                displayText.text = $"Could not resolve all Firebase dependencies: {dependencyStatus}";
            }
        });
    }

    public void SaveText()
    {
        if (!isFirebaseInitialized)
        {
            displayText.text = "Firebase is not initialized.";
            return;
        }

        string userId = "test"; // For testing purposes
        string textData = inputField.text;

        databaseReference.Child("users").Child(userId).Child("notes").SetValueAsync(textData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Text saved successfully.");
                displayText.text = "Text saved successfully.";
            }
            else
            {
                displayText.text = "Failed to save text: " + task.Exception;
                Debug.LogError("Failed to save text: " + task.Exception);
            }
        });
    }

    public void LoadText()
    {
        if (!isFirebaseInitialized)
        {
            displayText.text = "Firebase is not initialized.";
            return;
        }

        string userId = "test"; // For testing purposes

        databaseReference.Child("users").Child(userId).Child("notes").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string textData = snapshot.Value.ToString();
                    displayText.text = textData.Replace("\\n", "\n"); // Replace escaped newline characters with actual newlines
                }
                else
                {
                    displayText.text = "No text found for this user.";
                    Debug.Log("No text found for this user.");
                }
            }
            else
            {
                displayText.text = "Failed to load text: " + task.Exception;
                Debug.LogError("Failed to load text: " + task.Exception);
            }
        });
    }
}
