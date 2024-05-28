using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneSwitcher : MonoBehaviour
{
    public void SwitchToChatbotScene()
    {
        StopARSessionIfNeeded();
        SceneManager.LoadScene("chatbot");
    }

    public void SwitchToTestScene()
    {
        SceneManager.LoadScene(2);
    }

    public void SwitchToMainScene()
    {
        SceneManager.LoadScene("SampleScene");
        Invoke("RestartARSessionIfNeeded", 0.5f); // Delay to allow scene load
    }

    private void RestartARSessionIfNeeded()
    {
        ARSessionManager arSessionManager = FindObjectOfType<ARSessionManager>();
        if (arSessionManager != null)
        {
            arSessionManager.RestartARSession();
        }
    }

    private void StopARSessionIfNeeded()
    {
        ARSessionManager arSessionManager = FindObjectOfType<ARSessionManager>();
        if (arSessionManager != null)
        {
            arSessionManager.StopARSession();
        }
    }
}
