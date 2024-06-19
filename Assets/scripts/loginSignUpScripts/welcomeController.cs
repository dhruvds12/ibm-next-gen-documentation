using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WelcomeController : MonoBehaviour
{
    public Button loginButton;
    public Button signUpButton;

    void Start()
    {
        loginButton.onClick.AddListener(() => LoadLoginSignUpScene(true));
        signUpButton.onClick.AddListener(() => LoadLoginSignUpScene(false));
    }

    void LoadLoginSignUpScene(bool isLogin)
    {
        PlayerPrefs.SetInt("IsLoginMode", isLogin ? 1 : 0);
        SceneManager.LoadScene("LoginSignUpScene");
    }
}
