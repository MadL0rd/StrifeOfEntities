
using UnityEngine;
using UnityEngine.UI;

namespace GameSparksTutorials
{
    public class Login : MonoBehaviour
    {
        [SerializeField]
        private InputField username;

        [SerializeField]
        private InputField password;

        private SceneLoading SceneLoader;
        private bool firstAuthenticationTry;

        Login()
        {
            firstAuthenticationTry = true;
            string storedUsername = PlayerPrefs.GetString("login");
            string storedPassword = PlayerPrefs.GetString("password");

            EventManager.StartListening<string>("OnLoginResponse", OnLoginResponse);
            GS_Authentication.Login(storedUsername, storedPassword, "OnLoginResponse");
        }

        public void UserLogin()
        {
            firstAuthenticationTry = false;
            PlayerPrefs.SetString("login", username.text);
            PlayerPrefs.SetString("password", password.text);

            EventManager.StartListening<string>("OnLoginResponse", OnLoginResponse);
            GS_Authentication.Login(username.text, password.text, "OnLoginResponse");
        }

        public void SignUp()
        { 
            UIController.SetActivePanel(UI_Element.SignUp);
        }

        private void OnLoginResponse(string displayName)
        {
            if (displayName.Length > 0)
            {
                DataController.SaveValue("displayName", displayName);
                PlayerLog.GlobalMessage("Login success");
                if (firstAuthenticationTry)
                {
                    SceneLoader.LoadMenuScene();
                }
                else
                {
                    UIController.SetActivePanel(UI_Element.MainMenu);
                }
            }
            else
            {
                Debug.Log("Error OnLoginResponse");
                PlayerLog.GlobalMessage("Login error");
            }

            EventManager.StopListening<string>("OnLoginResponse", OnLoginResponse);
        }
    }
}