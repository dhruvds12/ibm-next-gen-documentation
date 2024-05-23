/*using System.Collections;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.UI;

public class SampleWebView : MonoBehaviour
{
    public string Url = "file:///android_asset/watsonAssistant.html"; // Update this path to match your hosted or local file
    public Text status;
    WebViewObject webViewObject;

    IEnumerator Start()
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webViewObject.canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
#endif
        webViewObject.Init(
            cb: (msg) =>
            {
                Debug.Log(string.Format("CallFromJS[{0}]", msg));
                status.text = msg;
                status.GetComponent<Animation>().Play();
            },
            err: (msg) =>
            {
                Debug.Log(string.Format("CallOnError[{0}]", msg));
                status.text = msg;
                status.GetComponent<Animation>().Play();
            },
            httpErr: (msg) =>
            {
                Debug.Log(string.Format("CallOnHttpError[{0}]", msg));
                status.text = msg;
                status.GetComponent<Animation>().Play();
            },
            ld: (msg) =>
            {
                Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS
                var js = @"
                    if (!(window.webkit && window.webkit.messageHandlers)) {
                        window.Unity = {
                            call: function(msg) {
                                window.location = 'unity:' + msg;
                            }
                        };
                    }
                ";
#elif UNITY_WEBPLAYER || UNITY_WEBGL
                var js = @"
                    window.Unity = {
                        call:function(msg) {
                            parent.unityWebView.sendMessage('WebViewObject', msg);
                        }
                    };
                ";
#else
                var js = "";
#endif
                webViewObject.EvaluateJS(js + @"Unity.call('ua=' + navigator.userAgent)");
            }
        );

        // Set margins to cover the entire screen
        webViewObject.SetMargins(0, 100, 0, 0);
        webViewObject.SetTextZoom(100);
        webViewObject.SetVisibility(true);

#if !UNITY_WEBPLAYER && !UNITY_WEBGL
        if (Url.StartsWith("http"))
        {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        }
        else
        {
            var src = System.IO.Path.Combine(Application.streamingAssetsPath, "watsonAssistant.html");
            var dst = System.IO.Path.Combine(Application.temporaryCachePath, "watsonAssistant.html");
            byte[] result = null;

            if (src.Contains("://"))  // for Android
            {
#if UNITY_2018_4_OR_NEWER
                var unityWebRequest = UnityWebRequest.Get(src);
                yield return unityWebRequest.SendWebRequest();
                result = unityWebRequest.downloadHandler.data;
#else
                var www = new WWW(src);
                yield return www;
                result = www.bytes;
#endif
            }
            else
            {
                result = System.IO.File.ReadAllBytes(src);
            }
            System.IO.File.WriteAllBytes(dst, result);
            webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
        }
#else
        if (Url.StartsWith("http"))
        {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        }
        else
        {
            webViewObject.LoadURL("StreamingAssets/watsonAssistant.html");
        }
#endif
        yield break;
    }

    void OnDestroy()
    {
        if (webViewObject != null)
        {
            Destroy(webViewObject.gameObject); // Correctly destroy the GameObject
        }
    }
}
*/