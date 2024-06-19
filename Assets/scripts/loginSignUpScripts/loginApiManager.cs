using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class loginApiManager : MonoBehaviour
{
    private const string BaseUrl = "http://aws-server.ddns.net:3000";

    public IEnumerator Login(string username, string password, System.Action<string> callback)
    {
        string url = $"{BaseUrl}/login";
        var postData = new { username = username, password = password };
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
                callback(null);
            }
            else
            {
                var response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);
                callback(response.userId);
            }
        }
    }

    public IEnumerator SignUp(string username, string email, string password, System.Action<string> callback)
    {
        string url = $"{BaseUrl}/signup";
        var postData = new { username = username, email = email, password = password };
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
                callback(null);
            }
            else
            {
                var response = JsonConvert.DeserializeObject<SignUpResponse>(request.downloadHandler.text);
                callback(response.userId);
            }
        }
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string userId;
    }

    [System.Serializable]
    public class SignUpResponse
    {
        public string userId;
    }
}
