using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ApiManager : MonoBehaviour
{
    // url to ddns
    private string baseUrl = "aws-server.ddns.net:3000";

    public TMP_InputField inputField;
    public TextMeshProUGUI displayText;

    void Start()
    {
        // Load existing notes on start
        // Using coroutine to wait for the response from the server
        // Coroutine is a function that can pause its execution and return control to Unity but then continue where it left off on the following frame
        StartCoroutine(GetNotes());
    }

    public IEnumerator CreateOrUpdateNote()
    {
        string url = $"{baseUrl}/notes";
        Note note = new Note { title = "My Note", content = inputField.text };
        string jsonData = JsonUtility.ToJson(note);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Note saved successfully: " + webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetNotes()
    {
        string url = $"{baseUrl}/notes";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Notes received: " + webRequest.downloadHandler.text);
                Note[] notes = JsonUtility.FromJson<NoteArray>("{\"notes\":" + webRequest.downloadHandler.text + "}").notes;

                // TODO update this to handle multiple notes 
                // TODO reponse will change when server properly implemented
                // Assuming only one note for simplicity
                if (notes.Length > 0)
                {
                    Note note = notes[0];
                    displayText.text = note.content;
                    inputField.text = note.content;
                }
            }
        }
    }
}

[System.Serializable]
public class Note
{
    public string title;
    public string content;
}

[System.Serializable]
public class NoteArray
{
    public Note[] notes;
}
