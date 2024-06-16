using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{

    // TODO change to https for added security
    // TODO change the player settings to disallow http requests
    private const string BaseUrl = "http://aws-server.ddns.net:3000";

    public IEnumerator PostNote(string userId, string noteKey, string noteContent)
    {
        string url = $"{BaseUrl}/notes";
        
        PostData postData = new PostData(userId, noteKey, noteContent);

        Debug.Log("Sending data to server...");
        Debug.Log("postData: " + postData);

        string jsonData = JsonUtility.ToJson(postData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetNote(string userId, string noteKey, System.Action<string> callback)
    {
        string url = $"{BaseUrl}/notes/{userId}/{noteKey}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);

                // Deserialize the JSON response
                NoteResponse noteResponse = JsonUtility.FromJson<NoteResponse>(request.downloadHandler.text);
                if (noteResponse != null && noteResponse.noteContent != null)
                {
                    callback?.Invoke(noteResponse.noteContent);
                }
                else
                {
                    Debug.LogError("Invalid response format.");
                }
            }
        }
    }
}


[System.Serializable]
public class NoteResponse
{
    public string noteContent;
}


[System.Serializable]
public class PostData
{
    public string userId;
    public string noteKey;
    public string noteContent;

    public PostData(string userId, string noteKey, string noteContent)
    {
        this.userId = userId;
        this.noteKey = noteKey;
        this.noteContent = noteContent;
    }
}