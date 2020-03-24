
using System.Collections.Generic;
using UnityEngine;

namespace GameSparksTutorials
{
    public enum UI_Element
    {
        Login,
        MainMenu,
        SignUp
    }

    public class UIController : AMonoBehaviourSingleton<UIController>
    {

        // Panels
        public Dictionary<UI_Element, GameObject> Panels
        {
            get
            {
                if (panels != null)
                {
                    return panels;
                }
                else
                {
                    panels = new Dictionary<UI_Element, GameObject>();
                    FindAndAddAllPanels();

                    return panels;
                }
            }
        }

        // Additionals

        public GameObject BackgroundBlur;

        public GameObject LoadingCircle;

        public List<GameObject> HideInEditor = new List<GameObject>();

        public UI_Element ActivePanel = UI_Element.Login;

        private Dictionary<UI_Element, GameObject> panels;

        public static void SetLoadingScreenActive(bool value)
        {
            Instance.BackgroundBlur.SetActive(value);
            Instance.LoadingCircle.SetActive(value);
        }

        public static void SetActivePanel(UI_Element element)
        {
            foreach (var panel in Instance.Panels.Values)
            {
                if (panel) panel.SetActive(false);
            }

            if (Instance.Panels[element]) Instance.Panels[element].SetActive(true);
        }

        /// <summary>
        /// 0 = MainMenu
        /// 1 = Login
        /// </summary>
        /// <param name="element"></param>
        public void SetActivePanel(int element)
        {
            var elementAsEnum = (UI_Element)element;
            if (element == 0)
            {
                GameSparks.Core.GS.Reset();
            }
            SetActivePanel(elementAsEnum);
        }

        private void Awake()
        {
            // Activate Hide in Editor elements
            foreach (var go in HideInEditor)
            {
                go.SetActive(true);
            }

            // Activate Login Panel
            SetActivePanel(UI_Element.Login);
        }

        private void FindAndAddAllPanels()
        {
            panels.Clear();
            panels.Add(UI_Element.Login, Instance.transform.Find("Login").gameObject);
            panels.Add(UI_Element.MainMenu, Instance.transform.Find("MainMenu").gameObject);
            panels.Add(UI_Element.SignUp, Instance.transform.Find("SignUp").gameObject);
        }
    }
}