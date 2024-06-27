using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionManager : MonoBehaviour
{
    private ARSession arSession;
    private ARSessionOrigin arSessionOrigin;

    void Awake()
    {
        arSession = FindObjectOfType<ARSession>();
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    public void RestartARSession()
    {
        StartCoroutine(RestartSessionCoroutine());
    }

    public void StopARSession()
    {
        if (arSession != null)
        {
            arSession.Reset();
        }
    }

    private IEnumerator RestartSessionCoroutine()
    {
        if (arSession != null)
        {
            arSession.Reset();
        }

        yield return null;

        if (arSessionOrigin != null)
        {
            arSessionOrigin.gameObject.SetActive(false);
            yield return null;
            arSessionOrigin.gameObject.SetActive(true);
        }
    }
}
