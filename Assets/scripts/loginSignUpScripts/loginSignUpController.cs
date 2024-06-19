using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginSignUpController : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button submitButton;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI statusText;
    public Button backButton;

    private loginApiManager apiManager;
    private bool isLoginMode;

    void Start()
    {
        apiManager = FindObjectOfType<loginApiManager>();

        isLoginMode = PlayerPrefs.GetInt("IsLoginMode", 1) == 1;

        titleText.text = isLoginMode ? "Login" : "Sign Up";
        emailInput.gameObject.SetActive(!isLoginMode);
        submitButton.onClick.AddListener(OnSubmit);
        backButton.onClick.AddListener(OnBackButton);
    }

    void OnSubmit()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        if (isLoginMode)
        {
            StartCoroutine(apiManager.Login(username, password, OnLoginResponse));
        }
        else
        {
            string email = emailInput.text;
            StartCoroutine(apiManager.SignUp(username, email, password, OnSignUpResponse));
        }
    }

    void OnLoginResponse(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            PlayerPrefs.SetString("userId", userId);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            statusText.text = "Login failed. Please check your credentials.";
        }
    }

    void OnSignUpResponse(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            PlayerPrefs.SetString("userId", userId);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            statusText.text = "Sign up failed. Please try again.";
        }
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("WelcomeScene");
    }
}
