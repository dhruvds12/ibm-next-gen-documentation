using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseTest : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private FirebaseApp app;

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

                // Send data to Firebase
                SendDataToFirebase();
                Debug.Log("Firebase is ready to use.");
            }
            else
            {
                Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    void SendDataToFirebase()
    {
        string data = "testing123";
        databaseReference.SetValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data sent successfully.");
            }
            else
            {
                Debug.Log("Failed to send data: " + task.Exception);
            }
        });
    }
}
