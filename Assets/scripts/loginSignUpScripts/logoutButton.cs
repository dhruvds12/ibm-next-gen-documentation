using UnityEngine;
using UnityEngine.SceneManagement;

public class logoutButton : MonoBehaviour
{
    public void OnLogoutButton()
    {
        SceneManager.LoadScene("WelcomeScene");
    }

}
