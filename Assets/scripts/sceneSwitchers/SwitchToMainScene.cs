using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcherMainScene: MonoBehaviour
{
    public Button backButton;

    void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(SwitchToARScene);
        }
    }

    void SwitchToARScene()
    {
        SceneManager.LoadScene(0); // Assumes AR scene is at index 0
    }
}
