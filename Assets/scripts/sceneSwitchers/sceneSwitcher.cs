using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToChatbotScene()
    {
        SceneManager.LoadScene("chatbot");
    }

    public void SwitchToTestScene()
    {
        SceneManager.LoadScene(2);
    }

    public void SwitchToMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
