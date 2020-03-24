
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSparksTutorials
{
    public class PopUpMessage : AMonoBehaviourSingleton<PopUpMessage>
    {
        [SerializeField]
        private Text text;

        [SerializeField]
        private Button btnAccept;

        [SerializeField]
        private Button btnDecline;

        private UnityAction lastAction;

        /// <summary>
        /// Activate the PopUp and set the button action and text.
        /// </summary>
        /// <param name="action">Action that will be triggered, when the button is pressed.</param>
        /// <param name="text">Button text.</param>
        /// <param name="background">0 = Neutral blue \n1 = Default positive green \n2 = Negative red</param>
        public static void ActivatePopUp(UnityAction action, string text)
        {
            // Add button listener
            Instance.btnAccept.onClick.AddListener(action);
            Instance.lastAction = action;

            // Change button text
            Instance.text.text = text;

            Instance.gameObject.SetActive(true);
        }

        private void DeactivatePopUpMessage()
        {
            Instance.gameObject.SetActive(false);
            Instance.btnAccept.onClick.RemoveListener(lastAction);
            Instance.btnDecline.onClick.RemoveListener(lastAction);
        }

        private void Awake()
        {
            Instance.btnAccept.onClick.AddListener(DeactivatePopUpMessage);
            Instance.btnDecline.onClick.AddListener(DeactivatePopUpMessage);

            Instance.gameObject.SetActive(false);
        }
    }
}