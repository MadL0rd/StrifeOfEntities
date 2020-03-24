
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

        public void UserLogin()
        {
            EventManager.StartListening<string>("OnLoginResponse", OnLoginResponse);

            GS_Authentication.Login(username.text, password.text, "OnLoginResponse");
        }

        public void GuestLogin()
        {
            EventManager.StartListening<string>("OnGuestLoginResponse", OnGuestLoginResponse);

            GS_Authentication.DeviceAuthentication("OnGuestLoginResponse");
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

                UIController.SetActivePanel(UI_Element.MainMenu);

                PlayerLog.GlobalMessage("Login success");
            } else
            {
                Debug.Log("Error OnLoginResponse");
                PlayerLog.GlobalMessage("Login error");
            }

            EventManager.StopListening<string>("OnLoginResponse", OnLoginResponse);
        }

        private void OnGuestLoginResponse(string displayName)
        {
            if (displayName.Length > 0)
            {
                OnLoginResponse(displayName);

                PopUpMessage.ActivatePopUp(delegate { UIController.SetActivePanel(UI_Element.SignUp); }, "Sign Up to build & make friends!");
            } else
            {
                Debug.Log("Error OnGuestLoginResponse");
            }

            EventManager.StopListening<string>("OnGuestLoginResponse", OnGuestLoginResponse);
        }
    }
}