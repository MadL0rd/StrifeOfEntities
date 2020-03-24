
using UnityEngine;
using UnityEngine.UI;

namespace GameSparksTutorials
{
    public class SignUp : MonoBehaviour
    {
        [SerializeField]
        InputField username;

        [SerializeField]
        InputField displayName;

        [SerializeField]
        InputField password;

        [SerializeField]
        InputField email;

        public void UserSignUp()
        {
            if (username.text != "" && displayName.text != "" && password.text != "" && email.text != "" && email.text.Contains("@"))
            {
                EventManager.StartListening<string>("OnSignUpResponse", OnSignUpResponse);

                GS_Authentication.SignUp(username.text, displayName.text, password.text, email.text, "OnSignUpResponse");
            }
            else
            {
                PlayerLog.GlobalMessage("Incorrect data!");
            }
        }

        private void OnSignUpResponse(string displayName)
        {
            DataController.SaveValue("displayName", displayName);

            UIController.SetActivePanel(UI_Element.MainMenu);

            EventManager.StopListening<string>("OnSignUpResponse", OnSignUpResponse);
        }
    }
}