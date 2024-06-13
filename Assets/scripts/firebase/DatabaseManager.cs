using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    public static FirebaseApp app;
    private DatabaseReference reference;

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
                // Manually create the FirebaseApp configuration
                AppOptions options = new AppOptions
                {
                    ApiKey = "AIzaSyCrrLT1jqB_jO_kCfuq-JqwKs--9urAAkA",
                    AppId = "1:811672755547:android:209fcc46e8aebc9fd48e7e",
                    ProjectId = "ibm-ar-cb075",
                    DatabaseUrl = new System.Uri("https://ibm-ar-cb075-default-rtdb.europe-west1.firebasedatabase.app"),
                    StorageBucket = "ibm-ar-cb075.appspot.com",
                };

                // Create the FirebaseApp instance
                app = FirebaseApp.Create(options);

                // Get the database reference
                reference = FirebaseDatabase.GetInstance(app).RootReference;

                // Log success
                Debug.Log("Firebase Initialized Successfully.");

                // Perform your database operations
                CreateUser("admin", "admin@ibmar.com");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private void CreateUser(string name, string email)
    {
        Debug.Log("Creating User...");
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        reference.Child("users").Child("test2").SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User created successfully.");
            }
            else
            {
                Debug.LogError("Failed to create user: " + task.Exception);
            }
        });
    }
}
