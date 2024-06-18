using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ApiManager : MonoBehaviour
{
    private const string BaseUrl = "http://aws-server.ddns.net:3000";

    public IEnumerator PostNote(string userId, string imageName, string noteKey, string noteContent)
    {
        string url = $"{BaseUrl}/notes";
        var postData = new PostNoteData
        {
            userId = userId,
            imageName = imageName,
            noteKey = noteKey,
            noteContent = noteContent
        };

        string jsonData = JsonConvert.SerializeObject(postData);

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

    public IEnumerator GetNote(string userId, string imageName, string noteKey, System.Action<string> callback)
    {
        string url = $"{BaseUrl}/notes/{userId}/{imageName}/{noteKey}";

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
                var note = JsonConvert.DeserializeObject<NoteResponse>(request.downloadHandler.text);
                callback?.Invoke(note.noteContent);
            }
        }
    }

    public IEnumerator GetSharedNotes(string userId, string imageName, string noteKey, System.Action<List<string>> callback)
    {
        string url = $"{BaseUrl}/sharedNotes/{userId}/{imageName}/{noteKey}";

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
                var sharedNotes = JsonConvert.DeserializeObject<List<string>>(request.downloadHandler.text);
                callback?.Invoke(sharedNotes);
            }
        }
    }

    public IEnumerator ShareNote(string userId, string imageName, string noteKey, string shareWithUserId)
    {
        string url = $"{BaseUrl}/share";
        var postData = new ShareNoteData
        {
            userId = userId,
            imageName = imageName,
            noteKey = noteKey,
            shareWithUserId = shareWithUserId
        };

        string jsonData = JsonConvert.SerializeObject(postData);

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

    [System.Serializable]
    public class NoteResponse
    {
        public string noteContent;
    }

    [System.Serializable]
    public class PostNoteData
    {
        public string userId;
        public string imageName;
        public string noteKey;
        public string noteContent;
    }

    [System.Serializable]
    public class ShareNoteData
    {
        public string userId;
        public string imageName;
        public string noteKey;
        public string shareWithUserId;
    }
}
